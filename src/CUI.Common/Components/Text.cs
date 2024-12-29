using System;
using System.Linq;
using System.Numerics;

using CUI.Common.Drawing;
using CUI.Common.Rendering;
using CUI.Common.Rendering.Buffer;

namespace CUI.Common.Components
{
    public class Text : Renderable
    {
        private string _text = string.Empty;
        
        public string Value
        {
            get => _text;
            set => _text = FormatText(value);
        }
        
        private string FormatText(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\t", "    ");
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
                RecalculateWidth();
            }
            if(LayoutMode.HasFlag(LayoutMode.FitY))
            {
                RecalculateHeight();
            }
        }

        private void RecalculateWidth()
        {
            Transform.InnerSize = new Vector2(_text.Split('\n').Max(x=>x.Length), Transform.InnerSize.Y);
        }
        
        private void RecalculateHeight()
        {
            Transform.InnerSize = new Vector2(Transform.InnerSize.X, _text.Count(x=> x == '\n') + 1);
        }

        protected void RenderText(IRenderTarget target, string text)
        {
            int y = 0;
            int x = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    y++;
                    x = 0;
                    continue;
                }
                RenderBufferPixel pixel = target.GetPixel(new Vector2(x, y));
                pixel.Character = text[i];
                pixel.SetForeground(ForegroundColor);
                pixel.SetBackground(BackgroundColor);
                x++;
            }
        }
        
        protected override void RenderSelf(IRenderTarget target)
        {
            RenderText(target, _text);
        }

    }
}