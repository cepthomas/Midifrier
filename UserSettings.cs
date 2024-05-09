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
using NAudio.Wave;
using NAudio.Midi;
using Ephemera.NBagOfTricks;
using Ephemera.NBagOfUis;
using Ephemera.NBagOfTricks.Slog;
using Ephemera.MidiLib;


namespace Midifrier
{
    [Serializable]
    public sealed class UserSettings : SettingsCore
    {
        #region Persisted Editable Properties
        [DisplayName("Control Color")]
        [Description("Pick what you like.")]
        [Browsable(true)]
        [JsonConverter(typeof(JsonColorConverter))]
        public Color ControlColor { get; set; } = Color.MediumOrchid;

        [DisplayName("Tempo Resolution")]
        [Description("Adjust tempo in UI.")]
        [Browsable(true)]
        public int TempoResolution { get; set; } = 5;

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
        public List<string> RootDirs { get; set; } = new();

        [DisplayName("Ignore Paths")]
        [Description("Ignore these noisy directories.")]
        [Browsable(true)]
        [Editor(typeof(StringListEditor), typeof(UITypeEditor))]
        public List<string> IgnoreDirs { get; set; } = new();

        [DisplayName("Single Click Select")]
        [Description("Generate event with single or double click.")]
        [Browsable(true)]
        public bool SingleClickSelect { get; set; } = false;

        [DisplayName("Midi Settings")]
        [Description("Edit midi settings.")]
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MidiSettings MidiSettings { get; set; } = new();
        #endregion

        #region Persisted Non-editable Persisted Properties
        [Browsable(false)]
        public bool Autoplay { get; set; } = true;

        [Browsable(false)]
        public bool Loop { get; set; } = false;

        [Browsable(false)]
        public double Volume { get; set; } = 0.5;

        [Browsable(false)]
        public int SplitterPosition { get; set; } = 30;
        #endregion
    }
}
