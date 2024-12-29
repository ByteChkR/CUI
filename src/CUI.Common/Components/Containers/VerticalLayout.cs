using System.Numerics;

using CUI.Common.Drawing;

namespace CUI.Common.Components.Containers
{
    public class VerticalLayout : Layout
    {
        public VerticalLayout(RenderColor foregroundColor = RenderColor.Inherit, RenderColor backgroundColor = RenderColor.Inherit) : base(foregroundColor, backgroundColor)
        {
        }

        protected override int SetItem(Renderable renderable, float weight, int currentPosition)
        {
            renderable.LayoutMode = LayoutMode.FillX;
            renderable.Transform.Size = new Vector2(Transform.InnerSize.X, Transform.InnerSize.Y * weight);
            renderable.Transform.Position = new Vector2(0, currentPosition);
            return currentPosition + (int)renderable.Transform.InnerSize.Y;
        }
    }
}