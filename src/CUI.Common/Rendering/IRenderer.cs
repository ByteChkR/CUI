using CUI.Common.Rendering.Buffer;

namespace CUI.Common.Rendering
{
    public interface IRenderer
    {
        RenderBuffer Buffer { get; }
        void Render();
    }
}