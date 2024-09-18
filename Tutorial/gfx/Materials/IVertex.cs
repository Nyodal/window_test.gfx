using FlexRobotics.gfx.Mathematics;

namespace FlexRobotics.gfx.Materials
{
    /// <summary>
    /// Defines raw vertex.
    /// </summary>
    public interface IVertex
    {
    }

    /// <summary>
    /// Defines <see cref="IVertex"/> which has <see cref="Position"/>.
    /// </summary>
    public interface IVertexPosition :
        IVertex
    {
        Vector3F Position { get; }
    }
}
