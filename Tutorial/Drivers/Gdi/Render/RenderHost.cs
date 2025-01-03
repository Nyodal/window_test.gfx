﻿using FlexRobotics.gfx.Engine.Render;
using FlexRobotics.gfx.Utilities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;


namespace FlexRobotics.gfx.Drivers.Gdi.Render
{
    /// <summary>
    /// Gdi render host.
    /// </summary>
    public class RenderHost :
        Engine.Render.RenderHost
    {
        #region // storage

        /// <summary>
        /// Graphics retrieved from <see cref="IRenderHost.HostHandle"/>.
        /// </summary>
        private Graphics GraphicsHost { get; set; }

        /// <summary>
        /// Device context of <see cref="GraphicsHost"/>.
        /// </summary>
        private IntPtr GraphicsHostDeviceContext { get; set; }

        /// <summary>
        /// Double buffer wrapper.
        /// </summary>
        private BufferedGraphics BufferedGraphics { get; set; }

        /// <summary>
        /// Back buffer.
        /// </summary>
        private DirectBitmap BackBuffer { get; set; }

        /// <summary>
        /// Font for drawing text with <see cref="System.Drawing"/> objects.
        /// </summary>
        private Font FontConsolas12 { get; set; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RenderHost(IRenderHostSetup renderHostSetup) :
            base(renderHostSetup)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);
            GraphicsHostDeviceContext = GraphicsHost.GetHdc();
            CreateBuffers(BufferSize);
            CreateViewport(ViewportSize);
            FontConsolas12 = new Font("Consolas", 12);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            FontConsolas12.Dispose();
            FontConsolas12 = default;

            DisposeBuffers();
            DisposeViewport();

            GraphicsHost.ReleaseHdc(GraphicsHostDeviceContext);
            GraphicsHostDeviceContext = default;

            GraphicsHost.Dispose();
            GraphicsHost = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        protected override void ResizeBuffers(Size size)
        {
            base.ResizeBuffers(size);

            DisposeBuffers();
            CreateBuffers(size);
        }

        /// <inheritdoc />
        protected override void ResizeViewport(Size size)
        {
            base.ResizeViewport(size);

            DisposeViewport();
            CreateViewport(size);
        }

        private void CreateBuffers(Size size)
        {
            BackBuffer = new DirectBitmap(size);
        }

        private void DisposeBuffers()
        {
            BackBuffer.Dispose();
            BackBuffer = default;
        }

        private void CreateViewport(Size size)
        {
            BufferedGraphics = BufferedGraphicsManager.Current.Allocate(GraphicsHostDeviceContext, new Rectangle(Point.Empty, size));
            BufferedGraphics.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        private void DisposeViewport()
        {
            BufferedGraphics.Dispose();
            BufferedGraphics = default;
        }

        #endregion

        #region // render

        /// <inheritdoc />
        protected override void RenderInternal()
        {
            var graphics = BackBuffer.Graphics;

            var t = DateTime.UtcNow.Millisecond / 1000.0;
            var s = Math.Sin(t * Math.PI);
            Color GetColor(int x, int y) => Color.FromArgb
            (
                byte.MaxValue,
                (byte)((double)x / BufferSize.Width * byte.MaxValue),
                (byte)((double)y / BufferSize.Height * byte.MaxValue),
                (byte)(s * byte.MaxValue)
            );

            Parallel.For(0, BackBuffer.Buffer.Length, index =>
            {
                BackBuffer.GetXY(index, out var x, out var y);
                BackBuffer.Buffer[index] = GetColor(x, y).ToArgb();
            });

            graphics.DrawString(FPSCounter.FPSString, FontConsolas12, Brushes.Red, 0, 0);
            graphics.DrawString($"Buffer   = {BufferSize.Width}, {BufferSize.Height}", FontConsolas12, Brushes.Cyan, 0, 16);
            graphics.DrawString($"Viewport = {ViewportSize.Width}, {ViewportSize.Height}", FontConsolas12, Brushes.Cyan, 0, 32);


            // flush and swap buffers
            BufferedGraphics.Graphics.DrawImage(BackBuffer.Bitmap, new RectangleF(PointF.Empty, ViewportSize), new RectangleF(new PointF(-0.5f, -0.5f), BufferSize), GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }

        #endregion
    }
}
