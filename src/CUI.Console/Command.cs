namespace CUI.Console;

internal abstract class Command
{
    protected CommandWindow Window { get; }
    protected Command(string name, CommandWindow window)
    {
        Name = name;
        Window = window;
    }
    public string Name { get; }
    public abstract void Execute(string args, Action<string> output);
}