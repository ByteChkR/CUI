using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

using CUI.Common.Drawing;

namespace CUI.Common.Rendering.Buffer;

public class RenderBuffer : IRenderBuffer
{
    private BufferPixel[,] _buffer;
    private readonly RenderBufferInfo _info;

    private Vector2 _size;
    public Vector2 Size
    {
        get => _size;
        set
        {
            if(value != _size)
            {
                _size = value;
                ResizeBuffer();
            }
        }
    }
    
    public void Clear()
    {
        if (_info.ClearFlags == RenderBufferClearFlags.None)
        {
            return;
        }

        int width = (int)_size.X;
        int height = (int)_size.Y;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _buffer[x, y] = new BufferPixel()
                {
                    Character = _info.ClearCharacter,
                    BackgroundColor = _info.GetClearBackgroundColor(),
                    ForegroundColor = _info.GetClearForegroundColor(),
                };
            }
        }
    }

    public void Clear(Rectangle area)
    {
        if (_info.ClearFlags == RenderBufferClearFlags.None)
        {
            return;
        }

        int width = (int)_size.X;
        int height = (int)_size.Y;
        for (int x = area.Left; x < area.Right; x++)
        {
            for (int y = area.Top; y < area.Bottom; y++)
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    continue;
                }
                _buffer[x, y] = new BufferPixel()
                {
                    Character = _info.ClearCharacter,
                    BackgroundColor = _info.GetClearBackgroundColor(),
                    ForegroundColor = _info.GetClearForegroundColor(),
                };
            }
        }
    }

    public IRenderTarget IgnorePadding(bool ignore)
    {
        return this;
    }

    public bool Contains(Vector2 position)
    {
        return position.X >= 0 && position.X < _size.X && position.Y >= 0 && position.Y < _size.Y;
    }

    public IRenderTarget CreateTarget(Transform transform, OverflowMode mode)
    {
        return new RenderTarget(transform, this, false, mode);
    }

    public void ClearPixel(Vector2 position)
    {
        if(_info.ClearFlags == RenderBufferClearFlags.None)
        {
            return;
        }
        IRenderBufferPixel px = GetPixel(position);
        px.Character = _info.ClearCharacter;
        px.BackgroundColor = _info.GetClearBackgroundColor();
        px.ForegroundColor = _info.GetClearForegroundColor();
    }

    public IRenderBufferPixel GetPixel(Vector2 position)
    {
        int x = (int)position.X;
        int y = (int)position.Y;
        if (x < 0 || x >= _size.X || y < 0 || y >= _size.Y)
        {
            return BufferPixel.Empty;
        }
        return _buffer[x, y];
    }

    public void ResizeBuffer()
    {
        int width = (int)_size.X;
        int height = (int)_size.Y;
        _buffer = new BufferPixel[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _buffer[x, y] = new BufferPixel()
                {
                    Character = _info.ClearCharacter,
                    ForegroundColor = _info.GetClearForegroundColor(),
                    BackgroundColor = _info.GetClearBackgroundColor(),
                };
            }
        }
    }

    public RenderBuffer(Vector2 size, RenderBufferInfo info)
    {
        _size = size;
        _info = info;
        ResizeBuffer();
    }
    public RenderBuffer(int width, int height, RenderBufferInfo info) : this(new Vector2(width, height), info)
    {
    }

    public IEnumerable<IRenderCommand> Update(IRenderBuffer buffer, bool force)
    {
        // Create a list of commands to update the current buffer based on the changes in the new buffer.
        if (buffer.Size != Size)
        {
            throw new InvalidOperationException("Buffer sizes do not match");
        }

        int width = (int)Size.X;
        int height = (int)Size.Y;
        // Create a list of commands to render the buffer.
        //  => a command is a sequence of characters with the same foreground and background color
            
        for (int y = 0; y < height; y++)
        {
            //A Render Command can be at most the width of the buffer
            ConsoleColor currentForegroundColor = ConsoleColor.White;
            ConsoleColor currentBackgroundColor = ConsoleColor.Black;
            List<char> currentCharacters = new List<char>();
            int startX = 0;
            for (int x = 0; x < width; x++)
            {
                IRenderBufferPixel oldPixel = GetPixel(new Vector2(x, y));
                IRenderBufferPixel pixel = buffer.GetPixel(new Vector2(x, y));

                // If the pixel is not dirty, skip it
                if (!force && oldPixel == pixel)
                {
                    // If there are characters in the current command, add it to the list of commands
                    if (currentCharacters.Count > 0)
                    {
                        yield return new RenderCommand(currentForegroundColor, currentBackgroundColor, currentCharacters.ToArray(), startX, y, this);
                        currentCharacters = new List<char>();
                    }
                    continue;
                }
                // If the pixel has different colors than the current command, start a new command
                if (pixel.ForegroundColor != currentForegroundColor || pixel.BackgroundColor != currentBackgroundColor)
                {
                    // If there are characters in the current command, add it to the list of commands
                    if (currentCharacters.Count > 0)
                    {
                        yield return new RenderCommand(currentForegroundColor, currentBackgroundColor, currentCharacters.ToArray(), startX, y, this);
                        currentCharacters = new List<char>();
                    }
                        
                    // Update the current colors
                    currentForegroundColor = pixel.ForegroundColor;
                    currentBackgroundColor = pixel.BackgroundColor;
                }
                    
                // If the current pixel is the first in the command, update the start position
                if(currentCharacters.Count == 0)
                {
                    startX = x;
                }
                currentCharacters.Add(pixel.Character);
            }
                
            // If there are characters in the current command, add it to the list of commands
            if (currentCharacters.Count > 0)
            {
                yield return new RenderCommand(currentForegroundColor, currentBackgroundColor, currentCharacters.ToArray(), startX, y, this);
            }
        }
    }

}