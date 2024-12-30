using System;
using System.Collections.Generic;
using System.Linq;

using CUI.Common.Input;
using CUI.Common.Rendering;

namespace CUI.Common;

public class RenderContext : IDisposable
{
    public RenderContext(IRenderer renderer, bool interactive = false)
    {
        Renderer = renderer;
        InputHandler = interactive ? new InputHandler(this) : null;
    }
    public IRenderer Renderer { get; }
    public InputHandler? InputHandler { get; }
    public Renderable Root { get; set; } = new Renderable();

    private bool _isRendering;
    public IEnumerable<Renderable> FocusableElements => Root.GetAllChildrenAndSelf()
                                                            .Where(x => x.Focusable)
                                                            .OrderBy(x => x.TabIndex);

    public void Render()
    {
        if (_isRendering)
        {
            return;
        }

        _isRendering = true;
        InputHandler?.Activate();
        Renderer.BeginRender();
        Root
            .WithSize(Renderer.Buffer.Size)
            .ComputeLayout();
        Renderer.Buffer.Clear();
        Root.RenderTo(Renderer.Buffer);
        Renderer.Render();
        Renderer.EndRender();
        Renderable? focus = InputHandler?.FocusElement;
        if(focus != null)
        {
            Renderer.SetInputFocus(focus);
        }
        _isRendering = false;
    }

    public void Dispose()
    {
        InputHandler?.Dispose();
    }
}