using CUI.Common;
using CUI.Common.Components;

namespace CUI.Console;

internal class CommandWindow : IDisposable
{
    private readonly Text _output;
    private readonly TextInput _input;
    private readonly RenderContext _context;
    private readonly List<Command> _commands = new List<Command>();
    public IEnumerable<Command> Commands => _commands;
    private bool _exit;
    public CommandWindow(RenderContext context, Text output, TextInput input)
    {
        _context = context;
        _commands.Add(new PrintEnvironmentVariablesCommand(this));
        _commands.Add(new ResolveEnvironmentVariablesCommand(this));
        _commands.Add(new HelpCommand(this));
        _commands.Add(new EchoCommand(this));
        _commands.Add(new ExitCommand(this));
        _output = output;
        _input = input;
        _input.OnKeyPress += args =>
        {
            if (args.Key.Key == ConsoleKey.Enter && args.Key.Modifiers == 0)
            {
                string line = _input.Value;
                _output.Value += $"COMMAND: {line}\n";
                _input.Value = string.Empty;
                _input.CursorPosition = 0;
                args.Handled = true;
                
                Command? cmd = _commands.FirstOrDefault(c => line.StartsWith(c.Name));
                if (cmd is not null)
                {
                    cmd.Execute(line.Substring(cmd.Name.Length).Trim(), s => _output.Value += s + "\n");
                }
                else if (line == "clear")
                {
                    output.Value = string.Empty;
                }
                else
                {
                    _output.Value += $"Unknown Command: {_input.Value}\nTry 'help' for a list of commands\n";
                }
                _input.SendChangedEvent(_input);
            }
            else if(args.Key.Key == ConsoleKey.Escape)
            {
                _exit = true;
                args.Handled = true;
            }
        };
        _input.OnChanged += _ => context.Render();
        _output.Value = "Welcome to the Command Window\nTry 'help' for a list of commands\nExit by pressing 'ESC'\n";
        
    }

    public void Exit()
    {
        _exit = true;
    }

    public void Run()
    {
        while (!_exit)
        {
            _context.Render();
            Thread.Sleep(250);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}