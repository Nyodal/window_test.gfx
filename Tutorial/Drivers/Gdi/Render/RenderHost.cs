using System;
using System.Drawing;
using System.Windows.Media.Animation;
using FlexRobotics.gfx.Win;


namespace FlexRobotics.gfx.Drivers.Gdi.Render
{
    /// <summary>
    /// Gdi render host.
    /// </summary>
    public class RenderHost :
        Engine.Render.RenderHost
    {
        #region // storage

        private Graphics GraphicsHost {  get; set; }
        private Font FontConsolas12 { get; set; }
        /// <summary>
        /// double buffer wrapper
        /// </summary>
        private BufferedGraphics BufferedGraphics { get; set; }

        #endregion

        #region // ctor
        /// <summary>
        /// Constructor.
        /// </summary>
        public RenderHost(IntPtr hostHandle) :
            base(hostHandle)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);

            BufferedGraphics = BufferedGraphicsManager.Current.Allocate(GraphicsHost, new Rectangle(Point.Empty, W.GetClientRectangle(hostHandle).Size));

            FontConsolas12 = new Font("Consolas", 12f);
        }

        public override void Dispose()
        {
            GraphicsHost.Dispose();
            GraphicsHost = default;

            BufferedGraphics.Dispose();
            BufferedGraphics = default;

            FontConsolas12.Dispose();
            FontConsolas12 = default;

            base.Dispose();
        }

        #endregion

        #region // render

        protected override void RenderInternal()
        {
            BufferedGraphics.Graphics.Clear(Color.Black);
            BufferedGraphics.Graphics.DrawString(FPSCounter.FPSString, FontConsolas12, Brushes.Red, 0, 0);

            BufferedGraphics.Render();
        }

        #endregion
    }
}
