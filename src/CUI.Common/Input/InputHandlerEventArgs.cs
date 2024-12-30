using System;

namespace CUI.Common.Input;

public class InputHandlerEventArgs : EventArgs
{
    public ConsoleKeyInfo Key { get; }
    public bool Handled { get; set; }
    public InputHandlerEventArgs(ConsoleKeyInfo key)
    {
        Key = key;
    }
}