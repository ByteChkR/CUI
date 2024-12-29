using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using CUI.Common.Drawing;
using CUI.Common.Rendering;
using CUI.Common.Rendering.Buffer;

namespace CUI.Common
{
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
        public virtual void ComputeLayout()
        {
            
            //Fill X : Set width of this to parent width
            if (LayoutMode.HasFlag(LayoutMode.FillX))
            {
                if (Parent != null)
                {
                    Transform.Size = new Vector2(Parent.Transform.InnerSize.X, Transform.Size.Y);
                }
            }
            
            //Fill Y : Set height of this to parent height
            if (LayoutMode.HasFlag(LayoutMode.FillY))
            {
                if (Parent != null)
                {
                    Transform.Size = new Vector2(Transform.Size.X, Parent.Transform.InnerSize.Y);
                }
            }
            
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
            
            // Fit Y : Compute Height of Children
            if (children.Length > 0 && LayoutMode.HasFlag(LayoutMode.FitY))
            {
                float height = children.Max(x => x.Transform.Position.Y + x.Transform.Size.Y);
                Transform.InnerSize = new Vector2(Transform.InnerSize.X, height);
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

        protected virtual void RenderChildren(IRenderTarget target)
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