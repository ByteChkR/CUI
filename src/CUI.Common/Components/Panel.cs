using System.Linq;

namespace CUI.Common.Components
{
    public class Panel : Renderable
    {
        public Panel(RenderColor foregroundColor = RenderColor.Inherit, RenderColor backgroundColor = RenderColor.Inherit) : base(foregroundColor, backgroundColor)
        {
            
        }
    }
}