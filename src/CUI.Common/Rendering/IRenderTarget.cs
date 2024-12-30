using System.Drawing;
using System.Numerics;

using CUI.Common.Rendering.Buffer;

namespace CUI.Common.Rendering;

public interface IRenderTarget
{
    Vector2 Size { get; }
    RenderBufferPixel GetPixel(Vector2 position);
    void Clear();
    void Clear(Rectangle area);
    IRenderTarget IgnorePadding(bool ignore);
    bool Contains(Vector2 position);
}