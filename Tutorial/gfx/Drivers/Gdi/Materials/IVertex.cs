using FlexRobotics.gfx.Mathematics;

namespace FlexRobotics.gfx.Drivers.Gdi.Materials
{
    /// <summary>
    /// Internal shader vertex interface.
    /// </summary>
    public interface IVertex
    {
        /// <summary>
        /// Clip space position (vertex shader output).
        /// </summary>
        Vector4F Position { get; }
    }
}
