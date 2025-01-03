﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using CUI.Common.Drawing;
using CUI.Common.Input;
using CUI.Common.Rendering;

namespace CUI.Common;

public class Renderable
{
    private readonly List<Renderable> _renderables = new List<Renderable>();

    public Renderable() { }

    public string Name { get; set; } = string.Empty;

    public Transform Transform { get; } = new Transform();
    public LayoutMode LayoutMode { get; set; } = LayoutMode.None;
    public OverflowMode OverflowMode { get; set; } = OverflowMode.Visible;
    public Renderable? Parent { get; internal set; }
    public bool Focusable { get; set; }
    public int TabIndex { get; set; }
    public bool Focused { get; internal set; }
    public bool CaptureControlKeys { get; set; }
    public virtual Vector2 InputFocus { get; }

    public RenderColor ForegroundColor { get; set; } = RenderColor.Inherit;

    public RenderColor BackgroundColor { get; set; } = RenderColor.Inherit;

    public RenderColor FocusForegroundColor { get; set; } = RenderColor.Inherit;
    public RenderColor FocusBackgroundColor { get; set; } = RenderColor.Inherit;

    public Vector2 GetParentPosition(Vector2 position)
    {
        return position + (Transform.Position - Transform.Pivot) + Transform.Padding.TopLeft;
    }

    public Vector2 GetRootPosition(Vector2 position)
    {
        if (Parent != null)
        {
            return Parent.GetRootPosition(GetParentPosition(position));
        }

        return GetParentPosition(position);
    }

    public event Action<Renderable> OnChanged = delegate { };

    public void SendChangedEvent(Renderable child)
    {
        OnChanged(child);
        Parent?.SendChangedEvent(child);
    }

    public event Action<InputHandlerEventArgs> OnKeyPress = delegate { };

    public void AddChild(Renderable renderable)
    {
        renderable.Parent = this;
        _renderables.Add(renderable);
    }

    public IEnumerable<Renderable> GetAllChildrenAndSelf()
    {
        yield return this;

        foreach (Renderable renderable in GetAllChildren())
        {
            yield return renderable;
        }
    }

    public IEnumerable<Renderable> GetAllChildren()
    {
        foreach (Renderable renderable in _renderables)
        {
            foreach (Renderable child in renderable.GetAllChildrenAndSelf())
            {
                yield return child;
            }
        }
    }

    public void RemoveChild(Renderable renderable)
    {
        renderable.Parent = null;
        _renderables.Remove(renderable);
    }

    public IEnumerable<Renderable> GetChildren(bool includeInactive = false)
    {
        IOrderedEnumerable<Renderable> list = _renderables.OrderBy(x => x.Transform.ZIndex);

        return includeInactive ? list : list.Where(x => x.Transform.Active);
    }

    public void ClearChildren()
    {
        _renderables.Clear();
    }

    public void HandleInput(ConsoleKeyInfo key)
    {
        InputHandlerEventArgs args = new InputHandlerEventArgs(key);
        OnKeyPress(args);

        if (args.Handled)
        {
            return;
        }

        HandleKeyPress(key);
    }

    protected virtual void HandleKeyPress(ConsoleKeyInfo key) { }

    protected virtual void OnComputeLayout() { }

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

        Renderable[] children = GetChildren()
            .ToArray();

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

    protected void FillColors(IRenderTarget target)
    {
        IRenderTarget t = target.IgnorePadding(true);

        for (int x = 0; x < t.Size.X; x++)
        {
            for (int y = 0; y < t.Size.Y; y++)
            {
                IRenderBufferPixel pixel = t.GetPixel(new Vector2(x, y));
                pixel.Character = ' ';
                pixel.SetBackground(BackgroundColor);
            }
        }
    }

    protected virtual void RenderSelf(IRenderTarget target) { }

    protected virtual void RenderChildren(IRenderTarget target)
    {
        Renderable[] children = GetChildren()
            .ToArray();

        for (int i = 0; i < children.Length; i++)
        {
            Renderable child = children[i];
            IRenderTarget t = target.CreateTarget(child.Transform, OverflowMode);
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