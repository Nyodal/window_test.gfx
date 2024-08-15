using FlexRobotics.gfx.Inputs;
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
        /// Viewport size. The size into which buffer will be scaled.
        /// </summary>
        protected Size ViewportSize { get; private set; }
        /// <inheritdoc />
        public FPSCounter FPSCounter { get; private set; }

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
            ViewportSize = HostInput.Size;

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

            BufferSize = default;
            ViewportSize = default;

            HostHandle = default;
        }

        #endregion

        #region // routines

        private void HostInputOnSizeChanged(object sender, ISizeEventArgs args)
        {
            var size = args.NewSize;

            // sanity check
            if (size.Width < 1 || size.Height < 1)
            {
                size = new Size(1, 1);
            }

            ResizeBuffers(size);
            ResizeViewport(size);
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
        protected virtual void ResizeViewport(Size size)
        {
            ViewportSize = size;
        }

        #endregion

        #region //render

        public void Render()
        {
            FPSCounter.StartFrame();
            RenderInternal();
            FPSCounter.StopFrame();
        }

        protected abstract void RenderInternal();

        #endregion
    }
}
