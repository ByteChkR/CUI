using System;

namespace CUI.Common.Drawing
{
    [Flags]
    public enum LayoutMode
    {
        FitX = 1,
        FitY = 2,
        FillX = 4,
        FillY = 8,
        NoneX = 16,
        NoneY = 32,
        Fit = FitX | FitY,
        Fill = FillX | FillY,
        None = NoneX | NoneY,
    }
}