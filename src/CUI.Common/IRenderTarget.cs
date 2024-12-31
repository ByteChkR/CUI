using System.Drawing;
using System.Numerics;

using CUI.Common.Drawing;

namespace CUI.Common.Rendering;

public interface IRenderTarget
{
    Vector2 Size { get; set; }
    IRenderBufferPixel GetPixel(Vector2 position);
    void Clear();
    void Clear(Rectangle area);
    IRenderTarget IgnorePadding(bool ignore);
    bool Contains(Vector2 position);
    IRenderTarget CreateTarget(Transform transform, OverflowMode mode);
}