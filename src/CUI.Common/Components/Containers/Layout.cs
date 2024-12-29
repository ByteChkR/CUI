using System.Collections.Generic;
using System.Linq;

using CUI.Common.Drawing;

namespace CUI.Common.Components.Containers
{
    public abstract class Layout : Renderable
    {
        
        protected Layout(RenderColor foregroundColor = RenderColor.Inherit, RenderColor backgroundColor = RenderColor.Inherit) : base(foregroundColor, backgroundColor)
        {
        }

        protected abstract int SetItem(Renderable renderable, float weight, int currentPosition);
        public override void ComputeLayout()
        {
            if(LayoutMode.HasFlag(LayoutMode.FillX) || LayoutMode.HasFlag(LayoutMode.FillY))
            {
                base.ComputeLayout();
            }
            IEnumerable<Renderable> children = GetChildren().ToArray();
            int currentPosition = 0;
            float totalWeight = children.Sum(x => x is LayoutElement elem ? elem.Weight : 1);
            foreach (Renderable? child in children)
            {
                float weight = 1;
                if (child is LayoutElement elem)
                {
                    weight = elem.Weight;
                }
                currentPosition = SetItem(child, weight / totalWeight, currentPosition);
                child.ComputeLayout();
            }
            if(LayoutMode.HasFlag(LayoutMode.FitX) || LayoutMode.HasFlag(LayoutMode.FitY))
            {
                base.ComputeLayout();
            }
        }
    }
}