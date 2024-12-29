using System.Numerics;

using CUI.Common.Drawing;

namespace CUI.Common.Components.Containers
{
    public class HorizontalLayout : Layout
    {
        public HorizontalLayout(RenderColor foregroundColor = RenderColor.Inherit, RenderColor backgroundColor = RenderColor.Inherit) : base(foregroundColor, backgroundColor)
        {
        }


        protected override int SetItem(Renderable renderable, float weight, int currentPosition)
        {
            renderable.LayoutMode = LayoutMode.FillY;
            renderable.Transform.Size = new Vector2(Transform.InnerSize.X * weight, Transform.InnerSize.Y);
            renderable.Transform.Position = new Vector2(currentPosition, 0);
            return currentPosition + (int)renderable.Transform.InnerSize.X;
        }
    }
}