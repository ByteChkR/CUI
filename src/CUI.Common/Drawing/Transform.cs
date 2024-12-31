using System.Drawing;
using System.Numerics;

using CUI.Common.Rendering;

namespace CUI.Common.Drawing;

public class Transform
{
    private bool _active = true;
    private Vector2 _position;
    private Vector2 _pivot;
    private Vector2 _innerSize;
    private Offset _padding;
    private int _zIndex;
    public bool Active
    {
        get => _active;
        set => _active = value;
    }
        
    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }
        
    public Vector2 Pivot
    {
        get => _pivot;
        set => _pivot = value;
    }
        
    public Vector2 InnerSize
    {
        get => _innerSize;
        set => _innerSize = value;
    }
        
    public Offset Padding
    {
        get => _padding;
        set => _padding = value;
    }

    public Vector2 Size
    {
        get => _innerSize + _padding.Size;
        set => _innerSize = value - _padding.Size;
    }
    public int ZIndex
    {
        get => _zIndex;
        set => _zIndex = value;
    }

    public Rectangle GetBounds(bool includePadding)
    {
        if (includePadding)
        {
            return new Rectangle((int)(Position.X - Pivot.X), (int)(Position.Y - Pivot.Y), (int)Size.X, (int)Size.Y);
        }
        return new Rectangle((int)(Position.X - Pivot.X) + Padding.Left, (int)(Position.Y - Pivot.Y) + Padding.Top, (int)InnerSize.X - Padding.Right, (int)InnerSize.Y - Padding.Bottom);
    }
        
    public bool Contains(Vector2 position, bool includePadding)
    {
        Vector2 thisPosition = Position - Pivot;
        Vector2 thisSize = Size;

        if (!includePadding)
        {
            thisPosition += new Vector2(Padding.Left, Padding.Top);
            thisSize -= new Vector2(Padding.Right, Padding.Bottom);
        }
        return position.X >= thisPosition.X && position.X < thisPosition.X + thisSize.X &&
               position.Y >= thisPosition.Y && position.Y < thisPosition.Y + thisSize.Y;
    }
    public bool Overlaps(Transform other, bool includePadding)
    {
        //Simple AABB collision detection
        Vector2 thisPosition = Position - Pivot;
        Vector2 thisSize = Size;
        Vector2 otherPosition = other.Position - other.Pivot;
        Vector2 otherSize = other.Size;
        if (!includePadding)
        {
            thisPosition += new Vector2(Padding.Left, Padding.Top);
            thisSize -= new Vector2(Padding.Right, Padding.Bottom);
            otherPosition += new Vector2(other.Padding.Left, other.Padding.Top);
            otherSize -= new Vector2(other.Padding.Right, other.Padding.Bottom);
        }
        return thisPosition.X < otherPosition.X + otherSize.X &&
               thisPosition.X + thisSize.X > otherPosition.X &&
               thisPosition.Y < otherPosition.Y + otherSize.Y &&
               thisPosition.Y + thisSize.Y > otherPosition.Y;
    }

}