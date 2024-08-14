using FlexRobotics.gfx.Inputs;
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
        IInput HostInput { get; }
        FPSCounter FPSCounter { get; }
        void Render();
    }
}
