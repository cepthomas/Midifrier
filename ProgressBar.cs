using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Ephemera.NBagOfTricks;
using System.Windows.Forms.VisualStyles;
using Ephemera.MidiLib;

//TODO1 put in midilib or???


namespace Midifrier
{
    /// <summary>The control.</summary>
    [DesignTimeVisible(true)]
    [Browsable(false)]
    public class ProgressBar : UserControl
    {
        #region Fields
        /// <summary>For tracking mouse moves.</summary>
        int _lastXPos = 0;

        /// <summary>Tooltip for mousing.</summary>
        readonly ToolTip _toolTip = new();

        /// <summary>For drawing text.</summary>
        readonly StringFormat _format = new();

        /// <summary>For drawing lines etc.</summary>
        readonly Pen _penMarker = new(Color.Red, 1);
        #endregion

        #region Properties
        /// <summary>Big font.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Font FontLarge { get; set; } = new("Microsoft Sans Serif", 16, FontStyle.Regular, GraphicsUnit.Point, 0);

        /// <summary>Baby font.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Font FontSmall { get; set; } = new("Microsoft Sans Serif", 10, FontStyle.Regular, GraphicsUnit.Point, 0);

        /// <summary>Drawing the active elements of a control.</summary>
        public Color DrawColor { get; set; } = Color.Red;

        /// <summary>Keep going at end.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool DoLoop { get; set; } = false;

        /// <summary>Convenience for readability.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MusicTime Length { get; set; } = MusicTime.ZERO;

        /// <summary>Convenience for readability.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MusicTime Current { get; } = MusicTime.ZERO;
        #endregion

        #region Events
        /// <summary>Something happened.</summary>
        public event EventHandler<StateChangeEventArgs>? StateChange;
        public class StateChangeEventArgs : EventArgs
        {
            /// <summary>User changed current time.</summary>
            public bool CurrentTimeChange { get; set; } = false;
        }
        #endregion

        #region Lifecycle
        /// <summary>
        /// Normal constructor.
        /// </summary>
        public ProgressBar()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// Post-construct.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            _penMarker.Color = DrawColor;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _toolTip.Dispose();
                _penMarker.Dispose();
                _format.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Something to say.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var s = $"current:{Current.Tick} length:{Length.Tick}";
            return s;
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Back up dude.
        /// </summary>
        public void Rewind()
        {
            Current.Reset();
            Invalidate();
        }

        /// <summary>
        /// Step current time.
        /// </summary>
        /// <returns>True if still running.</returns>
        public bool Increment()
        {
            bool running = true;

            if (Current >= Length) // at end
            {
                if (DoLoop) // continue from start
                {
                    Current.Set(0);
                }
                else // stop
                {
                    running = false;
                }
            }
            else // just continue
            {
                Current.Add(1);
            }

            Invalidate();

            return running;
        }
        #endregion

        #region Drawing
        /// <summary>
        /// Draw the control.
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            // Setup.
            pe.Graphics.Clear(BackColor);

            // Current pos.
            int markSize = 7;
            int cpos = GetClientFromTick(Current.Tick);
            pe.Graphics.DrawLine(_penMarker, cpos, 0, cpos, Height);
            PointF[] ploc = [new(cpos - markSize, 0), new(cpos + markSize, 0), new(cpos, 2 * markSize)];
            pe.Graphics.FillPolygon(_penMarker.Brush, ploc);

            // Text.
            _format.Alignment = StringAlignment.Center;
            _format.LineAlignment = StringAlignment.Near; // Center;
            pe.Graphics.DrawString(Current.ToString(), FontLarge, Brushes.Black, ClientRectangle, _format);

            _format.Alignment = StringAlignment.Far;
            _format.LineAlignment = StringAlignment.Near;
            pe.Graphics.DrawString(Length.ToString(), FontSmall, Brushes.Black, ClientRectangle, _format);
        }
        #endregion

        #region UI handlers
        /// <summary>
        /// Mouse position changes.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // SetToolTip triggers mouse move event - infernal loop.
            if (e.X != _lastXPos)
            {
                _lastXPos = e.X;

                // Assemble info.
                var actualTick = GetTickFromClient(e.X);
                MusicTime mt = new(actualTick);
                _toolTip.SetToolTip(this, $"{mt}");
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Selection of time and loop points.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            int newval = GetTickFromClient(e.X);
            Current.Set(newval);
            StateChange?.Invoke(this, new() { CurrentTimeChange = true });
            Invalidate();

            base.OnMouseDown(e);
        }
        #endregion

        #region Private functions
        /// <summary>
        /// Convert x pos to tick.
        /// </summary>
        /// <param name="x"></param>
        int GetTickFromClient(int x)
        {
            int tick = 0;

            if (Current < Length)
            {
                tick = x * Length.Tick / Width;
                tick = MathUtils.Constrain(tick, 0, Length.Tick);
            }

            return tick;
        }

        /// <summary>
        /// Map from time to UI pixels.
        /// </summary>
        /// <param name="tick"></param>
        /// <returns></returns>
        int GetClientFromTick(int tick)
        {
            return Length.Tick > 0 ? tick * Width / Length.Tick : 0;
        }
        #endregion
    }
}
