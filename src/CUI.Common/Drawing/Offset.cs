using System;
using System.Numerics;

namespace CUI.Common.Drawing;

public struct Offset : IEquatable<Offset>
{
    public Offset(int offset)
    {
        Left = offset;
        Top = offset;
        Right = offset;
        Bottom = offset;
    }
        
    public Offset(int x, int y)
    {
        Left = x;
        Top = y;
        Right = x;
        Bottom = y;
    }
        
    public Offset(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
    public int Left { get; set; }
    public int Top { get; set; }
    public int Right { get; set; }
    public int Bottom { get; set; }
    public Vector2 TopLeft => new Vector2(Left, Top);
    public Vector2 Size => new Vector2(Left + Right, Top + Bottom);
    public int X => Left + Right;
    public int Y => Top + Bottom;
        

    public bool Equals(Offset other)
    {
        return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
    }

    public override bool Equals(object? obj)
    {
        return obj is Offset other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Left, Top, Right, Bottom);
    }
        
    public static bool operator ==(Offset left, Offset right)
    {
        return left.Equals(right);
    }
        
    public static bool operator !=(Offset left, Offset right)
    {
        return !left.Equals(right);
    }
}