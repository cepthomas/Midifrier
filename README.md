# Midifrier

- A windows tool for playing midi files and Yamaha style files.
- This is primarily intended to be used for auditioning parts for use in compositions created in a real DAW.
- Because the windows multimedia timer has inadequate accuracy for midi notes, resolution is limited to 32nd notes.
- Minimal attention has been paid to aesthetics over functionality.
- Midi play devices are limited to the ones available on your box. (Hint- VirtualMidiSynth).
- Since midi files and NAudio use 1-based channel numbers, so does this application, except when used as an array index.
- If midi file type is `1`, all tracks are combined. Because.
- Tons of styles and info at https://psrtutorial.com/.

Requires VS2022 and .NET6.

# Usage

- The simple UI shows a tree directory navigator on the left and standard audio transport family of controls on the right.
- If midi file type is `1`, all tracks are combined. Because.
- Opens style files and plays the individual sections.
- Export style files as their component parts.
- Export current selection(s) and channel(s) to a new midi file. Useful for snipping style patterns.
- Some midi files with single instruments are sloppy with channel numbers so there are a couple of options for simple remapping.
- Click on the settings icon to edit your options. Note that not all midi options pertain to this application.
- In the log view: C for clear, W for word wrap toggle.

# External Components

- [NAudio](https://github.com/naudio/NAudio) (Microsoft Public License).
- Application icon: [Charlotte Schmidt](http://pattedemouche.free.fr/) (Copyright © 2009 of Charlotte Schmidt).
- Button icons: [Glyphicons Free](http://glyphicons.com/) (CC BY 3.0).
