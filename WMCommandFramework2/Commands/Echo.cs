using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.Commands
{
    public class Echo : Command
    {
        public override string[] Aliases()
        {
            return new string[] { "return", "print" };
        }

        public override CommandCopyright Copyright() => CommandCopyright.VANROS;

        public override string Description() => "Prints a message to the console.";

        public override string Name() => "echo";

        public override void OnInvoke(CommandInvoker invoker, CommandArguments args)
        {
            if (!(args.IsEmpty()) && args.GetCommandAtPosition(0) == "@off")
            {
                invoker.GetProcessor().DisplayEcho = false;
            }
            else if (!(args.IsEmpty()) && args.GetCommandAtPosition(0) == "@on")
            {
                invoker.GetProcessor().DisplayEcho = true;
            }
            else if (!(args.IsEmpty()))
            {
                string s = "";
                for (int i = 0; i < args.Count; i++)
                {
                    var x = args.GetCommandAtPosition(i);
                    if (s == "" || s == null)
                        s = $"{x}";
                    else
                        s += $" {x}";
                }
                Console.WriteLine(s);

            }
            else { }
        }

        public override CommandVersion Version() => new CommandVersion(1, 2, 1, "BETA");
    }
}
