using System;

namespace CUI.Common.Rendering.Buffer
{
    public class RenderBufferInfo
    {
        public RenderBufferInfo(RenderBufferClearFlags clearFlags = RenderBufferClearFlags.BackgroundBlack | RenderBufferClearFlags.ForegroundWhite, char clearCharacter = ' ')
        {
            ClearCharacter = clearCharacter;
            ClearFlags = clearFlags;
        }
        public RenderBufferClearFlags ClearFlags { get; }
        public char ClearCharacter { get; }

        public ConsoleColor GetClearBackgroundColor()
        {
            if(ClearFlags == RenderBufferClearFlags.None)
            {
                throw new InvalidOperationException("No color is set");
            }

            uint current = (uint)ClearFlags & 0xFFFF;
            int color = 0;
            for (int i = 0; i < 16; i++)
            {
                if ((current & (1 << i)) != 0)
                {
                    color = i;
                    break;
                }
            }
            
            return (ConsoleColor)color;
        }
        
        public ConsoleColor GetClearForegroundColor()
        {
            if(ClearFlags == RenderBufferClearFlags.None)
            {
                throw new InvalidOperationException("No color is set");
            }

            uint current = ((uint)ClearFlags & 0xFFFF0000) >> 16;
            int color = 0;
            for (int i = 0; i < 16; i++)
            {
                if ((current & (1 << i)) != 0)
                {
                    color = i;
                    break;
                }
            }
            
            return (ConsoleColor)color;
        }
    }
}