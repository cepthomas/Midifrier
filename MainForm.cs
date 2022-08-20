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
using NAudio.Midi;
using NAudio.Wave;
using NBagOfTricks;
using NBagOfTricks.Slog;
using NBagOfUis;
using MidiLib;


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

        /// <summary>Current file.</summary>
        string _fn = "";

        /// <summary>Current global user settings.</summary>
        readonly UserSettings _settings;

        /// <summary>Where to put things.</summary>
        readonly string _outPath = "";

        /// <summary>Midi output.</summary>
        readonly IOutputDevice _outputDevice = new NullOutputDevice();

        /// <summary>All the channels - key is user assigned name.</summary>
        readonly Dictionary<string, Channel> _channels = new();

        /// <summary>All the channel controls.</summary>
        readonly List<ChannelControl> _channelControls = new();

        /// <summary>The fast timer.</summary>
        readonly MmTimerEx _mmTimer = new();

        /// <summary>Midi events from the input file.</summary>
        MidiDataFile _mdata = new();
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
            // Tell the libs about their settings.
            MidiSettings.LibSettings = _settings.MidiSettings;

            InitializeComponent();

            Icon = Properties.Resources.zebra;

            // Set up paths.
            _outPath = Path.Combine(appDir, "out");
            DirectoryInfo di = new(_outPath);
            di.Create();

            // Init logging.
            LogManager.MinLevelFile = _settings.FileLogLevel;
            LogManager.MinLevelNotif = _settings.NotifLogLevel;
            LogManager.LogEvent += LogManager_LogEvent;
            LogManager.Run();

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
            txtViewer.MatchColors.Add("ERR", Color.LightPink);
            txtViewer.MatchColors.Add("WRN:", Color.Plum);

            // Other UI configs.
            toolStrip.Renderer = new NBagOfUis.CheckBoxRenderer() { SelectedColor = _settings.ControlColor };


            btnAutoplay.Checked = _settings.Autoplay;


            btnLoop.Checked = _settings.Loop;


            chkPlay.FlatAppearance.CheckedBackColor = _settings.ControlColor;


            sldVolume.DrawColor = _settings.ControlColor;
            sldVolume.Value = _settings.Volume;


            sldTempo.Resolution = _settings.TempoResolution;
            sldTempo.DrawColor = _settings.ControlColor;
            sldTempo.Resolution = _settings.TempoResolution;
            sldTempo.Value = _settings.MidiSettings.DefaultTempo;

            // Init channels and selectors.
            cmbDrumChannel1.Items.Add("NA");
            cmbDrumChannel2.Items.Add("NA");
            for (int i = 1; i <= MidiDefs.NUM_CHANNELS; i++)
            {
                cmbDrumChannel1.Items.Add(i);
                cmbDrumChannel2.Items.Add(i);
            }
            cmbDrumChannel1.SelectedIndex = MidiDefs.DEFAULT_DRUM_CHANNEL;
            cmbDrumChannel2.SelectedIndex = 0;

            barBar.ProgressColor = _settings.ControlColor;


            // Hook up some simple handlers.
            btnRewind.Click += (_, __) => { UpdateState(ExplorerState.Rewind); };
            btnKillMidi.Click += (_, __) => { _channels.Values.ForEach(ch => ch.Kill()); };
            btnLogMidi.Click += (_, __) => { _outputDevice.LogEnable = btnLogMidi.Checked; };
            sldTempo.ValueChanged += (_, __) => { SetTimer(); };
            chkPlay.CheckedChanged += ChkPlay_CheckedChanged;

            // Set up output device.
            foreach (var dev in _settings.MidiSettings.OutputDevices)
            {
                // Try midi.
                _outputDevice = new MidiOutput(dev.DeviceName);
                if (_outputDevice.Valid)
                {
                    break;
                }
            }

            // Run timer.
            SetTimer();
        }

        /// <summary>
        /// Form is legal now. Init things that want to log.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            _logger.Info($"OK to log now!!");

            if (!_outputDevice.Valid)
            {
                _logger.Error($"Something wrong with your output device:{_outputDevice.DeviceName}");
            }

            // Initialize tree from user settings.
            InitNavigator();

            ///// >>> Debug. _drums_ch1.mid  _LoveSong.S474.sty  WICKGAME.MID
            OpenFile(@"C:\Dev\repos\TestAudioFiles\WICKGAME.MID");
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
            // Resources.
            //_midiExplorer?.Dispose();

            // Stop and destroy mmtimer.
            Stop();

            // Resources.
            _mmTimer.Stop();
            _mmTimer.Dispose();

            _outputDevice.Dispose();

            // Wait a bit in case there are some lingering events.
            System.Threading.Thread.Sleep(100);


            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }
        #endregion

        #region State management
        /// <summary>
        /// General state management.
        /// </summary>
        void UpdateState(ExplorerState state) 
        {
            if (_outputDevice.Valid)
            {
                // Unhook.
                chkPlay.CheckedChanged -= ChkPlay_CheckedChanged;

                try
                {
                    switch (state)
                    {
                        case ExplorerState.Complete:
                            Rewind();
                            if (btnLoop.Checked)
                            {
                                chkPlay.Checked = true;
                                Play();
                            }
                            else
                            {
                                chkPlay.Checked = false;
                                Stop();
                            }
                            break;

                        case ExplorerState.Play:
                            chkPlay.Checked = true;
                            Play();
                            break;

                        case ExplorerState.Stop:
                            chkPlay.Checked = false;
                            Stop();
                            break;

                        case ExplorerState.Rewind:
                            Rewind();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    // Rehook.
                    chkPlay.CheckedChanged += ChkPlay_CheckedChanged;
                }
            }
        }

        /// <summary>
        /// Play button handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkPlay_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateState(chkPlay.Checked ? ExplorerState.Play : ExplorerState.Stop);
        }
        #endregion

        #region File handling
        /// <summary>
        /// Common file opener.
        /// </summary>
        /// <param name="fn">The file to open.</param>
        /// <returns>Status.</returns>
        public bool OpenFile(string fn) // TODO needs cleanup.
        {
            bool ok = true;

            UpdateState(ExplorerState.Stop);

            _logger.Info($"Opening file: {fn}");

            using (new WaitCursor())
            {
                try
                {
                    var ext = Path.GetExtension(fn).ToLower();

                    if (MidiLibDefs.MIDI_FILE_TYPES.Contains(ext) || MidiLibDefs.STYLE_FILE_TYPES.Contains(ext))
                    {
                    }
                    else
                    {
                        _logger.Error($"Invalid file type: {fn}");
                        ok = false;
                    }

                    if (ok)
                    {
                        // from midiexplorer:
                        try
                        {
                            // Reset stuff.
                            cmbDrumChannel1.SelectedIndex = MidiDefs.DEFAULT_DRUM_CHANNEL;
                            cmbDrumChannel2.SelectedIndex = 0;
                            _mdata = new MidiDataFile();

                            // Process the file. Set the default tempo from preferences.
                            _mdata.Read(fn, _settings.MidiSettings.DefaultTempo, false);

                            // Init new stuff with contents of file/pattern.
                            lbPatterns.Items.Clear();
                            var pnames = _mdata.GetPatternNames();

                            if (pnames.Count > 0)
                            {
                                pnames.ForEach(pn => { lbPatterns.Items.Add(pn); });
                            }
                            else
                            {
                                throw new InvalidOperationException($"Something wrong with this file: {fn}");
                            }

                            Rewind();

                            // Pick first.
                            lbPatterns.SelectedIndex = 0;

                            // Set up timer default.
                            sldTempo.Value = 100;

                            ExportMidiMenuItem.Enabled = _mdata.IsStyleFile;
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Couldn't open the file: {fn} because: {ex.Message}");
                            ok = false;
                        }


                        if(ok)
                        {
                            _fn = fn;
                            _settings.RecentFiles.UpdateMru(fn);
                        }

                        SetText();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Couldn't open the file: {fn} because: {ex.Message}");
                    _fn = "";
                    SetText();
                    ok = false;
                }
            }

            chkPlay.Enabled = ok;

            if (ok)
            {
                if (_settings.Autoplay)
                {
                    UpdateState(ExplorerState.Rewind);
                    UpdateState(ExplorerState.Play);
                }
            }

            return ok;
        }

        /// <summary>
        /// Initialize tree from user settings.
        /// </summary>
        void InitNavigator()
        {
            var s = MidiLibDefs.MIDI_FILE_TYPES + MidiLibDefs.STYLE_FILE_TYPES;
            ftree.FilterExts = s.SplitByTokens("|;*");
            ftree.RootDirs = _settings.RootDirs;
            ftree.SingleClickSelect = true;

            try
            {
                ftree.Init();
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
        void Navigator_FileSelectedEvent(object? sender, string fn)
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
            FileMenuItem.DropDownItems.Clear();

            FileMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                OpenMenuItem,
                toolStripSeparator2,
                ExportCsvMenuItem,
                ExportMidiMenuItem
            });

            if(_settings.RecentFiles.Count > 0)
            {
                FileMenuItem.DropDownItems.Add(new ToolStripSeparator());
                _settings.RecentFiles.ForEach(f =>
                {
                    ToolStripMenuItem menuItem = new(f, null, new EventHandler(Recent_Click));
                    FileMenuItem.DropDownItems.Add(menuItem);
                });
            }
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
            var fileTypes = $"Midi Files|{MidiLibDefs.MIDI_FILE_TYPES}|Style Files|{MidiLibDefs.STYLE_FILE_TYPES}";
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
            // Send midi stop all notes just in case.
            _channels.Values.ForEach(ch => ch.Kill());
        }

        /// <summary>
        /// 
        /// </summary>
        public void Rewind()
        {
            barBar.Current = new(0);
        }
        #endregion

        #region User settings
        /// <summary>
        /// Collect and save user settings.
        /// </summary>
        void SaveSettings()
        {
            _settings.FormGeometry = new Rectangle(Location.X, Location.Y, Width, Height);
            _settings.Volume = sldVolume.Value;
            _settings.Autoplay = btnAutoplay.Checked;
            _settings.Loop = btnLoop.Checked;
            _settings.TempoResolution = (int)sldTempo.Resolution;
            _settings.Volume = sldVolume.Value;
            _settings.Save();
        }

        /// <summary>
        /// Edit the common options in a property grid.
        /// </summary>
        void Settings_Click(object? sender, EventArgs e)
        {
            var changes = _settings.Edit("User Settings", 500);

            // Detect changes of interest.
            bool midiChange = false;
            bool navChange = false;
            bool restart = false;

            foreach (var (name, cat) in changes)
            {
                switch(name)
                {
                    case "InputDevice":
                    case "OutputDevice":
                    case "ControlColor":
                        restart = true;
                        break;

                    case "TempoResolution":
                        midiChange = true;
                        break;

                    case "RootDirs":
                        navChange = true;
                        break;
                }
            }

            if (restart)
            {
                MessageBox.Show("Restart required for device changes to take effect");
            }

            //if (midiChange)
            //{
            //    _midiExplorer?.UpdateSettings();
            //}

            if (navChange)
            {
                InitNavigator();
            }

            // Benign changes.
            btnLoop.Checked = _settings.Loop;

            SaveSettings();
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
            // Any soloes?
            bool anySolo = _channels.AnySolo();

            // Process each channel.
            foreach (var ch in _channels.Values)
            {
                // Look for events to send. Any explicit solos?
                if (ch.State == ChannelState.Solo || (!anySolo && ch.State == ChannelState.Normal))
                {
                    // Process any sequence steps.
                    var playEvents = ch.GetEvents(barBar.Current.TotalSubdivs);
                    foreach (var mevt in playEvents)
                    {
                        switch (mevt)
                        {
                            case NoteOnEvent evt:
                                if (ch.IsDrums && evt.Velocity == 0)
                                {
                                    // Skip drum noteoffs as windows GM doesn't like them.
                                }
                                else
                                {
                                    // Adjust volume. Redirect drum channel to default.
                                    NoteOnEvent ne = new(
                                        evt.AbsoluteTime,
                                        ch.IsDrums ? MidiDefs.DEFAULT_DRUM_CHANNEL : evt.Channel,
                                        evt.NoteNumber,
                                        Math.Min((int)(evt.Velocity * sldVolume.Value * ch.Volume), MidiDefs.MAX_MIDI),
                                        evt.OffEvent is null ? 0 : evt.NoteLength); // Fix NAudio NoteLength bug.

                                    ch.SendEvent(ne);
                                }
                                break;

                            case NoteEvent evt: // aka NoteOff
                                if (ch.IsDrums)
                                {
                                    // Skip drum noteoffs as windows GM doesn't like them.
                                }
                                else
                                {
                                    ch.SendEvent(evt);
                                }
                                break;

                            default:
                                // Everything else as is.
                                ch.SendEvent(mevt);
                                break;
                        }
                    }
                }
            }

            // Bump time. Check for end of play.
            bool done = barBar.IncrementCurrent(1);

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
                    UpdateState(chkPlay.Checked ? ExplorerState.Stop : ExplorerState.Play);
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// The user clicked something in one of the player controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Control_ChannelChangeEvent(object? sender, ChannelChangeEventArgs e)
        {
            Channel channel = ((ChannelControl)sender!).BoundChannel;

            if (e.StateChange)
            {
                switch (channel.State)
                {
                    case ChannelState.Normal:
                        break;

                    case ChannelState.Solo:
                        // Mute any other non-solo channels.
                        _channels.Values.ForEach(ch =>
                        {
                            if (channel.ChannelNumber != ch.ChannelNumber && channel.State != ChannelState.Solo)
                            {
                                channel.Kill();
                            }
                        });
                        break;

                    case ChannelState.Mute:
                        channel.Kill();
                        break;
                }
            }

            if (e.PatchChange && channel.Patch >= 0)
            {
                channel.SendPatch();
            }
        }

        /// <summary>
        /// All about me.
        /// </summary>
        void About_Click(object? sender, EventArgs e)
        {
            MiscUtils.ShowReadme("Midifrier");
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

            // Clean out our current elements.
            _channelControls.ForEach(c =>
            {
                Controls.Remove(c);
                c.Dispose();
            });
            _channelControls.Clear();
            _channels.Clear();

            // Load the new one.
            if (pinfo is null)
            {
                _logger.Error($"Invalid pattern!");
            }
            else
            {
                // Create the new controls.
                int x = lblChLoc.Left;
                int y = lblChLoc.Top;
                lblChLoc.Hide();

                // For scaling subdivs to internal.
                MidiTimeConverter mt = new(_mdata.DeltaTicksPerQuarterNote, _settings.MidiSettings.DefaultTempo);
                sldTempo.Value = pinfo.Tempo;

                foreach (var (number, patch) in pinfo.GetChannels(true, true))
                {
                    // Get events for the channel.
                    var chEvents = pinfo.GetFilteredEvents(new List<int>() { number });

                    // Is this channel pertinent?
                    if (chEvents.Any())
                    {
                        // Make new channel.
                        Channel channel = new()
                        {
                            ChannelName = $"chan{number}",
                            ChannelNumber = number,
                            Device = _outputDevice,
                            DeviceId = _outputDevice.DeviceName,
                            Volume = MidiLibDefs.VOLUME_DEFAULT,
                            State = ChannelState.Normal,
                            Patch = patch,
                            IsDrums = number == MidiDefs.DEFAULT_DRUM_CHANNEL,
                            Selected = false,
                        };
                        _channels.Add(channel.ChannelName, channel);
                        channel.SetEvents(chEvents);

                        // Make new control and bind to channel.
                        ChannelControl control = new()
                        {
                            Location = new(x, y),
                            BorderStyle = BorderStyle.FixedSingle,
                            BoundChannel = channel,
                            SelectedColor = _settings.ControlColor
                        };
                        control.ChannelChangeEvent += Control_ChannelChangeEvent;
                        Controls.Add(control);
                        _channelControls.Add(control);

                        // Good time to send initial patch.
                        channel.SendPatch();

                        // Adjust positioning.
                        y += control.Height + 5;
                    }
                }

                // Set timer.
                sldTempo.Value = pinfo.Tempo;
            }

            // Update bar.
            var tot = _channels.TotalSubdivs();
            barBar.Start = new(0);
            barBar.End = new(tot - 1);
            barBar.Length = new(tot);
            barBar.Current = new(0);

            UpdateDrumChannels();
        }

        /// <summary>
        /// Load pattern selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Patterns_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var pinfo = _mdata.GetPattern(lbPatterns.SelectedItem.ToString()!);

            LoadPattern(pinfo!);

            Rewind();

            if (_settings.Autoplay)
            {
                Play();
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
            UpdateDrumChannels();
        }

        /// <summary>
        /// Update all channels based on current UI.
        /// </summary>
        void UpdateDrumChannels()
        {
            _channelControls.ForEach(ctl => ctl.IsDrums =
                (ctl.ChannelNumber == cmbDrumChannel1.SelectedIndex) ||
                (ctl.ChannelNumber == cmbDrumChannel2.SelectedIndex));
        }
        #endregion

        #region Export
        /// <summary>
        /// Export current file to human readable or midi.
        /// </summary>
        void Export_Click(object? sender, EventArgs e)
        {
            var stext = ((ToolStripMenuItem)sender!).Text;

            try
            {
                // Get selected patterns.
                List<string> patternNames = new();
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
                List<PatternInfo> patterns = new();
                patternNames.ForEach(p => patterns.Add(_mdata.GetPattern(p)!));

                // Get selected channels.
                List<Channel> channels = new();
                _channelControls.Where(cc => cc.Selected).ForEach(cc => channels.Add(cc.BoundChannel));
                if (!channels.Any()) // grab them all.
                {
                    _channelControls.ForEach(cc => channels.Add(cc.BoundChannel));
                }

                switch (stext.ToLower())
                {
                    case "csv":
                        {
                            var newfn = MiscUtils.MakeExportFileName(_outPath, _mdata.FileName, "all", "csv");
                            MidiExport.ExportCsv(newfn, patterns, channels, _mdata.GetGlobal());
                            _logger.Info($"Exported to {newfn}");
                        }
                        break;

                    case "midi":
                        foreach (var pattern in patterns)
                        {
                            var newfn = MiscUtils.MakeExportFileName(_outPath, _mdata.FileName, pattern.PatternName, "mid");
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
        void LogManager_LogEvent(object? sender, LogEventArgs e)
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
            MidiTimeConverter mt = new(_mdata.DeltaTicksPerQuarterNote, sldTempo.Value);
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

        /// <summary>
        /// Debug stuff.
        /// </summary>
        void DumpDevices()
        {
            for (int id = -1; id < WaveOut.DeviceCount; id++)
            {
                var cap = WaveOut.GetCapabilities(id);
                _logger.Info($"WaveOut {id} {cap.ProductName}");
            }
            for (int id = -1; id < WaveIn.DeviceCount; id++)
            {
                var cap = WaveIn.GetCapabilities(id);
                _logger.Info($"WaveIn {id} {cap.ProductName}");
            }
        }

        #endregion
    }
}
