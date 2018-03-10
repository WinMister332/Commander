using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.Commands
{
    public class EchoCommand : Command
    {
        public override string[] CommandAliases()
        {
            return new string[] { "print", "return" };
        }

        public override string CommandDesc()
        {
            return "Prints the specified information to the console.";
        }

        public override string CommandName()
        {
            return "echo";
        }

        public override string CommandSynt()
        {
            return "<message>";
        }

        public override CommandVersion CommandVersion()
        {
            return new WMCommandFramework.CommandVersion(1,0,1,"b");
        }

        public override void OnCommandInvoked(CommandInvoker invoker, CommandArgs args)
        {
            if (args.IsEmpty()) throw new Exceptions.SyntaxException("The \"message\" argument cannot be null or empty.");
            else
            {
                Console.WriteLine($"{args.GetArgAtPosition(0)}");
            }
        }
    }
}
