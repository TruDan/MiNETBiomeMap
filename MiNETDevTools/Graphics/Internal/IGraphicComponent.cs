using System;

namespace MiNETDevTools.Graphics.Internal
{
    public interface IGraphicComponent : IDisposable
    {
        void InitialiseGraphics(GraphicsDevice device);

        void CreateResources(GraphicsDevice device);

        void DrawFrame(GraphicsDevice device);
    }
}
