using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CMD = Commander;

namespace Commander.Commands
{
    public class Help : Command
    {
        public Help()
        {
        }

        public override string[] CommandAliases()
        {
            return new string[] { "cmds", "commands", "listcmds", "listcommands" };
        }

        public override CommandCopyright CommandCopyright()
        {
            return CMD.CommandCopyright.EMPTY;
        }

        public override string CommandDescription()
        {
            return "Displays all commands.";
        }

        public override string CommandName()
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
                foreach (Command c in invoker.GetCommands().ToArray())
                {
                    if (s == "" || s == null)
                        s = $"{c.CommandName()}: {c.CommandDescription()}";
                    else
                        s += $"\n{c.CommandName()}: {c.CommandDescription()}";
                }
                Console.WriteLine(s);
            }
            else if (args.GetArgumentAtPosition(0) == "-l" || args.GetArgumentAtPosition(0) == "--list")
            {
                //Display a sorted or organized list.
                string s = "";
                int cnt = 1;
                string ln = "";
                foreach (Command c in invoker.GetCommands().ToArray())
                {
                    if (cnt == 5)
                    {
                        if (s == "" || s == null)
                        {
                            s = $"{ln}";
                            ln = $"{c.CommandName()}";
                            cnt = 1;

                        }
                        else
                        {
                            s += $"\n{ln}";
                            ln = $"{c.CommandName()}";
                            cnt = 1;
                        }
                    }
                    else
                    {
                        if (ln == "" || ln == null)
                            ln = $"{c.CommandName()}";
                        else
                            ln += $", {c.CommandName()}";
                        cnt++;
                    }
                }
                Console.WriteLine(s);
            }
            else
            {
                //Display information about a specific command or it's alias.
                var cmd = invoker.GetCommand(args.GetArgumentAtPosition(0));
                if (cmd != null)
                {
                    Console.WriteLine(
                        $"==========|HELP|==========\n" +
                        $"Name: {cmd.CommandName()}\n" +
                        $"Description: {cmd.CommandDescription()}\n" +
                        $"Version: {cmd.CommandVersion()}\n" +
                        $"Copyright: {cmd.CommandCopyright()}\n" +
                        $"Aliases: {cmd.CommandAliases().ToString(' ')}\n" +
                        $"==========|HELP|=========="
                        );
                }
                else
                {
                    string s = "";
                    //Then display commands normally.
                    foreach (Command c in invoker.GetCommands().ToArray())
                    {
                        if (s == "" || s == null)
                            s = $"{c.CommandName()}: {c.CommandDescription()}";
                        else
                            s += $"\n{c.CommandName()}: {c.CommandDescription()}";
                    }
                    Console.WriteLine(s);
                }
            }
        }

        public override string CommandSyntax() => $"[-l | command]";

        public override CommandVersion CommandVersion() => new CommandVersion(1, 2, 1, "BETA");
    }
}