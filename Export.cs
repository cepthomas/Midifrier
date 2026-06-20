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

        /// <summary>
        /// Export the contents in a human readable form.
        /// </summary>
        /// <param name="fn">Where to boss?</param>
        /// <param name="pattern">Specific pattern.</param>
        /// <param name="channels">Specific channnels or all if empty.</param>
        /// <param name="header">File header data to include.</param>
        public static void ExportText(string fn, Pattern pattern, List<int> channels, Header header)
        {
            // Collect output text.
            string indent = "    ";

            List<string> contentText =
            [
                $"MThd",
                $"{indent}MidiFileType {header.MidiFileType}",
                $"{indent}NumTracks {header.NumTracks}",
                $"{indent}DeltaTicksPerQuarterNote {header.DeltaTicksPerQuarterNote}"
            ];

            // Midi tracks and events.
            for (int i = 0; i < pattern.Tracks.Count; i++)
            {
                var track = pattern.Tracks[i];

                // Don't include only-meta tracks except first.
                //if (track.NumStandard == 0 && i != 0) { continue; }

                contentText.Add($"MTrk");

                foreach (var mevt in track.GetFilteredEvents(channels))
                {
                    contentText.Add($"{indent}{mevt}");
                }
            }

            File.WriteAllLines(fn, contentText);
        }

        /// <summary>
        /// Export the contents as csv.
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
        /// Dump a simple or style file as semi-structured verbatim contents.
        /// </summary>
        /// <param name="fn">The file to process.</param>
        /// <param name="noisy">Include events like controller changes, pitch wheel, ....</param>
        /// <param name="sysex">Include sysex.</param>
        /// <param name="clip">Limit number of ordinary events in output. 0 means no limit.</param>
        public static List<string> Dump(string fn, bool noisy = false, bool sysex = false, int clip = 20)
        {
            using var br = new BinaryReader(File.OpenRead(fn));
            bool done = false;
            const string IND = "    ";
            List<string> content = [];

            while (!done)
            {
                // Read next section.
                var bytes = br.ReadBytes(4);
                if (bytes.Length != 4) break;

                var sectionName = Encoding.UTF8.GetString(bytes);

                switch (sectionName)
                {
                    case "MThd":
                        {
                            // Read the midi header section.
                            uint chunkSize = MiscUtils.FixEndian(br.ReadUInt32());
                            if (chunkSize != 6) { throw new InvalidOperationException("Unexpected header chunk length"); }
                            var midiFileType = MiscUtils.FixEndian(br.ReadUInt16());
                            var numTracks = MiscUtils.FixEndian(br.ReadUInt16());
                            var deltaTicksPerQuarterNote = MiscUtils.FixEndian(br.ReadUInt16());

                            content.AddRange(
                            [
                                $"MThd",
                                $"{IND}MidiFileType {midiFileType}",
                                $"{IND}NumTracks {numTracks}",
                                $"{IND}DeltaTicksPerQuarterNote {deltaTicksPerQuarterNote}"
                            ]);
                        }
                        break;

                    case "MTrk":
                        {
                            // Read the track section.
                            content.Add($"MTrk");
                            uint chunkSize = MiscUtils.FixEndian(br.ReadUInt32());
                            long startPos = br.BaseStream.Position;
                            int absoluteTime = 0;

                            // Read all midi events.
                            bool donemtrk = false;
                            MidiEvent? me = null; // current event
                            int thrcnt = 0;

                            while (!donemtrk && br.BaseStream.Position < startPos + chunkSize)
                            {
                                me = MidiEvent.ReadNextEvent(br, me);
                                absoluteTime += me.DeltaTime;
                                me.AbsoluteTime = absoluteTime;

                                switch (me)
                                {
                                    case NoteOnEvent:
                                    case NoteEvent: // aka NoteOff
                                    case ControlChangeEvent when noisy:
                                    case PitchWheelChangeEvent when noisy:
                                        thrcnt++;

                                        if (clip > 0)
                                        {
                                            if (thrcnt == clip) { content.Add($"{IND}..."); }
                                            else if (thrcnt < clip) { content.Add($"{IND}{me}"); }
                                            else { } // don't add
                                        }
                                        else
                                        {
                                            content.Add($"{IND}{me}");
                                        }
                                        break;

                                    case TempoEvent:
                                    case TimeSignatureEvent:
                                    case KeySignatureEvent:
                                    case PatchChangeEvent:
                                        content.Add($"{IND}{me}");
                                        //thrcnt = 0; // reset
                                        break;

                                    case SysexEvent when sysex:
                                        var s = me.ToString().Replace("\r\n", " ");
                                        content.Add($"{IND}{s}");
                                        break;

                                    case TextEvent:
                                        //case TextEvent evt when evt.MetaEventType == MetaEventType.SequenceTrackName:
                                        //case TextEvent evt when evt.MetaEventType == MetaEventType.Marker:
                                        // See other functions for higher level.
                                        content.Add($"{IND}{me}");
                                        thrcnt = 0; // reset
                                        break;

                                    case MetaEvent evt when evt.MetaEventType == MetaEventType.EndTrack:
                                        // See other functions for higher level.
                                        content.Add($"{IND}{me}");
                                        thrcnt = 0; // reset
                                        donemtrk = true;
                                        break;

                                    case ControlChangeEvent:
                                    case PitchWheelChangeEvent:
                                    case SysexEvent:
                                        // Ignore filtered others.
                                        break;

                                    case SequencerSpecificEvent:
                                    case RawMetaEvent:
                                        content.Add($"{IND}{me}");
                                        break;

                                    default:
                                        // Leftovers to deal with.
                                        content.Add($"{IND}{me.GetType()} {me} <<<<<<<<<<<<<<<<<<<<");
                                        break;
                                }
                            }
                        }
                        break;

                    // Style detail sections.
                    case "CASM":
                    case "CSEG":
                    case "Sdec":
                    case "Ctab":
                    case "Cntt":
                    case "OTSc":
                    case "FNRc":
                        {
                            uint chunkSize = MiscUtils.FixEndian(br.ReadUInt32());
                            content.Add($"{sectionName} {chunkSize}");
                            br.ReadBytes((int)chunkSize); // skip
                        }
                        break;

                    default:
                        {
                            // Sometimes there's other stuff at the end of the file - report and ignore.
                            long sz = br.BaseStream.Length - br.BaseStream.Position;
                            content.Add($"Remainder {sz}");
                            done = true;
                        }
                        break;
                }
            }

            return content;
        }
    }
}
