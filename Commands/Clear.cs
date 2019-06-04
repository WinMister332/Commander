using System;
using System.Collections.Generic;
using System.Text;
using CMD = Commander;

namespace Commander.Commands
{
    public class Clear : Command
    {
        public override string[] CommandAliases() => new string[] { "cls", "clr", };

        public override CommandCopyright CommandCopyright() => CMD.CommandCopyright.EMPTY;

        public override string CommandDescription() => "Clears the command terminal.";

        public override string CommandName() => "clear";

        public override void OnInvoke(CommandInvoker invoker, CommandArguments args)
        {
            Console.Clear();
        }

        public override string CommandSyntax() => "";

        public override CommandVersion CommandVersion() => new CommandVersion(1, 2, 1, "BETA");
    }
}