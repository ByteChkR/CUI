using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CUI.Common
{
    public static class RenderableExtensions
    {
        public static void AddChildren(this Renderable renderable, params Renderable[] children)
        {
            foreach (Renderable child in children)
            {
                renderable.AddChild(child);
            }
        }
        
        public static void RemoveChildren(this Renderable renderable, params Renderable[] children)
        {
            foreach (Renderable child in children)
            {
                renderable.RemoveChild(child);
            }
        }
        
        public static T WithChildren<T>(this T renderable, params Renderable[] children) where T : Renderable
        {
            renderable.AddChildren(children);
            return renderable;
        }
        
        public static T WithChild<T>(this T renderable, Renderable child) where T : Renderable
        {
            renderable.AddChild(child);
            return renderable;
        }
        
        public static T WithLayout<T>(this T renderable, LayoutMode layoutMode) where T : Renderable
        {
            renderable.LayoutMode = layoutMode;
            return renderable;
        }
        
        public static T WithOverflow<T>(this T renderable, OverflowMode overflowMode) where T : Renderable
        {
            renderable.OverflowMode = overflowMode;
            return renderable;
        }
        
        public static T AtPosition<T>(this T renderable, Vector2 position) where T : Renderable
        {
            renderable.Transform.Position = position;
            return renderable;
        }
        
        public static T AtPosition<T>(this T renderable, float x, float y) where T : Renderable
        {
            renderable.Transform.Position = new Vector2(x, y);
            return renderable;
        }
        
        public static T WithPadding<T>(this T renderable, Offset padding) where T : Renderable
        {
            renderable.Transform.Padding = padding;
            return renderable;
        }
        public static T WithPadding<T>(this T renderable, int left, int top, int right, int bottom) where T : Renderable
        {
            renderable.Transform.Padding = new Offset(left, top, right, bottom);
            return renderable;
        }
        public static T WithPadding<T>(this T renderable, int x, int y) where T : Renderable
        {
            renderable.Transform.Padding = new Offset(x, y);
            return renderable;
        }
        public static T WithPadding<T>(this T renderable, int padding) where T : Renderable
        {
            renderable.Transform.Padding = new Offset(padding);
            return renderable;
        }
        
        public static T WithZIndex<T>(this T renderable, int zIndex) where T : Renderable
        {
            renderable.Transform.ZIndex = zIndex;
            return renderable;
        }
        
        public static T WithForegroundColor<T>(this T renderable, RenderColor color) where T : Renderable
        {
            renderable.ForegroundColor = color;
            return renderable;
        }
        
        public static T WithBackgroundColor<T>(this T renderable, RenderColor color) where T : Renderable
        {
            renderable.BackgroundColor = color;
            return renderable;
        }
        
        public static T WithActive<T>(this T renderable, bool active) where T : Renderable
        {
            renderable.Transform.Active = active;
            return renderable;
        }
        
        public static T WithSize<T>(this T renderable, Vector2 size) where T : Renderable
        {
            renderable.Transform.InnerSize = size;
            return renderable;
        }

        public static T WithSize<T>(this T renderable, int width, int height) where T : Renderable
        {
            renderable.Transform.InnerSize = new Vector2(width, height);
            return renderable;
        }
        
        public static T WithPivot<T>(this T renderable, Vector2 pivot) where T : Renderable
        {
            renderable.Transform.Pivot = pivot;
            return renderable;
        }
        
        public static T WithWidth<T>(this T renderable, float width) where T : Renderable
        {
            renderable.Transform.InnerSize = new Vector2(width, renderable.Transform.InnerSize.Y);
            return renderable;
        }
        
        public static T WithHeight<T>(this T renderable, float height) where T : Renderable
        {
            renderable.Transform.InnerSize = new Vector2(renderable.Transform.InnerSize.X, height);
            return renderable;
        }
        
        
        public static T WithColors<T>(this T renderable, RenderColor foregroundColor, RenderColor backgroundColor) where T : Renderable
        {
            renderable.ForegroundColor = foregroundColor;
            renderable.BackgroundColor = backgroundColor;
            return renderable;
        }

    }
    
    public class Renderable
    {
        private readonly List<Renderable> _renderables = new List<Renderable>();
        public void AddChild(Renderable renderable)
        {
            renderable.Parent = this;
            _renderables.Add(renderable);
        }
        
        public void RemoveChild(Renderable renderable)
        {
            renderable.Parent = null;
            _renderables.Remove(renderable);
        }

        public Renderable(RenderColor foregroundColor, RenderColor backgroundColor)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
        public Renderable()
        {
            
        }
        public IEnumerable<Renderable> GetChildren(bool includeInactive = false)
        {
            IOrderedEnumerable<Renderable> list = _renderables.OrderBy(x => x.Transform.ZIndex);
            return includeInactive ? list : _renderables.Where(x => x.Transform.Active);
        }
        
        public void ClearChildren()
        {
            _renderables.Clear();
        }

        public Transform Transform { get; } = new Transform();
        public LayoutMode LayoutMode { get; set; } = LayoutMode.None;
        public OverflowMode OverflowMode { get; set; } = OverflowMode.Visible;
        public Renderable? Parent { get; internal set; }

        protected virtual void OnComputeLayout()
        {
        }
        public void ComputeLayout()
        {
            Renderable[] children = GetChildren().ToArray();
            foreach (Renderable child in children)
            {
                child.ComputeLayout();
            }

            // Fit X : Compute Width of Children
            if (children.Length > 0 && LayoutMode.HasFlag(LayoutMode.FitX))
            {
                float width = children.Max(x => x.Transform.Position.X + x.Transform.Size.X);
                Transform.InnerSize = new Vector2(width, Transform.InnerSize.Y);
            }
            //Fill X : Set width of this to parent width
            else if (LayoutMode.HasFlag(LayoutMode.FillX))
            {
                if (Parent != null)
                {
                    Transform.InnerSize = new Vector2(Parent.Transform.InnerSize.X - Parent.Transform.Padding.X, Transform.InnerSize.Y);
                }
            }
            
            // Fit Y : Compute Height of Children
            if (children.Length > 0 && LayoutMode.HasFlag(LayoutMode.FitY))
            {
                float height = children.Max(x => x.Transform.Position.Y + x.Transform.Size.Y);
                Transform.InnerSize = new Vector2(Transform.InnerSize.X, height);
            }
            //Fill Y : Set height of this to parent height
            else if (LayoutMode.HasFlag(LayoutMode.FillY))
            {
                if (Parent != null)
                {
                    Transform.InnerSize = new Vector2(Transform.InnerSize.X, Parent.Transform.InnerSize.Y - Parent.Transform.Padding.Y);
                }
            }
            
            OnComputeLayout();
        }
        private RenderColor _foregroundColor = RenderColor.Inherit;
        private RenderColor _backgroundColor = RenderColor.Inherit;
        public RenderColor ForegroundColor
        {
            get => _foregroundColor;
            set => _foregroundColor = value;
        }
        
        public RenderColor BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }
        protected void FillColors(IRenderTarget target)
        {
            IRenderTarget t = target.IgnorePadding(true);
            for (int x = 0; x < t.Size.X; x++)
            {
                for (int y = 0; y < t.Size.Y; y++)
                {
                    RenderBufferPixel pixel = t.GetPixel(new Vector2(x, y));
                    pixel.Character = ' ';
                    pixel.SetBackground(BackgroundColor);
                }
            }
        }

        protected virtual void RenderSelf(IRenderTarget target)
        {
        }

        protected void RenderChildren(IRenderTarget target)
        {
            Renderable[] children = GetChildren().ToArray();
            for (int i = 0; i < children.Length; i++)
            {
                Renderable child = children[i];
                IRenderTarget t = child.Transform.CreateTarget(target, OverflowMode);
                child.RenderTo(t);
            }
        }
        
        public virtual void RenderTo(IRenderTarget target)
        {
            FillColors(target);
            RenderSelf(target);
            RenderChildren(target);
        }
    }
}