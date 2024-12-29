using System.Drawing;
using System.Numerics;

namespace CUI.Common
{
    public interface IRenderTarget
    {
        Vector2 Size { get; }
        RenderBufferPixel GetPixel(Vector2 position);
        void Clear();
        void Clear(Rectangle area);
        IRenderTarget IgnorePadding(bool ignore);
        bool Contains(Vector2 position);
    }
}