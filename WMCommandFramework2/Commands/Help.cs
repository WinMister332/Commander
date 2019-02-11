using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WMCommandFramework;
using WMCommandFramework.Utilities;
using WMCommandFramework.Attributes;

namespace WMCommandFramework.Commands
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

        public override CommandCopyright Copyright()
        {
            return CommandCopyright.VANROS;
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
                string s = "";
                //Then display commands normally.
                foreach (Command c in invoker.GetCommands())
                {
                    if (s == "" || s == null)
                        s = $"{c.Name()}: {c.Description()}";
                    else
                        s += $"/n{c.Name()}: {c.Description()}";
                }
                Console.WriteLine(s);
            }
            else if (args.GetCommandAtPosition(0) == "-l" || args.GetCommandAtPosition(0) == "--list")
            {
                //Display a sorted or organized list.
                string s = "";
                int cnt = 1;
                string ln = "";
                foreach (Command c in invoker.GetCommands())
                {
                    if (cnt == 5)
                    {
                        if (s == "" || s == null)
                        {
                            s = $"{ln}";
                            ln = $"{c.Name()}";
                            cnt = 1;

                        }
                        else
                        {
                            s += $"\n{ln}";
                            ln = $"{c.Name()}";
                            cnt = 1;
                        }
                    }
                    else
                    {
                        if (ln == "" || ln == null)
                            ln = $"{c.Name()}";
                        else
                            ln += $", {c.Name()}";
                        cnt++;
                    }
                }
                Console.WriteLine(s);
            }
            else
            {
                //Display information about a specific command or it's alias.
                var cmd = invoker.GetCommand(args.GetCommandAtPosition(0));
                if (cmd != null)
                {
                    Console.WriteLine(
                        $"==========|HELP|==========\n" +
                        $"Name: {cmd.Name()}\n" +
                        $"Description: {cmd.Description()}\n" +
                        $"Version: {cmd.Version()}" +
                        $"Copyright: {cmd.Copyright()}" +
                        $"Aliases: {cmd.Aliases().ToString()}\n" +
                        $"==========|HELP|=========="
                        );
                }
                else
                {
                    string s = "";
                    //Then display commands normally.
                    foreach (Command c in invoker.GetCommands())
                    {
                        if (s == "" || s == null)
                            s = $"{c.Name()}: {c.Description()}";
                        else
                            s += $"/n{c.Name()}: {c.Description()}";
                    }
                    Console.WriteLine(s);
                }
            }
        }

        public override CommandVersion Version()
        {
            return new CommandVersion(1,2,1, "BETA");
        }
    }
}
