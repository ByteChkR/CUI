using System;
using System.Numerics;

namespace CUI.Common.Components;

public class TextInput : Text
{
    public int CursorPosition { get; set; }
    public TextInput()
    {
        Focusable = true;
    }

    public override Vector2 InputFocus => new Vector2(CursorPosition, 0);

    protected override void HandleKeyPress(ConsoleKeyInfo key)
    {
        CursorPosition = Math.Min(Value.Length, CursorPosition);
        if(char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || char.IsWhiteSpace(key.KeyChar))
        {
            Value = Value.Insert(CursorPosition, key.KeyChar.ToString());
            CursorPosition++;
            SendChangedEvent(this);
        }
        else if (key.Modifiers == 0)
        {
            if (key.Key == ConsoleKey.Backspace)
            {
                if(Value.Length > 0)
                {
                    Value = Value.Remove(CursorPosition - 1, 1);
                    CursorPosition = Math.Max(0, CursorPosition - 1);
                    SendChangedEvent(this);
                }
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                CursorPosition = Math.Min(Value.Length, CursorPosition + 1);
                SendChangedEvent(this);
            }
            else if(key.Key == ConsoleKey.LeftArrow)
            {
                CursorPosition = Math.Max(0, CursorPosition - 1);
                SendChangedEvent(this);
            }
        }
        else if (key is { Modifiers: ConsoleModifiers.Alt, Key: ConsoleKey.Enter })
        {
            Value += "\n";
            SendChangedEvent(this);
        }
    }
}