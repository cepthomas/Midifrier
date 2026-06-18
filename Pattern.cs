using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using NAudio.Midi;
using Ephemera.NBagOfTricks;
using Ephemera.MidiLib;


namespace Midifrier
{
    /// <summary>
    /// The contents of a midi file pattern.
    /// </summary>
    public class Pattern()
    {
        #region Properties
        /// <summary>Pattern name. Empty indicates single pattern aka plain midi file.</summary>
        public string Name { get; set; } = MidiDataFile.UNNAMED;

        /// <summary>Pattern default tempo.</summary>
        public int Tempo { get; set; } = MidiDataFile.DEFAULT_TEMPO;

        /// <summary>All the tracks in the pattern.</summary>
        public List<Track> Tracks { get; set; } = [];
        #endregion

        /// <summary>
        /// Readable version.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} Tracks: {Tracks.Count}";
        }
    }
}