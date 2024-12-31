using System.Numerics;

using CUI.Common.Drawing;
using CUI.Common.Rendering;

namespace CUI.Common.Components.Containers.Panels;

public class BorderedPanel : Panel
{
    private BorderOptions _borderOptions;
    public BorderOptions BorderOptions
    {
        get => _borderOptions;
        set => _borderOptions = value;
    }
        
    public BorderedPanel(BorderOptions borderOptions)
    {
        _borderOptions = borderOptions;
        Transform.Padding = new Offset(1);
    }

    protected override void RenderSelf(IRenderTarget target)
    {
            
        IRenderTarget newTarget = target.IgnorePadding(true);
        Vector2 size = newTarget.Size;
        IRenderBufferPixel topLeft = newTarget.GetPixel(Vector2.Zero);
        IRenderBufferPixel topRight = newTarget.GetPixel(new Vector2(size.X - 1, 0));
        IRenderBufferPixel bottomLeft = newTarget.GetPixel(new Vector2(0, size.Y - 1));
        IRenderBufferPixel bottomRight = newTarget.GetPixel(size - Vector2.One);
        for (int x = 1; x < size.X - 1; x++)
        {
            newTarget.GetPixel(new Vector2(x, 0)).Character = _borderOptions.Horizontal;
            newTarget.GetPixel(new Vector2(x, size.Y - 1)).Character = _borderOptions.Horizontal;
        }
        for (int y = 1; y < size.Y - 1; y++)
        {
            newTarget.GetPixel(new Vector2(0, y)).Character = _borderOptions.Vertical;
            newTarget.GetPixel(new Vector2(size.X - 1, y)).Character = _borderOptions.Vertical;
        }
        topLeft.Character = _borderOptions.TopLeft;
        topRight.Character = _borderOptions.TopRight;
        bottomLeft.Character = _borderOptions.BottomLeft;
        bottomRight.Character = _borderOptions.BottomRight;
    }

    public override void RenderTo(IRenderTarget target)
    {
        //Change Order. So Overflow Hidden works with the border.
        if (OverflowMode == OverflowMode.Hidden)
        {
            FillColors(target);
            RenderChildren(target);
            RenderSelf(target);
        }
        else
        {
            base.RenderTo(target);
        }
    }
}