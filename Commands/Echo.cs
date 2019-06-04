using System;
using System.Collections.Generic;
using System.Text;
using CMD = Commander;

namespace Commander.Commands
{
    public class Echo : Command
    {
        public override string[] CommandAliases()
        {
            return new string[] { "return", "print" };
        }

        public override CommandCopyright CommandCopyright() => CMD.CommandCopyright.EMPTY;

        public override string CommandDescription() => "Prints a message to the console.";

        public override string CommandName() => "echo";

        public override void OnInvoke(CommandInvoker invoker, CommandArguments args)
        {
            if (!(args.IsEmpty()) && args.GetArgumentAtPosition(0) == "@off")
            {
                invoker.GetProcessor().Echo = false;
            }
            else if (!(args.IsEmpty()) && args.GetArgumentAtPosition(0) == "@on")
            {
                invoker.GetProcessor().Echo = true;
            }
            else if (!(args.IsEmpty()))
            {
                string s = "";
                for (int i = 0; i < args.Count(); i++)
                {
                    var x = args.GetArgumentAtPosition(i);
                    if (s == "" || s == null)
                        s = x;
                    else
                        s += $" {x}";
                }
                Console.WriteLine(s);
            }
            else { Console.WriteLine("Huh, somehow we ended up here...?"); }
        }

        public override string CommandSyntax() => "[message | @on/@off]\n@on: Turns echo on.\n@off: Turns echo off.";

        public override CommandVersion CommandVersion() => new CommandVersion(1, 2, 1, "BETA");
    }
}