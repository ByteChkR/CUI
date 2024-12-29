﻿namespace CUI.Common
{
    public class RenderContext
    {
        public RenderContext(IRenderer renderer)
        {
            Renderer = renderer;
        }
        public IRenderer Renderer { get; }
        public Renderable Elements { get; } = new Renderable();

        public void Render()
        {
            Elements.ComputeLayout();
            Renderer.Buffer.Clear();
            Elements.RenderTo(Renderer.Buffer);
            Renderer.Render();
        }
    }
}