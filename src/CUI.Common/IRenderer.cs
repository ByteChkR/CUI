using System;
using System.Collections.Generic;
using System.Numerics;

using CUI.Common.Drawing;

namespace CUI.Common.Rendering;

public class BufferPixel : IRenderBufferPixel
{
    public static BufferPixel Empty => new BufferPixel();
    private char _character = ' ';
    private ConsoleColor _foregroundColor = ConsoleColor.White;
    private ConsoleColor _backgroundColor = ConsoleColor.Black;

    public char Character
    {
        get => _character;
        set => _character = value;
    }

    public ConsoleColor ForegroundColor { 
        get => _foregroundColor;
        set => _foregroundColor = value;
    }

    public ConsoleColor BackgroundColor
    {
        get => _backgroundColor; 
        set => _backgroundColor = value;
    }
    public void SetBackground(RenderColor color)
    {
        if (color == RenderColor.Inherit)
        {
            return;
        }
        BackgroundColor = (ConsoleColor)color;
    }
        
    public void SetForeground(RenderColor color)
    {
        if (color == RenderColor.Inherit)
        {
            return;
        }
        ForegroundColor = (ConsoleColor)color;
    }

    public bool Equals(IRenderBufferPixel other)
    {
        return _character == other.Character && _foregroundColor == other.ForegroundColor && _backgroundColor == other.BackgroundColor;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is IRenderBufferPixel other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_character, (int)_foregroundColor, (int)_backgroundColor);
    }

    public static bool operator ==(BufferPixel? left, IRenderBufferPixel? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(BufferPixel? left, IRenderBufferPixel? right)
    {
        return !Equals(left, right);
    }
    public static bool operator ==(IRenderBufferPixel? right, BufferPixel? left)
    {
        return Equals(left, right);
    }

    public static bool operator !=(IRenderBufferPixel? right, BufferPixel? left)
    {
        return !Equals(left, right);
    }
}
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