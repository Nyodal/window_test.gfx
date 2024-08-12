using System;

namespace FlexRobotics.gfx.Engine.Render
{
    /// <summary>
    /// Interface for render host.
    /// </summary>
    public interface IRenderHost :
        IDisposable
    {
        /// <summary>
        /// Handle of hosting window.
        /// </summary>
        IntPtr HostHandle { get; }
        FPSCounter FPSCounter { get; }
        void Render();
    }
}
