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
using static Midifrier.MidiDataFile;


namespace Midifrier
{
    /// <summary>
    /// Writes to various output formats.
    /// </summary>
    public class MidiExport
    {
        /// <summary>
        /// Export the contents in a csv readable form. This is as the events appear in the original file.
        /// </summary>
        /// <param name="fn">Where to boss?</param>
        /// <param name="pattern">Specific pattern.</param>
        /// <param name="channels">Specific channnels or all if empty.</param>
        /// <param name="header">File header data to include.</param>
        public static void ExportCsv(string fn, Pattern pattern, List<int> channels, Header header)
        {
            // Collect output text.
            List<string> contentText =
            [
                $"AbsoluteTime,DeltaTime,Event,Channel,Content1,Content2",
                $"0,0,Header,0,MidiFileType,{header.MidiFileType}",
                $"0,0,Header,0,NumTracks,{header.NumTracks}",
                $"0,0,Header,0,DeltaTicksPerQuarterNote,{header.DeltaTicksPerQuarterNote}"
            ];

            // Midi events.
            for (int i = 0; i < pattern.Tracks.Count; i++)
            {
                var track = pattern.Tracks[i];

                // Don't include only-meta tracks except first.
                if (track.NumStandard == 0 && i != 0) { continue; }

                foreach (var mevt in track.GetFilteredEvents(channels))
                {
                    // Boilerplate.
                    var isMeta = mevt.CommandCode == MidiCommandCode.MetaEvent;
                    List<object> parts =
                    [
                        mevt.AbsoluteTime,
                        mevt.DeltaTime,
                        isMeta ? (mevt as MetaEvent)!.MetaEventType : mevt.CommandCode,
                        isMeta ? 0 : mevt.Channel
                    ];

                    switch (mevt)
                    {
                        case NoteOnEvent evt: parts.AddRange([evt.NoteNumber, evt.Velocity]); break;
                        case NoteEvent evt: parts.AddRange([evt.NoteNumber, ""]); break;
                        case TempoEvent evt: parts.AddRange([evt.Tempo, evt.MicrosecondsPerQuarterNote]); break;
                        case TimeSignatureEvent evt: parts.AddRange([evt.TimeSignature, ""]); break;
                        case KeySignatureEvent evt: parts.AddRange([evt.SharpsFlats, evt.MajorMinor]); break;
                        case PatchChangeEvent evt: parts.AddRange([evt.Patch, MidiDefs.Instruments.GetName(evt.Patch)]); break; // TODO doesn't handle alternate banks
                        case ControlChangeEvent evt: parts.AddRange([evt.Controller, MidiDefs.Controllers.GetName((int)evt.Controller)]); break;
                        case PitchWheelChangeEvent evt: /*parts.AddRange([evt.Pitch, ""]);*/ break;
                        case TextEvent evt: parts.AddRange([evt.Text, ""]); break;
                        // Others as needed: TrackSequenceNumberEvent, ChannelAfterTouchEvent, SysexEvent,
                        // MetaEvent, RawMetaEvent, SequencerSpecificEvent, SmpteOffsetEvent
                        default: parts.AddRange(["", ""]); break;
                    }
                    var sparts = string.Join(",", parts);
                    contentText.Add(sparts);
                }
            }

            File.WriteAllLines(fn, contentText);
        }

        /// <summary>
        /// Export pattern to midi file type 1. This is as the events appear in the original file.
        /// </summary>
        /// <param name="fn">Where to boss?</param>
        /// <param name="pattern">Specific pattern.</param>
        /// <param name="channels">Specific channnels or all if empty.</param>
        /// <param name="header">File header data to include.</param>
        public static void ExportMidi(string fn, Pattern pattern, List<int> channels, Header header)
        {
            // Init output file contents.
            int ppq = header.DeltaTicksPerQuarterNote;
            MidiEventCollection outColl = new(1, ppq);

            for (int i = 0; i < pattern.Tracks.Count; i++)
            {
                var track = pattern.Tracks[i];

                // Don't include only-meta tracks except first.
                if (track.NumStandard == 0 && i != 0) { continue; }

                // Build the event collection.
                IList<MidiEvent> outEvents = outColl.AddTrack();
                var events = track.GetFilteredEvents(channels);
                events?.ForEach(e => { outEvents.Add(e); });
            }

            // Use NAudio function to create out file.
            MidiFile.Export(fn, outColl);
        }
    }
}
