namespace Midifrier
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            toolStrip = new System.Windows.Forms.ToolStrip();
            btnAutoplay = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            btnLoop = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            btnLogMidi = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            btnKillMidi = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            cmbDrumChannel1 = new System.Windows.Forms.ToolStripComboBox();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            cmbDrumChannel2 = new System.Windows.Forms.ToolStripComboBox();
            toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            sldVolume = new Ephemera.NBagOfUis.ToolStripSlider();
            toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            sldBPM = new Ephemera.NBagOfUis.ToolStripSlider();
            toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            btnPlay = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            btnRewind = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            txtViewer = new Ephemera.NBagOfUis.TextViewer();
            toolTip = new System.Windows.Forms.ToolTip(components);
            lbPatterns = new System.Windows.Forms.CheckedListBox();
            btnAllPatterns = new System.Windows.Forms.Button();
            btnClearPatterns = new System.Windows.Forms.Button();
            ftree = new Ephemera.NBagOfUis.FilTree();
            MenuStrip = new System.Windows.Forms.MenuStrip();
            FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            RecentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ExportCsvMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExportMidiMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            SettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            timeBar = new Ephemera.MidiLib.TimeBar();
            lblChLoc = new System.Windows.Forms.Label();
            toolStrip.SuspendLayout();
            MenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnAutoplay, toolStripSeparator4, btnLoop, toolStripSeparator1, btnLogMidi, toolStripSeparator5, btnKillMidi, toolStripSeparator6, toolStripLabel1, cmbDrumChannel1, toolStripSeparator7, toolStripLabel2, cmbDrumChannel2, toolStripSeparator8, sldVolume, toolStripSeparator11, sldBPM, toolStripSeparator9, btnPlay, toolStripSeparator10, btnRewind, toolStripSeparator12 });
            toolStrip.Location = new System.Drawing.Point(0, 27);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new System.Drawing.Size(959, 43);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "toolStrip";
            // 
            // btnAutoplay
            // 
            btnAutoplay.AutoSize = false;
            btnAutoplay.CheckOnClick = true;
            btnAutoplay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnAutoplay.Image = Properties.Resources.glyphicons_221_play_button;
            btnAutoplay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnAutoplay.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnAutoplay.Name = "btnAutoplay";
            btnAutoplay.Size = new System.Drawing.Size(40, 40);
            btnAutoplay.Text = "toolStripButton1";
            btnAutoplay.ToolTipText = "Autoplay the selection";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 43);
            // 
            // btnLoop
            // 
            btnLoop.AutoSize = false;
            btnLoop.CheckOnClick = true;
            btnLoop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnLoop.Image = Properties.Resources.glyphicons_82_refresh;
            btnLoop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnLoop.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnLoop.Name = "btnLoop";
            btnLoop.Size = new System.Drawing.Size(40, 40);
            btnLoop.Text = "toolStripButton1";
            btnLoop.ToolTipText = "Loop forever";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 43);
            // 
            // btnLogMidi
            // 
            btnLogMidi.AutoSize = false;
            btnLogMidi.CheckOnClick = true;
            btnLogMidi.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnLogMidi.Image = Properties.Resources.glyphicons_170_record;
            btnLogMidi.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnLogMidi.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnLogMidi.Name = "btnLogMidi";
            btnLogMidi.Size = new System.Drawing.Size(40, 40);
            btnLogMidi.Text = "toolStripButton1";
            btnLogMidi.ToolTipText = "Enable logging midi events";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 43);
            // 
            // btnKillMidi
            // 
            btnKillMidi.AutoSize = false;
            btnKillMidi.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnKillMidi.Image = Properties.Resources.glyphicons_242_flash;
            btnKillMidi.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnKillMidi.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnKillMidi.Name = "btnKillMidi";
            btnKillMidi.Size = new System.Drawing.Size(40, 40);
            btnKillMidi.Text = "toolStripButton1";
            btnKillMidi.ToolTipText = "Kill all midi channels";
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(6, 43);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(39, 40);
            toolStripLabel1.Text = "DC1:";
            // 
            // cmbDrumChannel1
            // 
            cmbDrumChannel1.AutoSize = false;
            cmbDrumChannel1.Name = "cmbDrumChannel1";
            cmbDrumChannel1.Size = new System.Drawing.Size(50, 27);
            cmbDrumChannel1.ToolTipText = "Drum Channel - main";
            cmbDrumChannel1.SelectedIndexChanged += DrumChannel_SelectedIndexChanged;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(6, 43);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new System.Drawing.Size(39, 40);
            toolStripLabel2.Text = "DC2:";
            // 
            // cmbDrumChannel2
            // 
            cmbDrumChannel2.AutoSize = false;
            cmbDrumChannel2.Name = "cmbDrumChannel2";
            cmbDrumChannel2.Size = new System.Drawing.Size(50, 27);
            cmbDrumChannel2.ToolTipText = "Drum channel - secondary";
            cmbDrumChannel2.SelectedIndexChanged += DrumChannel_SelectedIndexChanged;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(6, 43);
            // 
            // sldVolume
            // 
            sldVolume.AccessibleName = "sldVolume";
            sldVolume.AutoSize = false;
            sldVolume.BorderStyle = System.Windows.Forms.BorderStyle.None;
            sldVolume.DrawColor = System.Drawing.Color.White;
            sldVolume.Label = "Volume";
            sldVolume.Maximum = 2D;
            sldVolume.Minimum = 0D;
            sldVolume.Name = "sldVolume";
            sldVolume.Resolution = 0.05D;
            sldVolume.Size = new System.Drawing.Size(150, 38);
            sldVolume.Text = "vol";
            sldVolume.ToolTipText = "How loud";
            sldVolume.Value = 1D;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new System.Drawing.Size(6, 43);
            // 
            // sldBPM
            // 
            sldBPM.AccessibleName = "sldBPM";
            sldBPM.AutoSize = false;
            sldBPM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            sldBPM.DrawColor = System.Drawing.Color.White;
            sldBPM.Label = "BPM";
            sldBPM.Maximum = 249D;
            sldBPM.Minimum = 40D;
            sldBPM.Name = "sldBPM";
            sldBPM.Resolution = 1D;
            sldBPM.Size = new System.Drawing.Size(150, 38);
            sldBPM.Text = "bpm";
            sldBPM.ToolTipText = "How fast";
            sldBPM.Value = 100D;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new System.Drawing.Size(6, 43);
            // 
            // btnPlay
            // 
            btnPlay.AutoSize = false;
            btnPlay.CheckOnClick = true;
            btnPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnPlay.Image = Properties.Resources.glyphicons_174_play;
            btnPlay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new System.Drawing.Size(40, 40);
            btnPlay.Text = "toolStripButton1";
            btnPlay.ToolTipText = "Play";
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new System.Drawing.Size(6, 43);
            // 
            // btnRewind
            // 
            btnRewind.AutoSize = false;
            btnRewind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnRewind.Image = Properties.Resources.glyphicons_173_rewind;
            btnRewind.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            btnRewind.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnRewind.Name = "btnRewind";
            btnRewind.Size = new System.Drawing.Size(40, 40);
            btnRewind.Text = "toolStripButton1";
            btnRewind.ToolTipText = "Rewind";
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new System.Drawing.Size(6, 43);
            // 
            // txtViewer
            // 
            txtViewer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtViewer.Location = new System.Drawing.Point(8, 548);
            txtViewer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtViewer.MaxText = 5000;
            txtViewer.Name = "txtViewer";
            txtViewer.Prompt = "";
            txtViewer.Size = new System.Drawing.Size(646, 122);
            txtViewer.TabIndex = 58;
            txtViewer.WordWrap = true;
            // 
            // lbPatterns
            // 
            lbPatterns.BackColor = System.Drawing.SystemColors.Control;
            lbPatterns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lbPatterns.FormattingEnabled = true;
            lbPatterns.Location = new System.Drawing.Point(566, 165);
            lbPatterns.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            lbPatterns.Name = "lbPatterns";
            lbPatterns.Size = new System.Drawing.Size(88, 338);
            lbPatterns.TabIndex = 94;
            toolTip.SetToolTip(lbPatterns, "Select patterns in style file");
            lbPatterns.SelectedIndexChanged += Patterns_SelectedIndexChanged;
            // 
            // btnAllPatterns
            // 
            btnAllPatterns.Location = new System.Drawing.Point(566, 131);
            btnAllPatterns.Name = "btnAllPatterns";
            btnAllPatterns.Size = new System.Drawing.Size(33, 28);
            btnAllPatterns.TabIndex = 95;
            btnAllPatterns.Text = "+";
            toolTip.SetToolTip(btnAllPatterns, "All patterns");
            btnAllPatterns.UseVisualStyleBackColor = true;
            btnAllPatterns.Click += AllOrNone_Click;
            // 
            // btnClearPatterns
            // 
            btnClearPatterns.Location = new System.Drawing.Point(622, 131);
            btnClearPatterns.Name = "btnClearPatterns";
            btnClearPatterns.Size = new System.Drawing.Size(33, 28);
            btnClearPatterns.TabIndex = 96;
            btnClearPatterns.Text = "-";
            toolTip.SetToolTip(btnClearPatterns, "Clear patterns");
            btnClearPatterns.UseVisualStyleBackColor = true;
            btnClearPatterns.Click += AllOrNone_Click;
            // 
            // ftree
            // 
            ftree.Location = new System.Drawing.Point(8, 76);
            ftree.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            ftree.Name = "ftree";
            ftree.Size = new System.Drawing.Size(527, 464);
            ftree.TabIndex = 89;
            toolTip.SetToolTip(ftree, "File selection");
            ftree.FileSelected += Navigator_FileSelected;
            // 
            // MenuStrip
            // 
            MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileMenuItem, ToolsMenuItem });
            MenuStrip.Location = new System.Drawing.Point(0, 0);
            MenuStrip.Name = "MenuStrip";
            MenuStrip.Size = new System.Drawing.Size(959, 27);
            MenuStrip.TabIndex = 90;
            MenuStrip.Text = "menuStrip";
            // 
            // FileMenuItem
            // 
            FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { OpenMenuItem, toolStripSeparator3, RecentMenuItem, toolStripSeparator2, ExportCsvMenuItem, ExportMidiMenuItem });
            FileMenuItem.Name = "FileMenuItem";
            FileMenuItem.Size = new System.Drawing.Size(43, 23);
            FileMenuItem.Text = "File";
            FileMenuItem.DropDownOpening += File_DropDownOpening;
            // 
            // OpenMenuItem
            // 
            OpenMenuItem.Name = "OpenMenuItem";
            OpenMenuItem.Size = new System.Drawing.Size(156, 24);
            OpenMenuItem.Text = "Open";
            OpenMenuItem.Click += Open_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(153, 6);
            // 
            // RecentMenuItem
            // 
            RecentMenuItem.Name = "RecentMenuItem";
            RecentMenuItem.Size = new System.Drawing.Size(156, 24);
            RecentMenuItem.Text = "Recent";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(153, 6);
            // 
            // ExportCsvMenuItem
            // 
            ExportCsvMenuItem.Name = "ExportCsvMenuItem";
            ExportCsvMenuItem.Size = new System.Drawing.Size(156, 24);
            ExportCsvMenuItem.Text = "Export CSV";
            ExportCsvMenuItem.Click += Export_Click;
            // 
            // ExportMidiMenuItem
            // 
            ExportMidiMenuItem.Name = "ExportMidiMenuItem";
            ExportMidiMenuItem.Size = new System.Drawing.Size(156, 24);
            ExportMidiMenuItem.Text = "Export Midi";
            ExportMidiMenuItem.Click += Export_Click;
            // 
            // ToolsMenuItem
            // 
            ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SettingsMenuItem, AboutMenuItem });
            ToolsMenuItem.Name = "ToolsMenuItem";
            ToolsMenuItem.Size = new System.Drawing.Size(54, 23);
            ToolsMenuItem.Text = "Tools";
            // 
            // SettingsMenuItem
            // 
            SettingsMenuItem.Name = "SettingsMenuItem";
            SettingsMenuItem.Size = new System.Drawing.Size(135, 24);
            SettingsMenuItem.Text = "Settings";
            SettingsMenuItem.Click += Settings_Click;
            // 
            // AboutMenuItem
            // 
            AboutMenuItem.Name = "AboutMenuItem";
            AboutMenuItem.Size = new System.Drawing.Size(135, 24);
            AboutMenuItem.Text = "About";
            AboutMenuItem.Click += About_Click;
            // 
            // timeBar
            // 
            timeBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            timeBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            timeBar.DoLoop = false;
            timeBar.DrawColor = System.Drawing.Color.Black;
            timeBar.FontLarge = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            timeBar.FontSmall = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            timeBar.Location = new System.Drawing.Point(566, 76);
            timeBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            timeBar.Name = "timeBar";
            timeBar.SelectedColor = System.Drawing.Color.White;
            timeBar.Size = new System.Drawing.Size(381, 47);
            timeBar.Snap = Ephemera.MidiLib.SnapType.Beat;
            timeBar.TabIndex = 93;
            // 
            // lblChLoc
            // 
            lblChLoc.AutoSize = true;
            lblChLoc.Location = new System.Drawing.Point(666, 131);
            lblChLoc.Name = "lblChLoc";
            lblChLoc.Size = new System.Drawing.Size(45, 19);
            lblChLoc.TabIndex = 97;
            lblChLoc.Text = "label1";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(959, 674);
            Controls.Add(lblChLoc);
            Controls.Add(lbPatterns);
            Controls.Add(btnAllPatterns);
            Controls.Add(btnClearPatterns);
            Controls.Add(timeBar);
            Controls.Add(ftree);
            Controls.Add(txtViewer);
            Controls.Add(toolStrip);
            Controls.Add(MenuStrip);
            MainMenuStrip = MenuStrip;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "MainForm";
            Text = "Midifrier";
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            MenuStrip.ResumeLayout(false);
            MenuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
        #endregion

        // new
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolTip toolTip;
        private Ephemera.NBagOfUis.FilTree ftree;
        private Ephemera.NBagOfUis.TextViewer txtViewer;
        private Ephemera.MidiLib.TimeBar timeBar;
        private System.Windows.Forms.CheckedListBox lbPatterns;
        private System.Windows.Forms.Button btnAllPatterns;
        private System.Windows.Forms.Button btnClearPatterns;
        private System.Windows.Forms.ToolStripButton btnAutoplay;
        private System.Windows.Forms.ToolStripButton btnLoop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ExportCsvMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportMidiMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem RecentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SettingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmbDrumChannel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox cmbDrumChannel2;
        private System.Windows.Forms.ToolStripButton btnLogMidi;
        private System.Windows.Forms.ToolStripButton btnKillMidi;
        private System.Windows.Forms.Label lblChLoc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private Ephemera.NBagOfUis.ToolStripSlider sldVolume;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private Ephemera.NBagOfUis.ToolStripSlider sldBPM;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton btnPlay;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton btnRewind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
    }
}