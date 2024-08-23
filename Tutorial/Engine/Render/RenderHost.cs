using FlexRobotics.gfx.Inputs;
using FlexRobotics.gfx.Engine.Commons;
using System;
using System.Drawing;

namespace FlexRobotics.gfx.Engine.Render
{
    /// <summary>
    /// Base class for render host.
    /// </summary>
    public abstract class RenderHost :
        IRenderHost
    {
        #region // storage

        /// <inheritdoc />
        public IntPtr HostHandle { get; private set; }
        /// <inheritdoc />
        public IInput HostInput { get; private set; }

        /// <summary>
        /// Desired buffer size.
        /// </summary>
        protected Size BufferSize { get; private set; }
        /// <summary>
        /// Desired surface size
        /// </summary>
        protected Size HostSize { get; private set; }

        /// <summary>
        /// Viewport size. The size into which buffer will be scaled.
        /// </summary>
        protected Viewport Viewport { get; private set; }
        /// <inheritdoc />
        public FPSCounter FPSCounter { get; private set; }
        /// <summary>
        /// Timestamp when frame was started (UTC).
        /// </summary>
        protected DateTime FrameStarted { get; private set; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected RenderHost(IRenderHostSetup renderHostSetup)
        {
            HostHandle = renderHostSetup.HostHandle;
            HostInput = renderHostSetup.HostInput;

            BufferSize = HostInput.Size;
            HostSize = HostInput.Size;
            Viewport = new Viewport(Point.Empty, HostInput.Size, 0, 1);

            FPSCounter = new FPSCounter(new TimeSpan(0, 0, 0, 0, 1000));

            HostInput.SizeChanged += HostInputOnSizeChanged;
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            HostInput.SizeChanged -= HostInputOnSizeChanged;

            HostInput.Dispose();
            HostInput = default;

            FPSCounter.Dispose();
            FPSCounter = default;

            Viewport = default;
            BufferSize = default;
            HostSize = default;

            HostHandle = default;
        }

        #endregion

        #region // routines

        private void HostInputOnSizeChanged(object sender, ISizeEventArgs args)
        {
            Size Sanitize(Size size)
            {
                if (size.Width < 1 || size.Height < 1)
                {
                    size = new Size(1, 1);
                }
                return size;
            }

            var hostSize = Sanitize(HostInput.Size);
            if (HostSize != hostSize)
            {
                ResizeHost(hostSize);
            }

            var bufferSize = Sanitize(args.NewSize);
            if (BufferSize != bufferSize)
            {
                ResizeBuffers(bufferSize);
            }
        }

        /// <summary>
        /// Resize all buffers.
        /// </summary>
        protected virtual void ResizeBuffers(Size size)
        {
            BufferSize = size;
        }

        /// <summary>
        /// Resize viewport.
        /// </summary>
        protected virtual void ResizeHost(Size size)
        {
            HostSize = size;
            Viewport = new Viewport(Point.Empty, size, 0, 1);
        }

        #endregion

        #region //render

        public void Render()
        {
            FrameStarted = DateTime.UtcNow;
            FPSCounter.StartFrame();
            RenderInternal();
            FPSCounter.StopFrame();
        }

        protected abstract void RenderInternal();

        #endregion
    }
}
