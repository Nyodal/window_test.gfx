using FlexRobotics.gfx.Inputs;
using FlexRobotics.gfx.Engine.Commons;
using System;
using System.Drawing;
using FlexRobotics.gfx.Engine.Commons.Camera;
using FlexRobotics.gfx.Engine.Commons.Camera.Projections;
using MathNet.Spatial.Euclidean;
using FlexRobotics.gfx.Engine.Operators;
using System.Collections.Generic;
using FlexRobotics.gfx.Utilities;

namespace FlexRobotics.gfx.Engine.Render
{
    /// <summary>
    /// Base class for render host.
    /// </summary>
    public abstract class RenderHost :
        IRenderHost
    {
        #region // storage

        /// <inheritdoc />
        public IntPtr HostHandle { get; private set; }
        /// <inheritdoc />
        public IInput HostInput { get; private set; }

        /// <summary>
        /// Desired buffer size.
        /// </summary>
        protected Size BufferSize { get; private set; }
        /// <summary>
        /// Desired surface size
        /// </summary>
        public Size HostSize { get; private set; }

        /// <summary>
        /// Viewport size. The size into which buffer will be scaled.
        /// </summary>
        protected Viewport Viewport { get; private set; }

        /// <inheritdoc cref="CameraInfo" />
        private ICameraInfo m_CameraInfo;

        /// <inheritdoc />
        public ICameraInfo CameraInfo
        {
            get => m_CameraInfo;
            set
            {
                m_CameraInfo = value;
                CameraInfoChanged?.Invoke(this, m_CameraInfo);
            }
        }
        /// <inheritdoc />
        public FPSCounter FPSCounter { get; private set; }
        /// <summary>
        /// Timestamp when frame was started (UTC).
        /// </summary>
        protected DateTime FrameStarted { get; private set; }

        /// <summary>
        /// Active operators.
        /// </summary>
        protected IEnumerable<IOperator> Operators { get; set; }


        #endregion

        #region // events

        public event EventHandler<ICameraInfo> CameraInfoChanged;

        #endregion

        #region // ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected RenderHost(IRenderHostSetup renderHostSetup)
        {
            HostHandle = renderHostSetup.HostHandle;
            HostInput = renderHostSetup.HostInput;

            HostSize = HostInput.Size;
            BufferSize = HostInput.Size;
            CameraInfo = new CameraInfo
            (
                new Point3D(1, 1, 1),
                new Point3D(0, 0, 0),
                UnitVector3D.ZAxis,
                //new ProjectionPerspective(0.001, 1000, Math.PI * 0.5, 0.5),
                new ProjectionOrthographic(0.001, 1000, 2, 2),
                new Viewport(0, 0, 1, 1, 0, 1)
            );
            FPSCounter = new FPSCounter(new TimeSpan(0, 0, 0, 0, 1000));

            Operators = new List<IOperator>
            {
                new OperatorResize(this, ResizeHost),
                new OperatorCameraZoom(this),
            };

            OperatorResize.Resize(this, HostSize, ResizeHost);
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            Operators.ForEach(o => o.Dispose());

            HostInput.Dispose();
            HostInput = default;

            FPSCounter.Dispose();
            FPSCounter = default;

            CameraInfo = default;
            BufferSize = default;
            HostSize = default;

            HostHandle = default;
        }

        #endregion

        #region // routines


        /// <summary>
        /// Resize all buffers.
        /// </summary>
        protected virtual void ResizeBuffers(Size size)
        {
            BufferSize = size;
        }

        /// <summary>
        /// Resize viewport.
        /// </summary>
        protected virtual void ResizeHost(Size size)
        {
            HostSize = size;
        }

        /// <summary>
        /// Ensure <see cref="BufferSize"/> are synced with <see cref="ICameraInfo"/>
        /// </summary>
        protected void EnsureBufferSize()
        {
            var size = CameraInfo.Viewport.Size;
            if (BufferSize != size)
            {
                ResizeBuffers(size);
            }
        }

        #endregion

        #region //render

        public void Render()
        {
            EnsureBufferSize();
            FrameStarted = DateTime.UtcNow;
            FPSCounter.StartFrame();
            RenderInternal();
            FPSCounter.StopFrame();
        }

        protected abstract void RenderInternal();

        #endregion
    }
}
