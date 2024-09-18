using System.Collections.Generic;

namespace FlexRobotics.gfx.Materials
{
    /// <summary>
    /// Has <see cref="IReadOnlyList{TVertex}"/>.
    /// </summary>
    public interface IHaveVertices<out TVertex>
    {
        /// <summary>
        /// Collection of <see cref="TVertex"/>.
        /// </summary>
        TVertex[] Vertices { get; }
    }
}
