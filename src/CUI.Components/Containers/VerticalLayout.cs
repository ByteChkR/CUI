using System;
using System.Linq;
using System.Numerics;

using CUI.Common.Drawing;

namespace CUI.Common.Components.Containers;

public class VerticalLayout : Layout
{

    protected override int SetItem(Renderable renderable, float weight, int currentPosition, float totalSize)
    {
        renderable.LayoutMode = LayoutMode.FillX;

        if (renderable is LayoutElement { FixedSize: true })
        {
            renderable.Transform.Position = new Vector2(0, currentPosition);
            return currentPosition + (int)renderable.Transform.InnerSize.Y;
        }
        renderable.Transform.Size = Transform.InnerSize with { Y = MathF.Floor(totalSize * weight) };
        renderable.Transform.Position = new Vector2(0, currentPosition);
        return currentPosition + (int)renderable.Transform.InnerSize.Y;
    }
    protected override int GetTotalFixedSize(Renderable[] children)
    {
        return (int)children.Where(x => x is LayoutElement elem && elem.FixedSize).Sum(x => (x.Transform.InnerSize.Y+x.Transform.Padding.Y));
    }
    protected override int GetTotalSize()
    {
        return (int)Transform.InnerSize.Y;
    }
}