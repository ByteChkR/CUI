using System;

namespace CUI.Common
{
    [Flags]
    public enum RenderBufferClearFlags
    {
        /// <summary>No color is set.</summary>
        None,
        /// <summary>The color black.</summary>
        BackgroundBlack = 1 << 0,
        /// <summary>The color dark blue.</summary>
        BackgroundDarkBlue = 1 << 1,
        /// <summary>The color dark green.</summary>
        BackgroundDarkGreen = 1 << 2,
        /// <summary>The color dark cyan (dark blue-green).</summary>
        BackgroundDarkCyan = 1 << 3,
        /// <summary>The color dark red.</summary>
        BackgroundDarkRed = 1 << 4,
        /// <summary>The color dark magenta (dark purplish-red).</summary>
        BackgroundDarkMagenta = 1 << 5,
        /// <summary>The color dark yellow (ochre).</summary>
        BackgroundDarkYellow = 1 << 6,
        /// <summary>The color gray.</summary>
        BackgroundGray = 1 << 7,
        /// <summary>The color dark gray.</summary>
        BackgroundDarkGray = 1 << 8,
        /// <summary>The color blue.</summary>
        BackgroundBlue = 1 << 9,
        /// <summary>The color green.</summary>
        BackgroundGreen = 1 << 10,
        /// <summary>The color cyan (blue-green).</summary>
        BackgroundCyan = 1 << 11,
        /// <summary>The color red.</summary>
        BackgroundRed = 1 << 12,
        /// <summary>The color magenta (purplish-red).</summary>
        BackgroundMagenta = 1 << 13,
        /// <summary>The color yellow.</summary>
        BackgroundYellow = 1 << 14,
        /// <summary>The color white.</summary>
        BackgroundWhite = 1 << 15,
        
        /// <summary>The color black.</summary>
        ForegroundBlack = 1 << 16,
        /// <summary>The color dark blue.</summary>
        ForegroundDarkBlue = 1 << 17,
        /// <summary>The color dark green.</summary>
        ForegroundDarkGreen = 1 << 18,
        /// <summary>The color dark cyan (dark blue-green).</summary>
        ForegroundDarkCyan = 1 << 19,
        /// <summary>The color dark red.</summary>
        ForegroundDarkRed = 1 << 20,
        /// <summary>The color dark magenta (dark purplish-red).</summary>
        ForegroundDarkMagenta = 1 << 21,
        /// <summary>The color dark yellow (ochre).</summary>
        ForegroundDarkYellow = 1 << 22,
        /// <summary>The color gray.</summary>
        ForegroundGray = 1 << 23,
        /// <summary>The color dark gray.</summary>
        ForegroundDarkGray = 1 << 24,
        /// <summary>The color blue.</summary>
        ForegroundBlue = 1 << 25,
        /// <summary>The color green.</summary>
        ForegroundGreen = 1 << 26,
        /// <summary>The color cyan (blue-green).</summary>
        ForegroundCyan = 1 << 27,
        /// <summary>The color red.</summary>
        ForegroundRed = 1 << 28,
        /// <summary>The color magenta (purplish-red).</summary>
        ForegroundMagenta = 1 << 29,
        /// <summary>The color yellow.</summary>
        ForegroundYellow = 1 << 30,
        /// <summary>The color white.</summary>
        ForegroundWhite = 1 << 31,
    }
}