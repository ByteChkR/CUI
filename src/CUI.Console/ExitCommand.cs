namespace CUI.Console;

internal class ExitCommand : Command
{
    public ExitCommand(CommandWindow window) : base("exit", window)
    {
    }

    public override void Execute(string args, Action<string> output)
    {
        output("Exiting...");
        Window.Exit();
    }
}