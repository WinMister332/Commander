using System;
using System.Collections.Generic;
using WMCommandFramework.Exceptions;
using System.Text;

namespace WMCommandFramework
{
    public class CommandInvoker
    {
        private List<Command> commands = new List<Command>();

        public CommandInvoker()
        {
            //TODO: Register Internal Commands.
            //AddCommand(new Help());
            //AddCommand(new Clear());
            //AddCommand(new Echo());
        }

        /// <summary>
        /// Adds a command to the Command Registry.
        /// </summary>
        /// <param name="c">The command to add to the Command Registry.</param>
        public void AddCommand(Command c)
        {
            commands.Add(c);
        }

        /// <summary>
        /// Removes a pre-existing command from the Command Registry.
        /// </summary>
        /// <param name="c">The command to remove from the Command Registry.</param>
        public void RemoveCommand(Command c)
        {
            commands.Remove(c);
        }

        /// <summary>
        /// Gets a list of all commands in the Command Registry.
        /// </summary>
        /// <returns>A list of commands in the Command Registry.</returns>
        public List<Command> GetCommands()
        {
            return commands;
        }

        /// <summary>
        /// Gets a command with the specified name.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <returns>The command with the specified name. Null if none.</returns>
        public Command GetCommandByName(string name)
        {
            Command out_x = null;
            foreach (Command c in commands)
            {
                if (c.CommandName().ToLower() == name.ToLower())
                {
                    out_x = c;
                    break;
                }
            }
            return out_x;
        }

        /// <summary>
        /// Gets a command with the specified alias.
        /// </summary>
        /// <param name="alias">The alias of the command.</param>
        /// <returns>The command with the specified alias. Null if none.</returns>
        public Command GetCommandByAlias(string alias)
        {
            Command out_x = null;
            foreach (Command c in commands)
            {
                var next = false;
                foreach (string s in c.CommandAliases())
                {
                    if (s.ToLower() == alias.ToLower())
                    {
                        out_x = c;
                        next = true;
                        break;
                    }
                }
                if (next == true) break;
            }
            return out_x;
        }

        /// <summary>
        /// Gets a command based on the specified value.
        /// </summary>
        /// <param name="value">The name or alias of the command.</param>
        /// <returns>The command with the specified value. Null if none.</returns>
        public Command GetCommand(string value)
        {
            var cname = GetCommandByName(value);
            var calias = GetCommandByAlias(value);
            Command out_x = null;
            if ((cname != null) && (calias == null))
                out_x = cname;
            else if ((cname == null) && (calias != null))
                out_x = calias;
            else
                out_x = null;
            return out_x;
        }

        /// <summary>
        /// Parses the input string and if a command exists it gets invoked.
        /// </summary>
        /// <param name="input">The string to parse containing command and args.</param>
        public void InvokeCommand(string input)
        {
            var x = input.Split(' ');
            var name = x[0];
            CommandArgs args = null;
            if (x.Length > name.Length) args = new CommandArgs();
            else args = new CommandArgs(new CommandArgs(x).Skip(name));
            var cmd = GetCommand(name);
            if (cmd == null)
            {
                //Not a command.
                Console.WriteLine($"\"{name.ToLower()}\", is not a valid command.");
            }
            else
            {
                if (!(args.isEmpty() && args.GetArgAtPosition(0) == "--version"))
                {
                    Console.WriteLine($@"{cmd.CommandName()}\nCurrent Version: {cmd.CommandVersion().ToString()}\n");
                }
                else
                {
                    try
                    {
                        //Invoke Command.
                        cmd.OnCommandInvoked(this, args);
                        return;
                    }
                    catch (SyntaxException sex)
                    {
                        //Unknown Error Print Syntax.
                        Console.WriteLine($"Syntax Error:\n<>: Required, []: Optional.\nUsage: {cmd.CommandSynt().ToLower()}");
                    }
                    catch (Exception ex)
                    {
                        if (WMCommandFramework.CommandUtils.DebugMode)
                        {
                            var data = $"[CommandInvoker]: An unknown error occurred! -> {ex.ToString()}";
                            Console.WriteLine(data);
                            System.Diagnostics.Debug.WriteLine(data);
                        }
                        return;
                    }
                }
            }
        }
    }
}
