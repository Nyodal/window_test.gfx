using MathNet.Spatial.Euclidean;

namespace FlexRobotics.gfx.Mathematics.Extensions
{
    public static class Point2DEx
    {
        public static Point2D ToPoint2D(this System.Drawing.Point point) => new Point2D(point.X, point.Y);

        public static Point2D ToPoint2D(this System.Windows.Point point) => new Point2D(point.X, point.Y);

        public static Point3D ToPoint3D(this Point2D point, double zoffset) => new Point3D(point.X, point.Y, zoffset);
    }
}
