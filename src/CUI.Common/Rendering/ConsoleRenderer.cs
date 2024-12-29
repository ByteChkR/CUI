using System;
using System.Numerics;

namespace CUI.Common.Components
{
    public class ConsoleRenderer : IRenderer
    {
        public RenderBuffer Buffer { get; }
        private RenderBuffer _displayBuffer;
        public ConsoleRenderer()
        {
            Buffer = new RenderBuffer(Console.WindowWidth, Console.WindowHeight, new RenderBufferInfo());
            _displayBuffer = new RenderBuffer(Console.WindowWidth, Console.WindowHeight, new RenderBufferInfo());
        }

        
        public void Render()
        {
            foreach (RenderCommand command in _displayBuffer.Update(Buffer))
            {
                Console.SetCursorPosition(command.X, command.Y);
                Console.BackgroundColor = command.BackgroundColor;
                Console.ForegroundColor = command.ForegroundColor;
                Console.Write(command.Characters);
                command.SetClean();
            }
            
            #if DEBUG
            //Ensure that the buffer and _displayBuffer are equal in all pixels
            for (int x = 0; x < Buffer.Size.X; x++)
            {
                for (int y = 0; y < Buffer.Size.Y; y++)
                {
                    RenderBufferPixel bufferPixel = Buffer.GetPixel(new Vector2(x, y));
                    RenderBufferPixel displayBufferPixel = _displayBuffer.GetPixel(new Vector2(x, y));
                    if (bufferPixel != displayBufferPixel)
                    {
                        throw new Exception("Buffer and display buffer are not equal");
                    }
                }
            }
            #endif
        }
    }
}