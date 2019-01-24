using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WMCommandFramework2;
using WMCommandFramework2.Attributes;

namespace WMCommandFramework2.Commands
{
    public class Help : Command
    {
        public Help()
        {
        }

        public override string[] Aliases()
        {
            return new string[] { "cmds", "commands", "listcmds", "listcommands" };
        }

        public override string Copyright()
        {
            return "";
        }

        public override string Description()
        {
            return "Displays all commands.";
        }

        public override string Name()
        {
            return "Help";
        }

        public override void OnInvoke(CommandInvoker invoker, CommandArguments args)
        {
            //Check if the argument array is empty.
            if (args.IsEmpty())
            {
                //Then display commands normally.

            }
            else if (args.GetCommandAtPosition(0) == "-l" || args.GetCommandAtPosition(0) == "--list")
            { 
                //Display a sorted or organized list.

            }
            else
            { 
                //Display information about a specific command or it's alias.

            }
        }

        public override string Version()
        {
            return "1.0.1-RN";
        }
    }
}
