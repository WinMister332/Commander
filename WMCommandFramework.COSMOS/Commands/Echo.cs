using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.COSMOS.Commands
{
    public class Echo : Command
    {
        public override string[] Aliases()
        {
            return new string[] { "print", "return" };
        }

        public override string Description()
        {
            return "Prints a message to the terminal.";
        }

        public override void Invoke(CommandInvoker invoker, CommandArgs args)
        {
            throw new NotImplementedException();
        }

        public override string Name()
        {
            return "echo";
        }

        public override string Syntax()
        {
            throw new NotImplementedException();
        }

        public override CommandVersion Version()
        {
            throw new NotImplementedException();
        }
    }
}
