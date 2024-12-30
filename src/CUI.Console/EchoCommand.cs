namespace CUI.Console;

internal class EchoCommand : Command
{
    public EchoCommand(CommandWindow window) : base("echo", window) { }

    public override void Execute(string args, Action<string> output)
    {
        output(args);
    }
}

internal class PrintEnvironmentVariablesCommand : Command
{
    public PrintEnvironmentVariablesCommand(CommandWindow window) : base("printenv", window) { }

    public override void Execute(string args, Action<string> output)
    {
        if (!string.IsNullOrEmpty(args))
        {
            foreach (string key in args.Split(' '))
            {
                output($"{key}={Environment.GetEnvironmentVariable(key)}");
            }
        }
        else
        {
            foreach (string key in Environment.GetEnvironmentVariables().Keys)
            {
                output($"{key}={Environment.GetEnvironmentVariable(key)}");
            }
        }
    }
}


internal class ResolveEnvironmentVariablesCommand : Command
{
    public ResolveEnvironmentVariablesCommand(CommandWindow window) : base("resolveenv", window) { }

    public override void Execute(string args, Action<string> output)
    {
        if (!string.IsNullOrEmpty(args))
        {
            output(Environment.ExpandEnvironmentVariables(args));
        }
        else
        {
            output("No environment variables specified.");
        }
    }
}