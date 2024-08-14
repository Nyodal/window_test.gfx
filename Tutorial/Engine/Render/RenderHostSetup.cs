using FlexRobotics.gfx.Inputs;
using System;

namespace FlexRobotics.gfx.Engine.Render
{
    /// <inheritdoc />
    public class RenderHostSetup :
        IRenderHostSetup
    {
        #region // storage

        /// <inheritdoc />
        public IntPtr HostHandle { get; }

        /// <inheritdoc />
        public IInput HostInput { get; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public RenderHostSetup(IntPtr hostHandle, IInput hostInput)
        {
            HostHandle = hostHandle;
            HostInput = hostInput;
        }

        #endregion
    }
}
