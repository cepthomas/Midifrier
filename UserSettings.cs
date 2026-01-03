using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ephemera.NBagOfTricks;
using Ephemera.NBagOfUis;
using Ephemera.MidiLib;


namespace Midifrier
{
    [Serializable]
    public sealed class UserSettings : SettingsCore
    {
        #region Persisted Editable Properties
        [DisplayName("Output Device")]
        [Description("Valid output device.")]
        [Browsable(true)]
        [Editor(typeof(GenericListTypeEditor), typeof(UITypeEditor))]
        public string OutputDevice { get; set; } = "???";

        [DisplayName("Tempo Resolution")]
        [Description("Adjust tempo in UI.")]
        [Browsable(true)]
        public int TempoResolution { get; set; } = 5;

        [DisplayName("Draw Color")]
        [Description("The color used for active control surfaces.")]
        [Browsable(true)]
        [JsonConverter(typeof(JsonColorConverter))]
        public Color DrawColor { get; set; } = Color.Red;

        [DisplayName("Selected Color")]
        [Description("The color used for control selections.")]
        [Browsable(true)]
        [JsonConverter(typeof(JsonColorConverter))]
        public Color SelectedColor { get; set; } = Color.Blue;

        [DisplayName("File Log Level")]
        [Description("Log level for file write.")]
        [Browsable(true)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogLevel FileLogLevel { get; set; } = LogLevel.Trace;

        [DisplayName("File Log Level")]
        [Description("Log level for UI notification.")]
        [Browsable(true)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogLevel NotifLogLevel { get; set; } = LogLevel.Debug;

        [DisplayName("Root Paths")]
        [Description("Your favorite places.")]
        [Browsable(true)]
        [Editor(typeof(StringListEditor), typeof(UITypeEditor))]
        public List<string> RootDirs { get; set; } = [];

        [DisplayName("Ignore Paths")]
        [Description("Ignore these noisy directories.")]
        [Browsable(true)]
        [Editor(typeof(StringListEditor), typeof(UITypeEditor))]
        public List<string> IgnoreDirs { get; set; } = [];

        [DisplayName("Single Click Select")]
        [Description("Generate event with single or double click.")]
        [Browsable(true)]
        public bool SingleClickSelect { get; set; } = false;

        [DisplayName("Default Export Folder")]
        [Description("Where to put exported files.")]
        [Browsable(true)]
        public string ExportFolder { get; set; } = "";
        #endregion

        #region Persisted Non-editable Persisted Properties
        [Browsable(false)]
        public bool Autoplay { get; set; } = true;

        [Browsable(false)]
        public bool Loop { get; set; } = false;

        [Browsable(false)]
        public bool LogMidi { get; set; } = false;

        [Browsable(false)]
        public double Volume { get; set; } = 0.5;

        [Browsable(false)]
        public int SplitterPosition { get; set; } = 30;
        #endregion
    }
}
