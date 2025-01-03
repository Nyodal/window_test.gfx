﻿using FlexRobotics.gfx.Engine.Render;
using FlexRobotics.gfx.Inputs;
using FlexRobotics.gfx.Utilities;
using FlexRobotics.gfx.Win;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexRobotics.gfx.Client
{
    public static class WindowFactory
    {
        /// <summary>
        /// Create various windows for rendering.
        /// </summary>
        public static IReadOnlyList<IRenderHost> SeedWindows()
        {
            // arbitrary default window size
            var size = new System.Drawing.Size(720, 480);

            // create windows (and render hosts)
            var renderHosts = new[]
            {
                CreateWindowForm(size, "Forms Gdi", rhs => new Drivers.Gdi.Render.RenderHost(rhs)),
                CreateWindowWpf(size, "Wpf Gdi", rhs => new Drivers.Gdi.Render.RenderHost(rhs)),
            };

            // sort windows in the middle of screen
            SortWindows(renderHosts);

            return renderHosts;
        }

        /// <summary>
        /// Create <see cref="System.Windows.Forms.Control"/> API create host control
        /// </summary>
        /// <returns></returns>
        private static System.Windows.Forms.Control CreateHostControl()
        {
            var hostControl = new System.Windows.Forms.Panel()
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackColor = System.Drawing.Color.Transparent,
                ForeColor = System.Drawing.Color.Transparent,
            };

            // focus control to capture mouse wheel events
            void EnsureFocus(System.Windows.Forms.Control control)
            {
                if (!control.Focused) control.Focus();
            }

            hostControl.MouseEnter += (sender, args) => EnsureFocus(hostControl);
            hostControl.MouseClick += (sender, args) => EnsureFocus(hostControl);

            return hostControl;
        }

        /// <summary>
        /// Create <see cref="System.Windows.Forms.Form"/> and <see cref="IRenderHost"/> for it.
        /// </summary>
        private static IRenderHost CreateWindowForm(System.Drawing.Size size, string title, Func<IRenderHostSetup, IRenderHost> ctorRenderHost)
        {
            var window = new System.Windows.Forms.Form
            {
                Size = size,
                Text = title,
            };

            var hostControl = CreateHostControl();
            window.Controls.Add(hostControl);

            window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();

            window.Show();

            return ctorRenderHost(new RenderHostSetup(hostControl.Handle(), new InputForms(hostControl)));
        }

        /// <summary>
        /// Create <see cref="System.Windows.Window"/> and <see cref="IRenderHost"/> for it.
        /// </summary>
        private static IRenderHost CreateWindowWpf(System.Drawing.Size size, string title, Func<IRenderHostSetup, IRenderHost> ctorRenderHost)
        {
            var window = new System.Windows.Window
            {
                Width = size.Width,
                Height = size.Height,
                Title = title,
            };

            var hostControl = CreateHostControl();

            // create forms host (wrapper for wpf)
            var windowsFormsHost = new System.Windows.Forms.Integration.WindowsFormsHost
            {
                Child = hostControl,
            };

            window.Content = windowsFormsHost;

            window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();

            window.Show();

            return ctorRenderHost(new RenderHostSetup(hostControl.Handle(), new InputForms(hostControl)));
        }

        /// <summary>
        /// Sort windows in the middle of primary screen.
        /// </summary>
        private static void SortWindows(IEnumerable<IRenderHost> renderHosts)
        {
            // get window infos from handles
            var windowInfos = renderHosts.Select(renderHost => new WindowInfo(renderHost.HostHandle).GetRoot()).ToArray();

            // figure out max size of window
            var maxSize = new System.Drawing.Size(windowInfos.Max(a => a.RectangleWindow.Width), windowInfos.Max(a => a.RectangleWindow.Height));

            // get max columns and rows
            var maxColumns = (int)Math.Ceiling(Math.Sqrt(windowInfos.Length));
            var maxRows = (int)Math.Ceiling((double)windowInfos.Length / maxColumns);

            // get initial top left corner
            var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen;
            var left = primaryScreen.WorkingArea.Width / 2 - maxColumns * maxSize.Width / 2;
            var top = primaryScreen.WorkingArea.Height / 2 - maxRows * maxSize.Height / 2;

            // try to move windows
            for (var row = 0; row < maxRows; row++)
            {
                for (var column = 0; column < maxColumns; column++)
                {
                    // figure out window index
                    var i = row * maxColumns + column;

                    // if it's too big, we moved all of them
                    if (i >= windowInfos.Length) return;

                    // get top left coordinate for window
                    var x = column * maxSize.Width + left;
                    var y = row * maxSize.Height + top;

                    // get window info
                    var windowInfo = windowInfos[i];

                    // move window
                    User32.MoveWindow(windowInfo.Handle, x, y, windowInfo.RectangleWindow.Width, windowInfo.RectangleWindow.Height, false);
                }
            }
        }
    }
}
