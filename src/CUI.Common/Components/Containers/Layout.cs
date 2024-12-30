using System.Collections.Generic;
using System.Linq;

using CUI.Common.Drawing;

namespace CUI.Common.Components.Containers;

public abstract class Layout : Renderable
{
        
    protected Layout(RenderColor foregroundColor = RenderColor.Inherit, RenderColor backgroundColor = RenderColor.Inherit) : base(foregroundColor, backgroundColor)
    {
    }

    protected abstract int SetItem(Renderable renderable, float weight, int currentPosition, float totalSize);
    protected abstract int GetTotalFixedSize(Renderable[] children);
    protected abstract int GetTotalSize();
    public override void ComputeLayout()
    {
        if(LayoutMode.HasFlag(LayoutMode.FillX) || LayoutMode.HasFlag(LayoutMode.FillY))
        {
            base.ComputeLayout();
        }
        Renderable[] children = GetChildren().ToArray();
        int currentPosition = 0;
        double totalWeight = children.Sum(x => x is LayoutElement elem ? elem.FixedSize ? 0 : elem.Weight : 1);
        float fixedSize = GetTotalFixedSize(children);
        float totalSize = GetTotalSize() - fixedSize;
        foreach (Renderable? child in children)
        {
            float weight = 1;
            if (child is LayoutElement elem)
            {
                if(elem.IgnoreLayout)
                {
                    continue;
                }
                weight = elem.Weight;
            }
            currentPosition = SetItem(child,totalWeight != 0 ? (float)(weight / totalWeight) : 1, currentPosition, totalSize);
            child.ComputeLayout();
        }
        if(LayoutMode.HasFlag(LayoutMode.FitX) || LayoutMode.HasFlag(LayoutMode.FitY))
        {
            base.ComputeLayout();
        }
    }
}