using FlexRobotics.gfx.Engine.Commons.Camera;
using FlexRobotics.gfx.Inputs;
using System;
using System.Drawing;

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

        /// <summary>
        /// Input from host.
        /// </summary>
        IInput HostInput { get; }

        /// <inheritdoc cref="ICameraInfo"/>
        ICameraInfo CameraInfo { get; set; }

        /// <summary>
        /// Desired surface size.
        /// </summary>
        Size HostSize { get; }

        /// <inheritdoc cref="Engine.Render.FpsCounter"/>
        FPSCounter FPSCounter { get; }

        /// <summary>
        /// Render.
        /// </summary>
        void Render();

        /// <summary>
        /// Fires when <see cref="CameraInfo"/> changed.
        /// </summary>
        event EventHandler<ICameraInfo> CameraInfoChanged;
    }
}
