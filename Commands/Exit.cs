using System;
using System.Collections.Generic;
using System.Text;
using CMD = Commander;

namespace Commander.Commands
{
    public class Exit : Command
    {
        public override string[] CommandAliases() => new string[] { "terminate", "quit", "stop", "end" };

        public override CommandCopyright CommandCopyright() => CMD.CommandCopyright.EMPTY;

        public override string CommandDescription() => "Ends or closes the current command processor.";

        public override string CommandName() => "exit";

        public override void OnInvoke(CommandInvoker invoker, CommandArguments args)
        {
            invoker.GetProcessor().ExitProcessor(); //Terminates the current processor.
        }

        public override string CommandSyntax() => $"";

        public override CommandVersion CommandVersion() => new CommandVersion(1, 2, 1, "BETA");
    }
}