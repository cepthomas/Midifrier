using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ephemera.NBagOfTricks;
using Ephemera.NBagOfTricks.PNUT;
using Midifrier;

// Some test code, ignore.
// Useful files - from https://github.com/cepthomas/TestAudioFiles:
// Style file, full info: _LoveSong.S474.sty
// Plain midi, full song: WICKGAME.MID (has other stuff after the last track)
// Plain midi, one instrument, no patch: bass_ch2.mid
// Plain midi, drums on different channel: _drums_ch1.mid


namespace Midifrier.Test
{
    //----------------------------------------------------------------
    public static class Executor
    {
        // This needs DEV_PATH set, or hack to taste.
        public static string InputDir = Path.Join(Environment.GetEnvironmentVariable("DEV_PATH"), "Misc", "TestAudioFiles");
        public static string OutputDir = Path.Join(MiscUtils.GetSourcePath(), "out");

        /// <summary>Test entry.</summary>
        public static void Run()
        {
            // Ensure paths.
            Directory.CreateDirectory(OutputDir);

            TestRunner runner = new(OutputFormat.Readable);
            //  MLEX_SIMPLE  MLEX_STYLE  MLEX_DUMP  MLEX
            var torun = new[] { "MLEX_DUMP" };
            runner.RunSuites(torun);

            // File.WriteAllLines(Path.Join(MiscUtils.GetSourcePath(), "test.txt"), runner.Context.OutputLines);
        }
    }

    //----------------------------------------------------------------
    /// <summary>Test export functions.</summary>
    public class MLEX_SIMPLE : TestSuite
    {
        public override void RunSuite()
        {
            StopOnFail(true);

            MidiDataFile mfd = new();
            mfd.Read(Path.Join(Executor.InputDir, "WICKGAME.MID"));

            //var numtr = mfd!.NumTracks; // 10
            var pnames = mfd.GetPatternNames();
            Assert(pnames.Count == 1);

            // Execute the export functions.
            var exThrown = ThrowsNot(() =>
            {
               var pattern = mfd.GetPattern(MidiDataFile.UNNAMED);

               var hdr = mfd.Header;

               var fn1 = Path.Join(Executor.OutputDir, "simple_midi_all");
               List<int> chs1 = [];
               MidiExport.ExportCsv($"{fn1}.csv", pattern, chs1, hdr);
               MidiExport.ExportMidi($"{fn1}.mid", pattern, chs1, hdr);
               MidiExport.ExportText($"{fn1}.txt", pattern, chs1, hdr);

               var fn2 = Path.Join(Executor.OutputDir, "simple_midi_some");
               List<int> chs2 = [1, 2, 3];
               MidiExport.ExportCsv($"{fn2}.csv", pattern, chs2, hdr);
               MidiExport.ExportMidi($"{fn2}.mid", pattern, chs2, hdr);
               MidiExport.ExportText($"{fn2}.txt", pattern, chs2, hdr);
            });
            Assert(exThrown == null);
        }
    }

    //----------------------------------------------------------------
    public class MLEX_STYLE : TestSuite
    {
        public override void RunSuite()
        {
            StopOnFail(true);

            // Style file, full info:
            var mfd = new MidiDataFile();
            mfd.Read(Path.Join(Executor.InputDir, "_LoveSong.S474.sty"));
            Assert(mfd is not null);

            // Load the new one.
            // long maxTick = 0;
            var pnames = mfd!.GetPatternNames();
            Assert(pnames.Count == 15);

            // Execute the export functions.
            var exThrown = ThrowsNot(() =>
            {
                var pattern = mfd.GetPattern("Main C");

                var hdr = mfd.Header;

                var fn1 = Path.Join(Executor.OutputDir, "style_Main_C");
                List<int> chs1 = [];
                MidiExport.ExportCsv($"{fn1}.csv", pattern, chs1, hdr);
                MidiExport.ExportMidi($"{fn1}.mid", pattern, chs1, hdr);
                MidiExport.ExportText($"{fn1}.txt", pattern, chs1, hdr);
            });
            Assert(exThrown == null);
        }
    }

    //----------------------------------------------------------------
    public class MLEX_DUMP : TestSuite
    {
        public override void RunSuite()
        {
            StopOnFail(true);

            // Style file, full info:
            var fnsimple = Path.Join(Executor.InputDir, "WICKGAME.MID");
            var fnstyle = Path.Join(Executor.InputDir, "_LoveSong.S474.sty");

            var t = MidiExport.Dump(fnsimple, true);
            File.WriteAllLines(Path.Join(Executor.OutputDir, "_dump_simple.txt"), t);

            t = MidiExport.Dump(fnstyle, true);
            File.WriteAllLines(Path.Join(Executor.OutputDir, "_dump_style.txt"), t);
        }
    }
}
