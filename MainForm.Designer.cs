namespace Ephemera.Midifrier
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnAutoplay = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLoop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLogMidi = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnKillMidi = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbDrumChannel1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.cmbDrumChannel2 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.sldVolume = new NBagOfUis.ToolStripSlider();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.sldBPM = new NBagOfUis.ToolStripSlider();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRewind = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.txtViewer = new NBagOfUis.TextViewer();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lbPatterns = new System.Windows.Forms.CheckedListBox();
            this.btnAllPatterns = new System.Windows.Forms.Button();
            this.btnClearPatterns = new System.Windows.Forms.Button();
            this.ftree = new NBagOfUis.FilTree();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.RecentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportCsvMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportMidiMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barBar = new MidiLib.BarBar();
            this.lblChLoc = new System.Windows.Forms.Label();
            this.toolStrip.SuspendLayout();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAutoplay,
            this.toolStripSeparator4,
            this.btnLoop,
            this.toolStripSeparator1,
            this.btnLogMidi,
            this.toolStripSeparator5,
            this.btnKillMidi,
            this.toolStripSeparator6,
            this.toolStripLabel1,
            this.cmbDrumChannel1,
            this.toolStripSeparator7,
            this.toolStripLabel2,
            this.cmbDrumChannel2,
            this.toolStripSeparator8,
            this.sldVolume,
            this.toolStripSeparator11,
            this.sldBPM,
            this.toolStripSeparator9,
            this.btnPlay,
            this.toolStripSeparator10,
            this.btnRewind,
            this.toolStripSeparator12});
            this.toolStrip.Location = new System.Drawing.Point(0, 28);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1110, 43);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // btnAutoplay
            // 
            this.btnAutoplay.AutoSize = false;
            this.btnAutoplay.CheckOnClick = true;
            this.btnAutoplay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAutoplay.Image = global::Ephemera.Midifrier.Properties.Resources.glyphicons_221_play_button;
            this.btnAutoplay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAutoplay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAutoplay.Name = "btnAutoplay";
            this.btnAutoplay.Size = new System.Drawing.Size(40, 40);
            this.btnAutoplay.Text = "toolStripButton1";
            this.btnAutoplay.ToolTipText = "Autoplay the selection";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 43);
            // 
            // btnLoop
            // 
            this.btnLoop.AutoSize = false;
            this.btnLoop.CheckOnClick = true;
            this.btnLoop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLoop.Image = global::Ephemera.Midifrier.Properties.Resources.glyphicons_82_refresh;
            this.btnLoop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLoop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoop.Name = "btnLoop";
            this.btnLoop.Size = new System.Drawing.Size(40, 40);
            this.btnLoop.Text = "toolStripButton1";
            this.btnLoop.ToolTipText = "Loop forever";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 43);
            // 
            // btnLogMidi
            // 
            this.btnLogMidi.AutoSize = false;
            this.btnLogMidi.CheckOnClick = true;
            this.btnLogMidi.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLogMidi.Image = global::Ephemera.Midifrier.Properties.Resources.glyphicons_170_record;
            this.btnLogMidi.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLogMidi.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogMidi.Name = "btnLogMidi";
            this.btnLogMidi.Size = new System.Drawing.Size(40, 40);
            this.btnLogMidi.Text = "toolStripButton1";
            this.btnLogMidi.ToolTipText = "Enable logging midi events";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 43);
            // 
            // btnKillMidi
            // 
            this.btnKillMidi.AutoSize = false;
            this.btnKillMidi.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnKillMidi.Image = global::Ephemera.Midifrier.Properties.Resources.glyphicons_242_flash;
            this.btnKillMidi.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnKillMidi.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnKillMidi.Name = "btnKillMidi";
            this.btnKillMidi.Size = new System.Drawing.Size(40, 40);
            this.btnKillMidi.Text = "toolStripButton1";
            this.btnKillMidi.ToolTipText = "Kill all midi channels";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 43);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(40, 40);
            this.toolStripLabel1.Text = "DC1:";
            // 
            // cmbDrumChannel1
            // 
            this.cmbDrumChannel1.AutoSize = false;
            this.cmbDrumChannel1.Name = "cmbDrumChannel1";
            this.cmbDrumChannel1.Size = new System.Drawing.Size(50, 28);
            this.cmbDrumChannel1.ToolTipText = "Drum Channel - main";
            this.cmbDrumChannel1.SelectedIndexChanged += new System.EventHandler(this.DrumChannel_SelectedIndexChanged);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 43);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(40, 40);
            this.toolStripLabel2.Text = "DC2:";
            // 
            // cmbDrumChannel2
            // 
            this.cmbDrumChannel2.AutoSize = false;
            this.cmbDrumChannel2.Name = "cmbDrumChannel2";
            this.cmbDrumChannel2.Size = new System.Drawing.Size(50, 28);
            this.cmbDrumChannel2.ToolTipText = "Drum channel - secondary";
            this.cmbDrumChannel2.SelectedIndexChanged += new System.EventHandler(this.DrumChannel_SelectedIndexChanged);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 43);
            // 
            // sldVolume
            // 
            this.sldVolume.AccessibleName = "sldVolume";
            this.sldVolume.AutoSize = false;
            this.sldVolume.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sldVolume.DrawColor = System.Drawing.Color.White;
            this.sldVolume.Label = "Volume";
            this.sldVolume.Maximum = 2D;
            this.sldVolume.Minimum = 0D;
            this.sldVolume.Name = "sldVolume";
            this.sldVolume.Resolution = 0.05D;
            this.sldVolume.Size = new System.Drawing.Size(150, 40);
            this.sldVolume.Text = "vol";
            this.sldVolume.ToolTipText = "How loud";
            this.sldVolume.Value = 1D;
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 43);
            // 
            // sldBPM
            // 
            this.sldBPM.AccessibleName = "sldBPM";
            this.sldBPM.AutoSize = false;
            this.sldBPM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sldBPM.DrawColor = System.Drawing.Color.White;
            this.sldBPM.Label = "BPM";
            this.sldBPM.Maximum = 249D;
            this.sldBPM.Minimum = 40D;
            this.sldBPM.Name = "sldBPM";
            this.sldBPM.Resolution = 1D;
            this.sldBPM.Size = new System.Drawing.Size(150, 40);
            this.sldBPM.Text = "bpm";
            this.sldBPM.ToolTipText = "How fast";
            this.sldBPM.Value = 100D;
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 43);
            // 
            // btnPlay
            // 
            this.btnPlay.AutoSize = false;
            this.btnPlay.CheckOnClick = true;
            this.btnPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPlay.Image = global::Ephemera.Midifrier.Properties.Resources.glyphicons_174_play;
            this.btnPlay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(40, 40);
            this.btnPlay.Text = "toolStripButton1";
            this.btnPlay.ToolTipText = "Play";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 43);
            // 
            // btnRewind
            // 
            this.btnRewind.AutoSize = false;
            this.btnRewind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRewind.Image = global::Ephemera.Midifrier.Properties.Resources.glyphicons_173_rewind;
            this.btnRewind.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRewind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRewind.Name = "btnRewind";
            this.btnRewind.Size = new System.Drawing.Size(40, 40);
            this.btnRewind.Text = "toolStripButton1";
            this.btnRewind.ToolTipText = "Rewind";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 43);
            // 
            // txtViewer
            // 
            this.txtViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtViewer.Location = new System.Drawing.Point(8, 577);
            this.txtViewer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtViewer.MaxText = 5000;
            this.txtViewer.Name = "txtViewer";
            this.txtViewer.Prompt = "";
            this.txtViewer.Size = new System.Drawing.Size(646, 128);
            this.txtViewer.TabIndex = 58;
            this.txtViewer.WordWrap = true;
            // 
            // lbPatterns
            // 
            this.lbPatterns.BackColor = System.Drawing.SystemColors.Control;
            this.lbPatterns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPatterns.FormattingEnabled = true;
            this.lbPatterns.Location = new System.Drawing.Point(566, 174);
            this.lbPatterns.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lbPatterns.Name = "lbPatterns";
            this.lbPatterns.Size = new System.Drawing.Size(88, 376);
            this.lbPatterns.TabIndex = 94;
            this.toolTip.SetToolTip(this.lbPatterns, "Select patterns in style file");
            this.lbPatterns.SelectedIndexChanged += new System.EventHandler(this.Patterns_SelectedIndexChanged);
            // 
            // btnAllPatterns
            // 
            this.btnAllPatterns.Location = new System.Drawing.Point(566, 138);
            this.btnAllPatterns.Name = "btnAllPatterns";
            this.btnAllPatterns.Size = new System.Drawing.Size(33, 29);
            this.btnAllPatterns.TabIndex = 95;
            this.btnAllPatterns.Text = "+";
            this.toolTip.SetToolTip(this.btnAllPatterns, "All patterns");
            this.btnAllPatterns.UseVisualStyleBackColor = true;
            this.btnAllPatterns.Click += new System.EventHandler(this.AllOrNone_Click);
            // 
            // btnClearPatterns
            // 
            this.btnClearPatterns.Location = new System.Drawing.Point(622, 138);
            this.btnClearPatterns.Name = "btnClearPatterns";
            this.btnClearPatterns.Size = new System.Drawing.Size(33, 29);
            this.btnClearPatterns.TabIndex = 96;
            this.btnClearPatterns.Text = "-";
            this.toolTip.SetToolTip(this.btnClearPatterns, "Clear patterns");
            this.btnClearPatterns.UseVisualStyleBackColor = true;
            this.btnClearPatterns.Click += new System.EventHandler(this.AllOrNone_Click);
            // 
            // ftree
            // 
            this.ftree.Location = new System.Drawing.Point(8, 80);
            this.ftree.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.ftree.Name = "ftree";
            this.ftree.Size = new System.Drawing.Size(527, 488);
            this.ftree.TabIndex = 89;
            this.toolTip.SetToolTip(this.ftree, "File selection");
            this.ftree.FileSelected += new System.EventHandler<string>(this.Navigator_FileSelected);
            // 
            // menuStrip
            // 
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ToolsMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "menuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1110, 28);
            this.MenuStrip.TabIndex = 90;
            this.MenuStrip.Text = "menuStrip";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenuItem,
            this.toolStripSeparator3,
            this.RecentMenuItem,
            this.toolStripSeparator2,
            this.ExportCsvMenuItem,
            this.ExportMidiMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(46, 24);
            this.FileMenuItem.Text = "File";
            this.FileMenuItem.DropDownOpening += new System.EventHandler(this.File_DropDownOpening);
            // 
            // OpenMenuItem
            // 
            this.OpenMenuItem.Name = "OpenMenuItem";
            this.OpenMenuItem.Size = new System.Drawing.Size(169, 26);
            this.OpenMenuItem.Text = "Open";
            this.OpenMenuItem.Click += new System.EventHandler(this.Open_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(166, 6);
            // 
            // RecentMenuItem
            // 
            this.RecentMenuItem.Name = "RecentMenuItem";
            this.RecentMenuItem.Size = new System.Drawing.Size(169, 26);
            this.RecentMenuItem.Text = "Recent";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
            // 
            // ExportCsvMenuItem
            // 
            this.ExportCsvMenuItem.Name = "ExportCsvMenuItem";
            this.ExportCsvMenuItem.Size = new System.Drawing.Size(169, 26);
            this.ExportCsvMenuItem.Text = "Export CSV";
            this.ExportCsvMenuItem.Click += new System.EventHandler(this.Export_Click);
            // 
            // ExportMidiMenuItem
            // 
            this.ExportMidiMenuItem.Name = "ExportMidiMenuItem";
            this.ExportMidiMenuItem.Size = new System.Drawing.Size(169, 26);
            this.ExportMidiMenuItem.Text = "Export Midi";
            this.ExportMidiMenuItem.Click += new System.EventHandler(this.Export_Click);
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingsMenuItem,
            this.AboutMenuItem});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            this.ToolsMenuItem.Size = new System.Drawing.Size(58, 24);
            this.ToolsMenuItem.Text = "Tools";
            // 
            // SettingsMenuItem
            // 
            this.SettingsMenuItem.Name = "SettingsMenuItem";
            this.SettingsMenuItem.Size = new System.Drawing.Size(145, 26);
            this.SettingsMenuItem.Text = "Settings";
            this.SettingsMenuItem.Click += new System.EventHandler(this.Settings_Click);
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(145, 26);
            this.AboutMenuItem.Text = "About";
            this.AboutMenuItem.Click += new System.EventHandler(this.About_Click);
            // 
            // barBar
            // 
            this.barBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.barBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.barBar.FontLarge = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.barBar.FontSmall = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.barBar.Location = new System.Drawing.Point(566, 80);
            this.barBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barBar.MarkerColor = System.Drawing.Color.Black;
            this.barBar.Name = "barBar";
            this.barBar.ProgressColor = System.Drawing.Color.White;
            this.barBar.Size = new System.Drawing.Size(532, 49);
            this.barBar.TabIndex = 93;
            // 
            // lblChLoc
            // 
            this.lblChLoc.AutoSize = true;
            this.lblChLoc.Location = new System.Drawing.Point(666, 138);
            this.lblChLoc.Name = "lblChLoc";
            this.lblChLoc.Size = new System.Drawing.Size(50, 20);
            this.lblChLoc.TabIndex = 97;
            this.lblChLoc.Text = "label1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 709);
            this.Controls.Add(this.lblChLoc);
            this.Controls.Add(this.lbPatterns);
            this.Controls.Add(this.btnAllPatterns);
            this.Controls.Add(this.btnClearPatterns);
            this.Controls.Add(this.barBar);
            this.Controls.Add(this.ftree);
            this.Controls.Add(this.txtViewer);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.MenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "Midifrier";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        // new
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolTip toolTip;
        private NBagOfUis.FilTree ftree;
        private NBagOfUis.TextViewer txtViewer;
        private MidiLib.BarBar barBar;
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
        private NBagOfUis.ToolStripSlider sldVolume;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private NBagOfUis.ToolStripSlider sldBPM;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton btnPlay;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton btnRewind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
    }
}