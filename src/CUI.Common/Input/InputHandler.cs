using System;
using System.Linq;
using System.Threading;

namespace CUI.Common.Input;

public class InputHandler : IDisposable
{
    public readonly RenderContext RenderContext;
    public event EventHandler<InputHandlerEventArgs> InputReceived = delegate { };
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly Thread _inputThread;
    public int TabIndex { get; private set; } = 0;
    private Renderable? _focusElement;
    private bool _started = false;
    public Renderable? FocusElement => _focusElement;

    private Renderable? GetCurrentFocus()
    {
        Renderable? focus = RenderContext.FocusableElements.FirstOrDefault(x => x.TabIndex == TabIndex);
        if(focus == null && TabIndex != 0)
        {
            TabIndex = 0;
            focus = RenderContext.FocusableElements.FirstOrDefault(x => x.TabIndex == TabIndex);
        }
        return focus;
    }
    public InputHandler(RenderContext renderContext)
    {
        RenderContext = renderContext;
        _inputThread = new Thread(InputLoop);
    }

    public void Activate()
    {
        if(!_started)
        {
            _inputThread.Start();
            _started = true;
        }
    }

    private void InputLoop()
    {
        CancellationToken token = _cancellationTokenSource.Token;
        _focusElement = GetCurrentFocus();

        while (!token.IsCancellationRequested)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            
            InputHandlerEventArgs args = new InputHandlerEventArgs(key);
            InputReceived(this, args);

            if (args.Handled)
            {
                continue;
            }
            
            if (_focusElement is not { CaptureControlKeys: true } && key is { Modifiers: 0, Key: ConsoleKey.Tab })
            {
                TabIndex++;
                if(_focusElement != null)
                {
                    _focusElement.Focused = false;
                }

                _focusElement = GetCurrentFocus();
                if(_focusElement != null)
                {
                    _focusElement.Focused = true;
                }
            }
            else
            {
                _focusElement?.HandleInput(key);
            }
        }
    }

    public void Dispose()
    {
        if(_inputThread.IsAlive)
        {
            _cancellationTokenSource.Cancel();
        }

        _cancellationTokenSource.Dispose();
    }
}