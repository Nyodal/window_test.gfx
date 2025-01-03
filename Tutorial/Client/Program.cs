﻿using FlexRobotics.gfx.Engine.Render;
using FlexRobotics.gfx.Utilities;
using System;
using System.Collections.Generic;

namespace FlexRobotics.gfx.Client
{
    /// <summary>
    /// Program for client side to use graphics modules.
    /// </summary>
    internal class Program :
        System.Windows.Application,
        IDisposable
    {
        #region // storage

        /// <summary>
        /// Our created render hosts.
        /// </summary>
        private IReadOnlyList<IRenderHost> RenderHosts { get; set; }

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Program()
        {
            // call actual constructor once our application is initialized
            Startup += (sender, args) => Ctor();

            // dispose resources when application is exiting
            Exit += (sender, args) => Dispose();
        }

        /// <summary>
        /// Functional constructor.
        /// </summary>
        private void Ctor()
        {
            // create some render hosts for rendering
            RenderHosts = WindowFactory.SeedWindows();

            // render loop
            while (!Dispatcher.HasShutdownStarted)
            {
                Render(RenderHosts);
                System.Windows.Forms.Application.DoEvents();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // dispose all render hosts we have
            RenderHosts.ForEach(host => host.Dispose());
            RenderHosts = default;
        }

        #endregion

        #region // render

        private static void Render(IEnumerable<IRenderHost> renderHosts)
        {
            renderHosts.ForEach(rh => rh.Render());
        }

        #endregion
    }
}
