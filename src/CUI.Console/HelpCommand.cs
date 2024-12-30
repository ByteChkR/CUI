namespace CUI.Console;

internal class HelpCommand : Command
{
    public HelpCommand(CommandWindow window) : base("help", window) { }

    public override void Execute(string args, Action<string> output)
    {
        output("Available Commands:");
        foreach (Command command in Window.Commands)
        {
            output($" - {command.Name}");
        }
    }
}