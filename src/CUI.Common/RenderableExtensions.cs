using System.Linq;
using System.Numerics;

using CUI.Common.Components;
using CUI.Common.Components.Containers;
using CUI.Common.Drawing;

namespace CUI.Common;

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
        
    public static T WithName<T>(this T renderable, string name) where T : Renderable
    {
        renderable.Name = name;
        return renderable;
    }

    public static Renderable? Find(this Renderable renderable, string name) => 
        renderable
            .GetAllChildrenAndSelf()
            .FirstOrDefault(x => x.Name == name);
        
    public static T? Find<T>(this Renderable renderable, string name) where T : Renderable => 
        renderable
            .GetAllChildrenAndSelf()
            .FirstOrDefault(x => x.Name == name) as T;
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
    
    public static T WithTabIndex<T>(this T renderable, int tabIndex) where T : Renderable
    {
        renderable.TabIndex = tabIndex;
        return renderable;
    }
    
    public static T WithFocusable<T>(this T renderable, bool focusable) where T : Renderable
    {
        renderable.Focusable = focusable;
        return renderable;
    }
    
    public static T WithCaptureControlKeys<T>(this T renderable, bool captureControlKeys) where T : Renderable
    {
        renderable.CaptureControlKeys = captureControlKeys;
        return renderable;
    }
    
    public static LayoutElement AsLayoutElement(this Renderable renderable, bool ignoreLayout)
    {
        return new LayoutElement(renderable, 0, ignoreLayout);
    }
        
    public static LayoutElement AsLayoutElement(this Renderable renderable, float weight)
    {
        return new LayoutElement(renderable, weight);
    }

}