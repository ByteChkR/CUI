using System;
using System.Numerics;

namespace CUI.Common.Components
{
    public class Text : Renderable
    {
        private string _text = string.Empty;
        
        public string Value
        {
            get => _text;
            set => _text = value;
        }
        

        public Text(RenderColor foregroundColor = RenderColor.Inherit, RenderColor backgroundColor = RenderColor.Inherit) : base(foregroundColor, backgroundColor)
        {
        }
        public Text(string text, RenderColor foregroundColor = RenderColor.Inherit, RenderColor backgroundColor = RenderColor.Inherit) : this(foregroundColor, backgroundColor)
        {
            _text = text;
        }

        protected override void OnComputeLayout()
        {
            base.OnComputeLayout();
            if (LayoutMode.HasFlag(LayoutMode.FitX))
            {
                RecalculateBounds();
            }
        }

        private void RecalculateBounds()
        {
            Transform.InnerSize = new Vector2(_text.Length, 1);
        }

        protected override void RenderSelf(IRenderTarget target)
        {
            for (int i = 0; i < _text.Length; i++)
            {
                RenderBufferPixel pixel = target.GetPixel(new Vector2(i, 0));
                pixel.Character = _text[i];
                pixel.SetForeground(ForegroundColor);
                pixel.SetBackground(BackgroundColor);
            }
        }

    }
}