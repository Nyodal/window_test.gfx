using FlexRobotics.gfx.Inputs;
using System;

namespace FlexRobotics.gfx.Engine.Render
{
    /// <summary>
    /// Provides data required for <see cref="IRenderHost"/> creation.
    /// </summary>
    public interface IRenderHostSetup
    {
        /// <inheritdoc cref="IRenderHost.HostHandle" />
        IntPtr HostHandle { get; }

        /// <inheritdoc cref="IRenderHost.HostInput" />
        IInput HostInput { get; }
    }
}
