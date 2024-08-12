using System;
using System.Drawing;


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

        #endregion

        #region // ctor
        /// <summary>
        /// Constructor.
        /// </summary>
        public RenderHost(IntPtr hostHandle) :
            base(hostHandle)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);
            FontConsolas12 = new Font("Consolas", 11f);
        }

        public override void Dispose()
        {
            GraphicsHost?.Dispose();
            GraphicsHost = default;

            FontConsolas12?.Dispose();
            FontConsolas12 = default;

            base.Dispose();
        }

        #endregion

        #region // render

        protected override void RenderInternal()
        {
            GraphicsHost.Clear(Color.Black);
            GraphicsHost.DrawString(FPSCounter.FPSString, FontConsolas12, Brushes.Red, 0, 0); 
        }

        #endregion
    }
}
