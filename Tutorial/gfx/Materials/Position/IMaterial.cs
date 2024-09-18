using System.Drawing;

namespace FlexRobotics.gfx.Materials.Position
{
    /// <summary>
    /// Mono color <see cref="Materials.IMaterial"/>.
    /// </summary>
    public interface IMaterial :
         Materials.IMaterial
    {
        /// <summary>
        /// Color to use for rendering.
        /// </summary>
        Color Color { get; }
    }
}
