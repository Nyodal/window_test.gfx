using FlexRobotics.gfx.Inputs;
using System;

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
        public IInput HostInput { get; private set; }
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

            FPSCounter = new FPSCounter(new TimeSpan(0, 0, 0, 0, 1000));
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            FPSCounter.Dispose();
            FPSCounter = default;

            HostHandle = default;

            HostInput?.Dispose();
            HostInput = default;
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
