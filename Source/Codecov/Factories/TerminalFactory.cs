﻿using System.Collections.Generic;
using Codecov.Terminal;

namespace Codecov.Factories
{
    internal static class TerminalFactory
    {
        public static IDictionary<TerminalName, ITerminal> Create()
        {
            var terminals = new Dictionary<TerminalName, ITerminal> { { TerminalName.Generic, new Terminal.Terminal() }, { TerminalName.Powershell, new PowerShell() } };

            foreach (var key in terminals.Keys)
            {
                if (!terminals[key].Exists)
                {
                    terminals.Remove(key);
                }
            }

            return terminals;
        }
    }
}
