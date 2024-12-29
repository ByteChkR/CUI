using System;
using System.Numerics;

namespace CUI.Common
{
    public class RenderCommand
    {
        public RenderCommand(ConsoleColor foregroundColor, ConsoleColor backgroundColor, char[] characters, int x, int y, RenderBuffer buffer)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            Characters = characters;
            X = x;
            Y = y;
            _buffer = buffer;
        }
        private readonly RenderBuffer _buffer;
        public int X { get; }
        public int Y { get;  }
        public ConsoleColor ForegroundColor { get; }
        public ConsoleColor BackgroundColor { get; }
        public char[] Characters { get; }

        public void SetClean()
        {
            int width = Characters.Length;
            for (int i = 0; i < width; i++)
            {
                Vector2 position = new Vector2(X + i, Y);
                RenderBufferPixel pixel = _buffer.GetPixel(position);
                pixel.Character = Characters[i];
                pixel.ForegroundColor = ForegroundColor;
                pixel.BackgroundColor = BackgroundColor;
            }
        }
    }
}