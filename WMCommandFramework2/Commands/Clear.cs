using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.Commands
{
    public class Clear : Command
    {
        public override string[] Aliases() => new string[] { "cls", "clr", };

        public override CommandCopyright Copyright() => CommandCopyright.VANROS;

        public override string Description() => "Clears the command terminal.";

        public override string Name() => "clear";

        public override void OnInvoke(CommandInvoker invoker, CommandArguments args)
        {
            Console.Clear();
        }

        public override CommandVersion Version() => new CommandVersion(1,2,1, "BETA");
    }
}
