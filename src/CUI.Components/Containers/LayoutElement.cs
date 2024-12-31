using CUI.Common.Drawing;

namespace CUI.Common.Components.Containers;

public class LayoutElement : Renderable
{
    public float Weight { get; set; }
    public bool IgnoreLayout { get; set; }
    public bool FixedSize { get; set; }
    public LayoutElement(Renderable child, float weight = 1, bool ignoreLayout = false)
    {
        AddChild(child);
        Weight = weight;
        IgnoreLayout = ignoreLayout;
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