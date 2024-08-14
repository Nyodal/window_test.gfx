using System;
using System.Diagnostics;

namespace FlexRobotics.gfx.Engine.Render
{
    public class FPSCounter :
        IDisposable
    {
        #region // storage

        public TimeSpan UpdateRate { get; }
        public double FPSRender { get; private set; }
        public double FPSGlobal { get; private set; }
        public string FPSString => $"FPS: {FPSRender:0} ({FPSGlobal:0})";
        private TimeSpan Elapsed { get; set; }
        private int FrameCount { get; set; }
        private Stopwatch StopwatchUpdate { get; set; }
        private Stopwatch StopwatchFrame { get; set; }


        #endregion

        #region // ctor

        public FPSCounter(TimeSpan updateRate)
        {
            UpdateRate = updateRate;

            StopwatchUpdate = new Stopwatch();
            StopwatchFrame = new Stopwatch();

            StopwatchUpdate.Start();

            Elapsed = TimeSpan.Zero;
        }

        public void Dispose()
        {
            StopwatchUpdate.Stop();
            StopwatchUpdate = default;

            StopwatchFrame.Stop();
            StopwatchFrame = default;
        }

        #endregion

        #region // routines

        public void StartFrame()
        {
            StopwatchFrame.Restart();
        }

        public void StopFrame()
        {
            StopwatchFrame.Stop();
            Elapsed = StopwatchFrame.Elapsed;
            FrameCount++;

            var updateElapsed = StopwatchUpdate.Elapsed;
            if (updateElapsed >= UpdateRate)
            {
                FPSRender = FrameCount / Elapsed.TotalSeconds;
                FPSGlobal = FrameCount / updateElapsed.TotalSeconds;

                StopwatchUpdate.Restart();
                Elapsed = TimeSpan.Zero;
                FrameCount = 0;
            }
        }

        #endregion
    }
}
