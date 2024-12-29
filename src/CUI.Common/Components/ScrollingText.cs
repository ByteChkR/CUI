using System.Linq;

using CUI.Common.Rendering;

namespace CUI.Common.Components
{
    public class ScrollingText : Text
    {
        private string CalculateText()
        {
            int maxLines = (int)Transform.InnerSize.Y;
            string[] lines = Value.Split('\n').Where(x=>!string.IsNullOrEmpty(x)).ToArray();
            if (lines.Length <= maxLines)
            {
                return Value;
            }
            return string.Join("\n", lines.Skip(lines.Length - maxLines));
        }
        protected override void RenderSelf(IRenderTarget target)
        {
            string text = CalculateText();
            RenderText(target, text);
        }
    }
}