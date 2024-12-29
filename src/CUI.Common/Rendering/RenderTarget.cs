using System.Drawing;
using System.Numerics;

namespace CUI.Common
{
    public class RenderTarget : IRenderTarget
    {
        public RenderTarget(Transform transform, IRenderTarget parent, bool includePadding, OverflowMode overflowMode)
        {
            Transform = transform;
            Parent = parent;
            IncludePadding = includePadding;
            OverflowMode = overflowMode;
        }
        public bool IncludePadding { get; }
        public Transform Transform { get; }
        public IRenderTarget Parent { get; }
        public OverflowMode OverflowMode { get; }
        public Vector2 Size => IncludePadding ? Transform.Size : Transform.InnerSize;

        private Vector2 CalculatePosition(Vector2 position)
        {
            return position + (Transform.Position - Transform.Pivot) + (IncludePadding ? Vector2.Zero : Transform.Padding.TopLeft);
        }
        public RenderBufferPixel GetPixel(Vector2 position)
        {

            if (OverflowMode == OverflowMode.Hidden && !Contains(position))
            {
                return RenderBufferPixel.Empty;
            }
            Vector2 pos = CalculatePosition(position);
            return Parent.GetPixel(pos);
        }

        public void Clear()
        {
            Parent.Clear(Transform.GetBounds(IncludePadding));
        }
        
        public void Clear(Rectangle area)
        {
            Vector2 pos = new Vector2(area.Left, area.Top) + (Transform.Position - Transform.Pivot) + (IncludePadding ? Vector2.Zero : Transform.Padding.TopLeft);
            Rectangle newArea = new Rectangle((int)pos.X, (int)pos.Y, area.Width - (IncludePadding ? 0 : Transform.Padding.Right), area.Height - (IncludePadding ? 0 : Transform.Padding.Bottom));
            Parent.Clear(newArea);
        }

        public IRenderTarget IgnorePadding(bool ignore)
        {
            if(IncludePadding == ignore)
            {
                return this;
            }
            return new RenderTarget(Transform, Parent, ignore, OverflowMode);
        }

        public bool Contains(Vector2 position)
        {
            Vector2 p = CalculatePosition(position);
            bool transformContains = Transform.Contains(p, IncludePadding);
            bool parentContains = Parent.Contains(p);
            return transformContains && parentContains;
        }
    }
}