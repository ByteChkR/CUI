using System;

using CUI.Common.Drawing;

namespace CUI.Common.Rendering.Buffer;

public class RenderBufferPixel : IEquatable<RenderBufferPixel>
{
    public static RenderBufferPixel Empty => new RenderBufferPixel();
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

    public bool Equals(RenderBufferPixel? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return _character == other._character && _foregroundColor == other._foregroundColor && _backgroundColor == other._backgroundColor;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((RenderBufferPixel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_character, (int)_foregroundColor, (int)_backgroundColor);
    }

    public static bool operator ==(RenderBufferPixel? left, RenderBufferPixel? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(RenderBufferPixel? left, RenderBufferPixel? right)
    {
        return !Equals(left, right);
    }
}