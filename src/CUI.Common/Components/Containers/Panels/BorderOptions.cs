using System;

namespace CUI.Common.Components.Containers.Panels
{
    public struct BorderOptions : IEquatable<BorderOptions>
    {
        public char TopLeft;
        public char TopRight;
        public char BottomLeft;
        public char BottomRight;
        public char Horizontal;
        public char Vertical;
        public static BorderOptions Default = new BorderOptions
        {
            TopLeft = '┌',
            TopRight = '┐',
            BottomLeft = '└',
            BottomRight = '┘',
            Horizontal = '─',
            Vertical = '│',
        };
        public static BorderOptions Double = new BorderOptions
        {
            TopLeft = '╔',
            TopRight = '╗',
            BottomLeft = '╚',
            BottomRight = '╝',
            Horizontal = '═',
            Vertical = '║',
        };

        public bool Equals(BorderOptions other)
        {
            return TopLeft == other.TopLeft && TopRight == other.TopRight && BottomLeft == other.BottomLeft && BottomRight == other.BottomRight && Horizontal == other.Horizontal && Vertical == other.Vertical;
        }

        public override bool Equals(object? obj)
        {
            return obj is BorderOptions other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TopLeft, TopRight, BottomLeft, BottomRight, Horizontal, Vertical);
        }
        
        public static bool operator ==(BorderOptions left, BorderOptions right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(BorderOptions left, BorderOptions right)
        {
            return !left.Equals(right);
        }
    }
}