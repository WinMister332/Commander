using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.Commands
{
    public class HelpCommand : Command
    {
        public override string[] CommandAliases()
        {
            return new string[] { "commands", "cmds", "command", "cmd" };
        }

        public override string CommandDesc()
        {
            return "Displays information on commands.";
        }

        public override string CommandName()
        {
            return "help";
        }

        public override string CommandSynt()
        {
            return "[command]";
        }

        public override CommandVersion CommandVersion()
        {
            return new WMCommandFramework.CommandVersion(1, 0, 1, "b");
        }

        public override void OnCommandInvoked(CommandInvoker invoker, CommandArgs args)
        {
            if (CommandUtils.DebugMode) Console.WriteLine($"Attempting to run \"help\"!");
            if (args.IsEmpty())
            {
                if (CommandUtils.DebugMode) Console.WriteLine($"Argument array is empty.");
                //List all commands with their descriptions.
                foreach (Command c in invoker.GetCommands())
                {
                    Console.WriteLine($"{c.CommandName()}: {c.CommandDesc()}");
                }
            }
            else
            {
                if (!(args.IsEmpty()) && (args.StartsWithSwitch("l")))
                {
                    int cnt = 0;
                    string outp = "";
                    string dat = "";
                    foreach (Command c in invoker.GetCommands())
                    {
                        cnt++;
                        if (outp == "" || outp == null)
                        {
                            outp = c.CommandName();
                        }
                        else
                        {
                            outp += $", {c.CommandName()}";
                        }
                        if (cnt == 5)
                        {
                            if (dat == "" || dat == null)
                            {
                                dat = outp;
                            }
                            else
                            {
                                dat += $"{outp}\n";
                            }
                            outp = "";
                            cnt = 0;
                        }
                    }
                    Console.WriteLine(dat);
                }
                else
                {
                    var cmd = invoker.GetCommand(args.GetArgAtPosition(0));
                    if (cmd == null)
                    {
                        foreach (Command c in invoker.GetCommands())
                        {
                            Console.WriteLine($"{c.CommandName()}: {c.CommandDesc()}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[]==========|Command Information|==========[]\n" +
                            $"Name: {cmd.CommandName()}\n" +
                            $"Description: {cmd.CommandDesc()}\n" +
                            $"Syntax: {cmd.CommandSynt()}\n" +
                            $"Version: {cmd.CommandVersion()}\n" +
                            $"Aliases: {ConvertToString(cmd.CommandAliases())}\n" +
                            $"[]==========|Command Information|==========[]");
                    }
                }
            }
        }

        private string ConvertToString(string[] arr)
        {
            if (arr == null || (arr.Length == 0))
                return "";
            else
            {
                string x = "";
                foreach (string s in arr)
                {
                    if (x == "" || x == null)
                        x = s;
                    else
                        x += $", {s}";
                }
                return x;
            }
        }
    }
}
