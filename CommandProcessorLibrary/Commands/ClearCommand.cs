using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.Commands
{
    public class ClearCommand : Command
    {
        public override string[] CommandAliases()
        {
            return new string[] { "clr", "cls" };
        }

        public override string CommandDesc()
        {
            return "Clears the terminal of all it's text.";
        }

        public override string CommandName()
        {
            return "clear";
        }

        public override string CommandSynt()
        {
            return "";
        }

        public override CommandVersion CommandVersion()
        {
            return new WMCommandFramework.CommandVersion(1, 0, 1, "b");
        }

        public override void OnCommandInvoked(CommandInvoker invoker, CommandArgs args)
        {
            Console.Clear();
        }
    }
}
