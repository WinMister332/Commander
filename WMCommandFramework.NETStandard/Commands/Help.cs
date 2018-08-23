using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.NETStandard.Commands
{
    public class Help : Command
    {
        public override string[] Aliases()
        {
            return new string[] { "cmd", "command", "cmds", "commands" };
        }

        public override string Description()
        {
            return "Lists all commands that where registered.";
        }

        public override void Invoke(CommandInvoker invoker, CommandArgs args)
        {
            if (args.IsEmpty())
            {
                //Execute Normally.
                foreach (Command c in invoker.GetCommands())
                {
                    Console.WriteLine($"{c.Name()}: {c.Description()}");
                }
            }
            else
            {
                if (args.StartsWithSwitch("l"))
                {
                    //Show without descriptions.
                    int cnt = 0;
                    string x = "";
                    string output = "";
                    foreach (Command c in invoker.GetCommands())
                    {
                        cnt++;
                        if (cnt == 5)
                        {
                            if (output == "" || output == null)
                                output = x;
                            else
                                output += $"\n{x}";
                            x = "";
                            cnt = 0;
                        }
                        else
                        {
                            if (x == "" || x == null)
                                x = $"{c.Name()}";
                            else
                                x += $", {c.Name()}";
                        }
                    }
                }
                else
                {
                    var cmd = invoker.GetCommand(args.GetArgAtPosition(0));
                    if (!(cmd == null))
                    {
                        //Execute CMD-Specific help.
                        if (cmd.Copyright() == null && cmd.Version() != null)
                        {
                            //Copyright is null, Version is not.
                            Console.WriteLine(
                                $"[+]==========|HELP|==========[+]\n" +
                                $"Name: {cmd.Name()}\n" +
                                $"Description: {cmd.Description()}\n" +
                                $"Syntax: {cmd.Syntax()}\n" +
                                $"Aliases: {ToString(cmd.Aliases())}\n" +
                                $"Version: {cmd.Version().ToString()}\n" +
                                $"[+]==========|HELP|==========[+]");
                        }
                        else if (cmd.Copyright() != null && cmd.Version() == null)
                        {
                            //Version is null, Copyright is not.
                            Console.WriteLine(
                                $"[+]==========|HELP|==========[+]\n" +
                                $"Name: {cmd.Name()}\n" +
                                $"Description: {cmd.Description()}\n" +
                                $"Syntax: {cmd.Syntax()}\n" +
                                $"Aliases: {ToString(cmd.Aliases())}\n" +
                                $"Copyright: {cmd.Copyright()}\n" +
                                $"[+]==========|HELP|==========[+]");
                        }
                        else if (cmd.Copyright() != null && cmd.Version() != null)
                        {
                            //Neither Copyright or Version is null.
                            Console.WriteLine(
                                $"[+]==========|HELP|==========[+]\n" +
                                $"Name: {cmd.Name()}\n" +
                                $"Description: {cmd.Description()}\n" +
                                $"Syntax: {cmd.Syntax()}\n" +
                                $"Aliases: {ToString(cmd.Aliases())}\n" +
                                $"Version: {cmd.Version()}" +
                                $"Copyright: {cmd.Copyright()}\n" +
                                $"[+]==========|HELP|==========[+]");
                        }
                        else
                        {
                            //Both are null.
                            Console.WriteLine(
                                $"[+]==========|HELP|==========[+]\n" +
                                $"Name: {cmd.Name()}\n" +
                                $"Description: {cmd.Description()}\n" +
                                $"Syntax: {cmd.Syntax()}\n" +
                                $"Aliases: {ToString(cmd.Aliases())}\n"+
                                $"[+]==========|HELP|==========[+]");
                        }
                    }
                    else
                    {
                        //Execute Normally.

                    }
                }
            }
        }

        public override string Name()
        {
            return "help";
        }

        public override string Syntax()
        {
            return "[-l | command] (-l: Lists all commands without their descriptions.)";
        }

        public override CommandVersion Version()
        {
            return new CommandVersion(1,0,3,"b");
        }

        private string ToString(string[] value)
        {
            string x = "";
            for (int i = 0; i == value.Length; i++)
            {
                var index = i--;
                if (x == "" || x == null)
                    x = $"{value[index]}";
                else
                    x += $", {value[index]}";
            }
            return x;
        }
    }
}
