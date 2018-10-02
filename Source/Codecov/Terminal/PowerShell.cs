namespace Codecov.Terminal
{
    internal class PowerShell : Terminal
    {
        public override bool Exists => !string.IsNullOrWhiteSpace(RunScript("$PSVersionTable.PSVersion"));

        public override string Run(string command, string commandArguments)
        {
            return base.Run("powershell", $@"-command ""{command} {commandArguments}""");
        }
    }
}
