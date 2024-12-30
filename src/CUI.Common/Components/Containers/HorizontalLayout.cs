using System.Linq;
using System.Numerics;

using CUI.Common.Drawing;

namespace CUI.Common.Components.Containers;

public class HorizontalLayout : Layout
{
    public HorizontalLayout(RenderColor foregroundColor = RenderColor.Inherit, RenderColor backgroundColor = RenderColor.Inherit) : base(foregroundColor, backgroundColor)
    {
    }


    protected override int SetItem(Renderable renderable, float weight, int currentPosition, float totalSize)
    {
        renderable.LayoutMode = LayoutMode.FillY;

        if (renderable is LayoutElement { FixedSize: true })
        {
            renderable.Transform.Position = new Vector2(currentPosition, 0);
            return currentPosition + (int)renderable.Transform.InnerSize.X;
        }

        renderable.Transform.Size = Transform.InnerSize with { X = totalSize * weight };
        renderable.Transform.Position = new Vector2(currentPosition, 0);
        return currentPosition + (int)renderable.Transform.InnerSize.X;
    }

    protected override int GetTotalFixedSize(Renderable[] children)
    {
        return (int)children.Where(x => x is LayoutElement elem && elem.FixedSize).Sum(x => (x.Transform.InnerSize.X+x.Transform.Padding.X));
    }
    protected override int GetTotalSize()
    {
        return (int)Transform.InnerSize.X;
    }
}