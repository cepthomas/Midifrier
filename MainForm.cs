using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using NAudio.Midi;
using Ephemera.NBagOfTricks;
using Ephemera.NBagOfUis;
using Ephemera.MidiLib;
using Ephemera.MidiLibEx;


namespace Midifrier
{
    public partial class MainForm : Form
    {
        #region Types
        public enum ExplorerState { Stop, Play, Rewind, Complete }
        #endregion

        #region Fields
        /// <summary>My logger.</summary>
        readonly Logger _logger = LogManager.CreateLogger("MainForm");

        /// <summary>Current global user settings.</summary>
        readonly UserSettings _settings;

        /// <summary>Current file.</summary>
        string _fn = "";

        /// <summary>All the channel controls.</summary>
        readonly List<ChannelControl> _channelControls = [];

        /// <summary>The fast timer.</summary>
        readonly MmTimerEx _mmTimer = new();

        /// <summary>Midi events from the input file.</summary>
        MidiDataFile _mdata = new();

        /// <summary>Constant.</summary>
        const int DEFAULT_TEMPO = 100;

        /// <summary>Drums may be on unusual channel.</summary>
        int _drumChannel = MidiDefs.DEFAULT_DRUM_CHANNEL;

        /// <summary>Not used currently.</summary>
        PatternInfo? _currentPattern = null;
        #endregion

        #region Lifecycle
        /// <summary>
        /// Constructor.
        /// </summary>
        public MainForm()
        {
            // Must do this first before initializing.
            string appDir = MiscUtils.GetAppDataDir("Midifrier", "Ephemera");
            _settings = (UserSettings)UserSettings.Load(appDir, typeof(UserSettings));

            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            // Init logging.
            LogManager.MinLevelFile = _settings.FileLogLevel;
            LogManager.MinLevelNotif = _settings.NotifLogLevel;
            LogManager.LogMessage += LogManager_LogMessage;
            LogManager.Run(Path.Join(appDir, "log.txt"), 100000);

            // Init main form from settings
            WindowState = FormWindowState.Normal;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(_settings.FormGeometry.X, _settings.FormGeometry.Y);
            Size = new Size(_settings.FormGeometry.Width, _settings.FormGeometry.Height);
            KeyPreview = true; // for routing kbd strokes through OnKeyDown
            SetText();

            // The text output.
            txtViewer.Font = Font;
            txtViewer.WordWrap = true;
            txtViewer.MatchText.Add("ERR", Color.LightPink);
            txtViewer.MatchText.Add("WRN:", Color.Plum);

            // Other UI configs.
            toolStrip.Renderer = new ToolStripCheckBoxRenderer() { SelectedColor = _settings.DrawColor };
            btnAutoplay.Checked = _settings.Autoplay;
            btnLoop.Checked = _settings.Loop;
            sldVolume.DrawColor = _settings.DrawColor;
            sldVolume.Value = _settings.Volume;
            timeBar.DrawColor = _settings.DrawColor;

            // FilTree settings.
            ftree.RootDirs = _settings.RootDirs;
            var s = MidiDataFile.MIDI_FILE_TYPES + MidiDataFile.STYLE_FILE_TYPES;
            ftree.FilterExts = s.SplitByTokens("|;*");
            ftree.IgnoreDirs = _settings.IgnoreDirs;
            ftree.SplitterPosition = _settings.SplitterPosition;
            ftree.SingleClickSelect = _settings.SingleClickSelect;
            ftree.RecentFiles = _settings.RecentFiles;
            ftree.InitTree();

            sldBPM.Resolution = _settings.TempoResolution;
            sldBPM.DrawColor = _settings.DrawColor;
            sldBPM.Value = DEFAULT_TEMPO;

            // Init channels and selectors.
            for (int i = 1; i <= MidiDefs.NUM_CHANNELS; i++)
            {
                cmbDrumChannel.Items.Add(i);
            }
            cmbDrumChannel.SelectedIndex = _drumChannel - 1;

            // Hook up some simple handlers.
            btnRewind.Click += (_, __) => UpdateState(ExplorerState.Rewind);
            btnKillMidi.Click += (_, __) => MidiManager.Instance.Kill();
            btnLogMidi.Checked = _settings.LogMidi;
            btnLogMidi.Click += (_, __) => _settings.LogMidi = btnLogMidi.Checked;
            sldBPM.ValueChanged += (_, __) => SetTimer();
            btnAutoplay.Click += (_, __) => _settings.Autoplay = btnAutoplay.Checked;
            btnLoop.Click += (_, __) => _settings.Loop = btnLoop.Checked;
            btnPlay.Click += Play_Click;
            MenuStrip.MenuActivate += (_, __) => UpdateUi();

            // Run timer.
            SetTimer();
        }

        /// <summary>
        /// Form is legal now. Init things that want to log.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            // Set up output device.
            if (MidiManager.Instance.GetOutputDevice(_settings.OutputDevice) is null)
            {
                _logger.Error($"Invalid midi output device:{_settings.OutputDevice}");
            }

            // Initialize tree from user settings.
            InitNavigator();

            base.OnLoad(e);
        }

        /// <summary>
        /// Clean up on shutdown. Dispose() will get the rest.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            LogManager.Stop();
            UpdateState(ExplorerState.Stop);
            SaveSettings();

            base.OnFormClosing(e);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            // Stop and destroy mmtimer.
            Stop();
            MidiManager.Instance.Kill();

            // Resources.
            _mmTimer.Stop();
            _mmTimer.Dispose();
            DestroyControls();
            MidiManager.Instance.DestroyDevices();

            // Wait a bit in case there are some lingering events.
            System.Threading.Thread.Sleep(100);

            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }
        #endregion

        #region User settings
        /// <summary>
        /// Edit the common options in a property grid.
        /// </summary>
        void Settings_Click(object? sender, EventArgs e)
        {
            GenericListTypeEditor.SetOptions("OutputDevice", MidiOutputDevice.GetAvailableDevices());

            var changes = SettingsEditor.Edit(_settings, "User Settings", 500);

            // Detect changes of interest.
            bool navChange = false;
            bool restart = false;

            foreach (var (name, cat) in changes)
            {
                switch (name)
                {
                    case "OutputDevice":
                    case "DrawColor":
                    case "TempoResolution":
                    case "FileLogLevel":
                    case "NotifLogLevel":
                        restart = true;
                        break;
                }
            }

            if (restart)
            {
                MessageBox.Show("Restart required for device changes to take effect");
            }

            if (navChange)
            {
                InitNavigator();
            }

            SaveSettings();
        }

        /// <summary>
        /// Collect and save user settings.
        /// </summary>
        void SaveSettings()
        {
            _settings.FormGeometry = new Rectangle(Location.X, Location.Y, Width, Height);
            _settings.Volume = sldVolume.Value;
            _settings.Autoplay = btnAutoplay.Checked;
            _settings.Loop = btnLoop.Checked;
            _settings.TempoResolution = (int)sldBPM.Resolution;
            _settings.Volume = sldVolume.Value;
            _settings.Save();
        }
        #endregion

        #region State management
        /// <summary>
        /// General state management.
        /// </summary>
        void UpdateState(ExplorerState state) 
        {
            // Unhook.
            btnPlay.CheckedChanged -= Play_Click;

            switch (state)
            {
                case ExplorerState.Complete:
                    Rewind();
                    if (btnLoop.Checked)
                    {
                        btnPlay.Checked = true;
                        Play();
                    }
                    else
                    {
                        btnPlay.Checked = false;
                        Stop();
                    }
                    break;

                case ExplorerState.Play:
                    btnPlay.Checked = true;
                    Play();
                    break;

                case ExplorerState.Stop:
                    btnPlay.Checked = false;
                    Stop();
                    break;

                case ExplorerState.Rewind:
                    Rewind();
                    break;
            }

            // Rehook.
            btnPlay.CheckedChanged += Play_Click;
        }

        /// <summary>
        /// Play button handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play_Click(object? sender, EventArgs e)
        {
            UpdateState(btnPlay.Checked ? ExplorerState.Play : ExplorerState.Stop);
        }
        #endregion

        #region File handling
        /// <summary>
        /// Common file opener.
        /// </summary>
        /// <param name="fn">The file to open.</param>
        /// <returns>Status.</returns>
        public bool OpenFile(string fn)
        {
            bool ok = true;
            UpdateState(ExplorerState.Stop);

            _logger.Info($"Opening file: {fn}");

            try
            {
                if (!File.Exists(fn))
                {
                    throw new InvalidOperationException($"Invalid file: {fn}");
                }

                var ext = Path.GetExtension(fn).ToLower();
                if (!MidiDataFile.MIDI_FILE_TYPES.Contains(ext) && !MidiDataFile.STYLE_FILE_TYPES.Contains(ext))
                {
                    throw new InvalidOperationException($"Invalid file type: {fn}");
                }

                using (new WaitCursor())
                {
                    // Reset stuff.
                    cmbDrumChannel.SelectedIndex = _drumChannel - 1;
                    _mdata = new MidiDataFile();

                    // Process the file.
                    _mdata.Read(fn, DEFAULT_TEMPO, false);

                    // Init new stuff with contents of file/pattern.
                    lbPatterns.Items.Clear();
                    var pnames = _mdata.GetPatternNames();

                    if (pnames.Any())
                    {
                        pnames.ForEach(pn => { lbPatterns.Items.Add(pn); });
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid file: {fn}");
                    }

                    Rewind();

                    // Default to first.
                    lbPatterns.SelectedIndex = 0;
                    Patterns_SelectedIndexChanged(null, new());

                    ExportMidiMenuItem.Enabled = _mdata.IsStyleFile;

                    // All good so far.
                    _fn = fn;
                    _settings.UpdateMru(fn);

                    if (_settings.Autoplay)
                    {
                        UpdateState(ExplorerState.Rewind);
                        UpdateState(ExplorerState.Play);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Couldn't open the file: {fn} because: {ex.Message}");
                _fn = "";
                ok = false;
            }

            SetText();
            btnPlay.Enabled = ok;

            return ok;
        }

        /// <summary>
        /// Initialize tree from user settings.
        /// </summary>
        void InitNavigator()
        {
            try
            {
                ftree.InitTree();
            }
            catch (DirectoryNotFoundException)
            {
                _logger.Warn("No tree directories");
            }
        }

        /// <summary>
        /// Tree has selected a file to play.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fn"></param>
        void Navigator_FileSelected(object? sender, string fn)
        {
            OpenFile(fn);
        }

        /// <summary>
        /// Organize the file menu item drop down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void File_DropDownOpening(object? sender, EventArgs e)
        {
            RecentMenuItem.DropDownItems.Clear();
            _settings.RecentFiles.ForEach(f =>
            {
                ToolStripMenuItem menuItem = new(f, null, new EventHandler(Recent_Click));
                RecentMenuItem.DropDownItems.Add(menuItem);
            });
        }

        /// <summary>
        /// The user has asked to open a recent file.
        /// </summary>
        void Recent_Click(object? sender, EventArgs e)
        {
            if (sender is not null)
            {
                string fn = sender.ToString()!;
                OpenFile(fn);
            }
        }

        /// <summary>
        /// Allows the user to select an audio clip or midi from file system.
        /// </summary>
        void Open_Click(object? sender, EventArgs e)
        {
            var fileTypes = $"Midi Files|{MidiDataFile.MIDI_FILE_TYPES}|Style Files|{MidiDataFile.STYLE_FILE_TYPES}";
            using OpenFileDialog openDlg = new()
            {
                Filter = fileTypes,
                Title = "Select a file"
            };

            if (openDlg.ShowDialog() == DialogResult.OK && openDlg.FileName != _fn)
            {
                OpenFile(openDlg.FileName);
                _fn = openDlg.FileName;
            }
        }
        #endregion

        #region Play functions
        /// <summary>
        /// 
        /// </summary>
        public void Play()
        {
            _mmTimer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            _mmTimer.Stop();
            MidiManager.Instance.Kill();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Rewind()
        {
            timeBar.Rewind();
        }
        #endregion

        #region Midi send
        /// <summary>
        /// Multimedia timer callback. Synchronously outputs the next midi events.
        /// </summary>
        void MmTimerCallback(double totalElapsed, double periodElapsed)
        {
            try
            {
                // Bump time. Check for end of play.
                if (DoNextStep())
                {
                    this.InvokeIfRequired(_ => { UpdateState(ExplorerState.Complete); });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Synchronously outputs the next midi events. Does solo/mute.
        /// This is running on the background thread.
        /// </summary>
        /// <returns>True if sequence completed.</returns>
        public bool DoNextStep()
        {
            // Update all channels. Any soloes?
            bool anySolo = _channelControls.Where(c => c.State == ChannelState.Solo).Any();

            // Process each channel.
            foreach (var cc in _channelControls)
            {
                var ch = cc!.BoundChannel!;

                // Look for events to send. Any explicit solos?
                if (cc.State == ChannelState.Solo || (!anySolo && cc.State == ChannelState.Normal))
                {
                    // Process any sequence steps.
                    var playEvents = ch.Events.Get(timeBar.Current);

                    foreach (var mevt in playEvents)
                    {
                        switch (mevt)
                        {
                            case NoteOn evt:
                                // Adjust volume.
                                evt.Velocity = MathUtils.Constrain((int)(evt.Velocity * ch.Volume), 0, MidiDefs.MAX_MIDI);
                                // Adjust channel.
                                evt.ChannelNumber =  evt.ChannelNumber == _drumChannel ? MidiDefs.DEFAULT_DRUM_CHANNEL : _drumChannel;
                                ch.Send(evt);
                                break;

                            case NoteOff evt:
                                // Adjust channel.
                                evt.ChannelNumber =  evt.ChannelNumber == _drumChannel ? MidiDefs.DEFAULT_DRUM_CHANNEL : _drumChannel;
                                ch.Send(evt);
                                break;

                            default:
                                // Everything else as is.
                                ch.Send(mevt);
                                break;
                        }
                    }
                }
            }

            // Bump time. Check for end of play.
            bool done = !timeBar.Increment();

            return done;
        }
        #endregion

        #region UI event handlers
        /// <summary>
        /// Do some global key handling. Space bar is used for stop/start playing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    // Toggle.
                    UpdateState(btnPlay.Checked ? ExplorerState.Stop : ExplorerState.Play);
                    e.Handled = true;
                    break;
            }
            base.OnKeyDown(e);
        }

        /// <summary>
        /// The user clicked something in one of the player controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Control_ChannelChange(object? sender, ChannelChangeEventArgs e)
        {
            var cc = sender as ChannelControl;
            var channel = cc!.BoundChannel!;

            if (e.State)
            {
                switch (cc.State)
                {
                    case ChannelState.Normal:
                        break;

                    case ChannelState.Solo:
                        // Mute any other non-solo channels.
                        MidiManager.Instance.OutputChannels.ForEach(ch =>
                        {
                            if (channel.ChannelNumber != ch.ChannelNumber && cc.State != ChannelState.Solo)
                            {
                                MidiManager.Instance.Kill(channel);
                            }
                        });
                        break;

                    case ChannelState.Mute:
                        MidiManager.Instance.Kill(channel);
                        break;
                }
            }
        }

        /// <summary>
        /// All about me.
        /// </summary>
        void About_Click(object? sender, EventArgs e)
        {
            Tools.ShowReadme("Midifrier");
            txtViewer.AppendLine(string.Join(Environment.NewLine, MidiDefs.GenUserDeviceInfo()));
        }

        /// <summary>
        /// Set UI item enables according to system states.
        /// </summary>
        void UpdateUi()
        {
            btnRewind.Enabled = true;
            btnPlay.Enabled = _mdata.GetPatternNames().Any();

            OpenMenuItem.Enabled = true;
            AboutMenuItem.Enabled = true;
            SettingsMenuItem.Enabled = true;
        }
        #endregion

        #region Channel Controls
        /// <summary>
        /// Destroy controls.
        /// </summary>
        void DestroyControls()
        {
            MidiManager.Instance.Kill();

            // Clean out our current elements.
            _channelControls.ForEach(c =>
            {
                c.ChannelChange -= Control_ChannelChange;
                Controls.Remove(c);
                c.Dispose();
            });
            _channelControls.Clear();
        }
        #endregion

        #region Process patterns
        /// <summary>
        /// Load the requested pattern and create controls.
        /// </summary>
        /// <param name="pinfo"></param>
        void LoadPattern(PatternInfo pinfo)
        {
            Stop();
            DestroyControls();
            MidiManager.Instance.DestroyChannels();

            // Load the new one.
            if (pinfo is null)
            {
                _logger.Error($"Invalid pattern!");
                return;
            }
            _currentPattern = pinfo;

            // Create the new controls.
            int x = lblChLoc.Left;
            int y = lblChLoc.Top;
            lblChLoc.Hide();

            sldBPM.Value = pinfo.Tempo;

            long maxTick = 0;

            foreach (var (chnum, patch) in pinfo.GetChannels(true, true))
            {
                // Get events for the channel.
                var midiEvents = pinfo.GetFilteredEvents([chnum]).ToList();

                // Is this channel pertinent?
                if (!midiEvents.Any()) continue;

                // Convert events to internal.
                EventCollection chEvents = new();

                foreach (var mevt in midiEvents)
                {
                    MusicTime when = new(mevt.AbsoluteTime);
                    maxTick = Math.Max(when.Tick, maxTick);

                    switch (mevt)
                    {
                        case NoteOnEvent onevt:
                            chEvents.Add(new NoteOn(chnum, onevt.NoteNumber, onevt.Velocity, when));
                            break;

                        case NoteEvent nevt:
                            if (nevt.CommandCode == MidiCommandCode.NoteOff)
                            {
                                chEvents.Add(new NoteOff(chnum, nevt.NoteNumber, when));
                            }
                            else if (nevt.CommandCode == MidiCommandCode.NoteOn)
                            {
                                chEvents.Add(new NoteOn(chnum, nevt.NoteNumber, nevt.Velocity, when));
                            }
                            break;

                        case PatchChangeEvent pevt:
                            chEvents.Add(new Patch(chnum, pevt.Patch, when));
                            break;

                        case ControlChangeEvent ctlevt:
                            chEvents.Add(new Controller(chnum, (int)ctlevt.Controller, ctlevt.ControllerValue, when));
                            break;

                        default:
                            // As is.
                            chEvents.Add(new Other(chnum, mevt.GetAsShortMessage(), when));
                            break;
                    }
                }

                // Make new channel. Attach corresponding events.
                var channel = MidiManager.Instance.OpenOutputChannel(_settings.OutputDevice, chnum, $"ch{chnum}", patch);
                channel.Events = chEvents;

                // Make new control and bind to channel.
                ChannelControl control = new()
                {
                    Location = new(x, y),
                    BorderStyle = BorderStyle.FixedSingle,
                    BoundChannel = channel,
                    DrawColor = _settings.DrawColor,
                    SelectedColor = _settings.SelectedColor,
                    Options = DisplayOptions.SoloMute
                };
                control.ChannelChange += Control_ChannelChange;
                Controls.Add(control);
                _channelControls.Add(control);

                // Adjust positioning.
                y += control.Height + 5;
            }

            // Set timer.
            sldBPM.Value = pinfo.Tempo;

            // Update bar.
            Dictionary<int, string> sectInfo = [];
            sectInfo.Add(0, "sect1");
            sectInfo.Add((int)maxTick / MusicTime.TicksPerBeat, "END");
            timeBar.Snap = SnapType.FourBar;
            timeBar.GridLines = 4 * MusicTime.TicksPerBar;
            timeBar.InitSectionInfo(sectInfo);
            timeBar.Current.Reset();
            timeBar.Invalidate();
        }

        /// <summary>
        /// Load pattern selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Patterns_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lbPatterns.SelectedItem is not null)
            {
                var pinfo = _mdata.GetPattern(lbPatterns.SelectedItem.ToString()!);

                LoadPattern(pinfo!);

                Rewind();

                if (_settings.Autoplay)
                {
                    Play();
                }
            }
        }

        /// <summary>
        /// Pattern selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AllOrNone_Click(object? sender, EventArgs e)
        {
            bool check = sender == btnAllPatterns;
            for (int i = 0; i < lbPatterns.Items.Count; i++)
            {
                lbPatterns.SetItemChecked(i, check);
            }
        }
        #endregion

        #region Drum channel
        /// <summary>
        /// User changed the drum channel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DrumChannel_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _drumChannel = cmbDrumChannel.SelectedIndex + 1;
        }
        #endregion

        #region Export
        /// <summary>
        /// Export current file to human readable or midi.
        /// </summary>
        void Export_Click(object? sender, EventArgs e)
        {
            var stext = ((ToolStripMenuItem)sender!).Text ?? "no text";

            try
            {
                // Get selected patterns.
                List<string> patternNames = [];
                if (lbPatterns.Items.Count == 1)
                {
                    patternNames.Add(lbPatterns.Items[0].ToString()!);
                }
                else if (lbPatterns.CheckedItems.Count > 0)
                {
                    foreach (var p in lbPatterns.CheckedItems)
                    {
                        patternNames.Add(p.ToString()!);
                    }
                }
                else
                {
                    _logger.Warn("Please select at least one pattern");
                    return;
                }

                List<PatternInfo> patterns = [];
                patternNames.ForEach(p => patterns.Add(_mdata.GetPattern(p)!));

                // Get selected channels.
                var selControls = _channelControls.Where(cc => cc.Selected).ToList();
                if (!selControls.Any()) // grab them all.
                {
                    _channelControls.ForEach(cc => selControls.Add(cc));
                }
                List<OutputChannel> channels = [.. selControls.Select(cc => cc.BoundChannel)];

                switch (stext.ToLower())
                {
                    case "export csv":
                        {
                            var newfn = Tools.MakeExportFileName(_settings.ExportFolder, _mdata.FileName, "export", "csv");
                            MidiExport.ExportCsv(newfn, patterns, channels, _mdata.GetGlobal());
                            _logger.Info($"Exported to {newfn}");
                        }
                        break;

                    case "export midi":
                        foreach (var pattern in patterns)
                        {
                            var newfn = Tools.MakeExportFileName(_settings.ExportFolder, _mdata.FileName, pattern.PatternName, "mid");
                            MidiExport.ExportMidi(newfn, pattern, channels, _mdata.GetGlobal());
                            _logger.Info($"Export midi to {newfn}");
                        }
                        break;

                    default:
                        _logger.Error($"Ooops: {stext}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");
            }
        }
        #endregion

        #region Misc functions
        /// <summary>
        /// Show log events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LogManager_LogMessage(object? sender, LogMessageEventArgs e)
        {
            // Usually come from a different thread.
            if (IsHandleCreated)
            {
                this.InvokeIfRequired(_ => { txtViewer.AppendLine($"{e.Message}"); });
            }
        }

        /// <summary>
        /// Convert tempo to period and set mm timer accordingly.
        /// </summary>
        void SetTimer()
        {
            MidiTimeConverter mt = new(_mdata.DeltaTicksPerQuarterNote, sldBPM.Value);
            double period = mt.RoundedInternalPeriod();
            _mmTimer.SetTimer((int)Math.Round(period), MmTimerCallback);
        }

        /// <summary>
        /// Utility for header.
        /// </summary>
        void SetText()
        {
            var s = _fn == "" ? "No file loaded" : _fn;
            Text = $"Midifrier {MiscUtils.GetVersionString()} - {s}";
        }
        #endregion
    }
}
