using System;
using System.Numerics;

using CUI.Common.Rendering.Buffer;

namespace CUI.Common.Rendering;

public class ConsoleRenderer : IRenderer
{
    public IRenderBuffer Buffer { get; }
    private IRenderBuffer _displayBuffer;
    private bool _forceRender = true;
    public ConsoleRenderer()
    {
        Buffer = new RenderBuffer(Console.WindowWidth, Console.WindowHeight, new RenderBufferInfo());
        _displayBuffer = new RenderBuffer(Console.WindowWidth, Console.WindowHeight, new RenderBufferInfo());
    }
    
    public void Render()
    {
        foreach (RenderCommand command in _displayBuffer.Update(Buffer, _forceRender))
        {
            Console.SetCursorPosition(command.X, command.Y);
            Console.BackgroundColor = command.BackgroundColor;
            Console.ForegroundColor = command.ForegroundColor;
            Console.Write(command.Characters);
            command.SetClean();
        }
        Console.SetCursorPosition(0,0);
            
#if DEBUG
        //Ensure that the buffer and _displayBuffer are equal in all pixels
        for (int x = 0; x < Buffer.Size.X; x++)
        {
            for (int y = 0; y < Buffer.Size.Y; y++)
            {
                IRenderBufferPixel bufferPixel = Buffer.GetPixel(new Vector2(x, y));
                IRenderBufferPixel displayBufferPixel = _displayBuffer.GetPixel(new Vector2(x, y));
                if (!bufferPixel.Equals(displayBufferPixel))
                {
                    throw new Exception("Buffer and display buffer are not equal");
                }
            }
        }
#endif
    }

    public void BeginRender()
    {
        Vector2 consoleSize = new Vector2(Console.WindowWidth, Console.WindowHeight);
        if (Buffer.Size != consoleSize)
        {
            Buffer.Size = consoleSize;
            _displayBuffer.Size = consoleSize;
            _forceRender = true;
        }
    }
    public void EndRender()
    {
        _forceRender = false;
    }
    
    public void SetInputFocus(Renderable element)
    {
        Vector2 position = element.GetRootPosition(element.InputFocus);
        Console.SetCursorPosition((int)position.X, (int)position.Y);
    }
}