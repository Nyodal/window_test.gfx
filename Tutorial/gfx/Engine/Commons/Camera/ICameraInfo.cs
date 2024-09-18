using FlexRobotics.gfx.Engine.Commons.Camera.Projections;
using MathNet.Spatial.Euclidean;
using System;

namespace FlexRobotics.gfx.Engine.Commons.Camera
{
    /// <summary>
    /// Camera information describing view.
    /// </summary>
    public interface ICameraInfo :
        ICloneable
    {
        /// <summary>
        /// Camera position.
        /// </summary>
        Point3D Position { get; }

        /// <summary>
        /// Camera target.
        /// </summary>
        Point3D Target { get; }

        /// <summary>
        /// Camera up vector.
        /// </summary>
        UnitVector3D UpVector { get; }

        /// <summary>
        /// Camera projection.
        /// </summary>
        IProjection Projection { get; }

        /// <summary>
        /// Camera viewport.
        /// </summary>
        Viewport Viewport { get; }

        /// <inheritdoc cref="ICameraInfoCache"/>
        ICameraInfoCache Cache { get; }
    }
}
