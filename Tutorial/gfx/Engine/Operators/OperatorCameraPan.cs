﻿using FlexRobotics.gfx.Engine.Common.Camera;
using FlexRobotics.gfx.Engine.Render;
using FlexRobotics.gfx.Inputs;
using FlexRobotics.gfx.Mathematics;
using FlexRobotics.gfx.Mathematics.Extensions;
using FlexRobotics.gfx.Utilities;
using MathNet.Spatial.Euclidean;

namespace FlexRobotics.gfx.Engine.Operators
{
    /// <summary>
    /// Camera pan operator.
    /// </summary>
    public class OperatorCameraPan :
        Operator
    {
        #region // storage

        /// <summary>
        /// Camera info at the time when mouse was pressed down.
        /// </summary>
        private ICameraInfo MouseDownCameraInfo { get; set; }

        /// <summary>
        /// Plane which goes through <see cref="GetPanOrigin"/> and is perpendicular
        /// to <see cref="ICameraInfoEx.GetEyeDirection"/> at the time when mouse was pressed down.
        /// </summary>
        private Plane? MouseDownPlane { get; set; }

        /// <summary>
        /// Mouse ray intersected with <see cref="MouseDownPlane"/> at the time when mouse was pressed down.
        /// </summary>
        private Point3D? MouseDownOnPlane { get; set; }

        #endregion

        #region // ctor

        /// <inheritdoc />
        public OperatorCameraPan(IRenderHost renderHost) :
            base(renderHost)
        {
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            MouseDownCameraInfo = default;
            MouseDownPlane = default;
            MouseDownOnPlane = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        /// <summary>
        /// Get pan origin.
        /// </summary>
        private static Point3D GetPanOrigin(ICameraInfo cameraInfo)
        {
            return cameraInfo.Target;
        }

        /// <inheritdoc />
        protected override void InputOnMouseDown(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseDown(sender, args);

            if (args.Buttons != MouseButtons.Right) return;

            MouseDownCameraInfo = RenderHost.CameraInfo.Cloned();

            // construct plane
            var panOrigin = GetPanOrigin(MouseDownCameraInfo);
            MouseDownPlane = new Plane(panOrigin, MouseDownCameraInfo.GetEyeDirection());

            // intersect mouse ray with plane
            var mouseRay = MouseDownCameraInfo.GetMouseRay(Space.Screen, args.Position.ToPoint3D(0));
            MouseDownOnPlane = MouseDownPlane.Value.IntersectionWith(mouseRay);
        }

        /// <inheritdoc />
        protected override void InputOnMouseUp(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseUp(sender, args);

            if (args.Buttons != MouseButtons.Right) return;

            MouseDownCameraInfo = default;
            MouseDownPlane = default;
            MouseDownOnPlane = default;
        }

        /// <inheritdoc />
        protected override void InputOnMouseMove(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseMove(sender, args);

            if (MouseDownCameraInfo is null || !MouseDownPlane.HasValue || !MouseDownOnPlane.HasValue) return;

            // intersect mouse ray with plane
            var mouseRay = MouseDownCameraInfo.GetMouseRay(Space.Screen, args.Position.ToPoint3D(0));
            var mouseMoveOnPlane = MouseDownPlane.Value.IntersectionWith(mouseRay);

            // figure out new camera position and target
            var offset = mouseMoveOnPlane - MouseDownOnPlane.Value;
            var target = MouseDownCameraInfo.Target - offset;
            var position = MouseDownCameraInfo.Position - offset;

            // set camera info
            var cameraInfo = RenderHost.CameraInfo;
            RenderHost.CameraInfo = new CameraInfo(position, target, cameraInfo.UpVector, cameraInfo.Projection.Cloned(), cameraInfo.Viewport);
        }

        #endregion

    }
}
