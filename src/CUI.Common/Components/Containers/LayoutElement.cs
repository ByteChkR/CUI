using CUI.Common.Drawing;

namespace CUI.Common.Components.Containers
{
    public class LayoutElement : Renderable
    {
        public float Weight { get; set; }
        public LayoutElement(Renderable child, float weight = 1)
        {
            AddChild(child);
            Weight = weight;
        }

        protected override void OnComputeLayout()
        {
            foreach (Renderable child in GetChildren())
            {
                child.LayoutMode = LayoutMode.Fill;
            }
            base.OnComputeLayout();
        }
    }
}