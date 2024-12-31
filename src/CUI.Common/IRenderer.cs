using System;
using System.Collections.Generic;
using System.Numerics;

using CUI.Common.Drawing;

namespace CUI.Common.Rendering;

public interface IRenderBufferPixel : IEquatable<IRenderBufferPixel>
{
    char Character { get; set; }
    ConsoleColor ForegroundColor { get; set; }
    ConsoleColor BackgroundColor { get; set; }
    void SetBackground(RenderColor color);
    void SetForeground(RenderColor color);
    
}


public interface IRenderBuffer : IRenderTarget
{
    IEnumerable<IRenderCommand> Update(IRenderBuffer buffer, bool force);
}
public interface IRenderCommand{}

public interface IRenderer
{
    IRenderBuffer Buffer { get; }
    void Render();
    void BeginRender();
    void EndRender();
    void SetInputFocus(Renderable element);
}