namespace Codecov.Terminal
{
    internal interface ITerminal
    {
        bool Exists { get; }

        string Run(string command, string commandArguments);

        string RunScript(string script);
    }
}
