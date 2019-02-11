using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.Commands
{
    public class Exit : Command
    {
        public override string[] Aliases() => new string[] { "terminate", "quit", "stop", "end" };

        public override CommandCopyright Copyright() => CommandCopyright.VANROS;

        public override string Description() => "Ends or closes the current command processor.";

        public override string Name() => "exit";

        public override void OnInvoke(CommandInvoker invoker, CommandArguments args)
        {
            invoker.GetProcessor().ExitProcessor = true; //Terminates the current processor.
        }

        public override CommandVersion Version() => new CommandVersion(1, 2, 1, "BETA");
    }
}
