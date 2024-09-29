using FlexRobotics.gfx.Mathematics;

namespace FlexRobotics.gfx.Drivers.Gdi.Materials.Position
{
    /// <inheritdoc cref="IVertexShader"/>
    public readonly struct Vertex :
        IVertex
    {
        #region // storage

        /// <inheritdoc />
        public Vector4F Position { get; }

        #endregion

        #region // ctor

        /// <summary />
        public Vertex(Vector4F position)
        {
            Position = position;
        }

        #endregion

        #region // routines

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Position: {Position}";
        }

        #endregion
    }
}
