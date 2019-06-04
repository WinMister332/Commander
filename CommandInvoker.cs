using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    public class CommandInvoker
    {
        private CommandProcessor processor = null;
        private bool useQuotes = true;
        private List<Command> commands = null;

        public CommandInvoker(bool useQuotes = true)
        {
            commands = new List<Command>();
            this.useQuotes = useQuotes;
            //Add Default Commands.
            RegisterCommands(new Command[]
            {
                new Commands.Clear(),
                new Commands.Echo(),
                new Commands.Exit(),
                new Commands.Help()
            });
        }

        public CommandInvoker(int capacity, bool useQuotes = true)
        {
            commands = new List<Command>(capacity);
            this.useQuotes = useQuotes;
            //Add Default Commands.
            RegisterCommands(new Command[]
            {
                new Commands.Clear(),
                new Commands.Echo(),
                new Commands.Exit(),
                new Commands.Help()
            });
        }

        public void RegisterCommand(Command command)
        {
            if (!commands.Contains(command))
                commands.Add(command);
        }

        public void RegisterCommands(Command[] commands)
        {
            foreach (Command c in commands)
                RegisterCommand(c);
        }

        public void UnregisterCommand(Command command)
        {
            if (commands.Contains(command))
                commands.Remove(command);
        }

        public void UnregisterCommands(Command[] commands)
        {
            foreach (Command c in commands)
                UnregisterCommand(c);
        }

        public void OverrideCommand(Command oldCommand, Command newCommand)
        {
            var xindex = commands.IndexOf(oldCommand);
            commands[xindex] = newCommand;
        }
        public void OverrideCommand(string oldCommandName, Command newCommand)
        {
            var cmd = GetCommand(oldCommandName);
            if (cmd == null) return;
            if (GetCommands().ToList().Contains(cmd))
            {
                //overwrite existing.
                commands[GetCommands().ToList().IndexOf(cmd)] = newCommand;
            }
            else
            {
                //Just add it.
                RegisterCommand(newCommand);
            }
        }

        public Returnable<Command> GetCommands() => new Returnable<Command>(commands);

        public Command GetCommandByName(string name)
        {
            foreach (Command command in GetCommands().ToArray())
                if (command.CommandName().ToLower() == name.ToLower())
                    return command;
            return null;
        }
        public Command GetCommandByAlias(string alias)
        {
            foreach (Command cmd in GetCommands().ToArray())
                foreach (string aliasx in cmd.CommandAliases())
                    if (aliasx.ToLower() == alias.ToLower())
                        return cmd;
            return null;
        }
        public Command GetCommand(string value)
        {
            var cname = GetCommandByName(value);
            var calias = GetCommandByAlias(value);
            if (cname != null && calias == null)
                return cname;
            else if (cname == null && calias != null)
                return calias;
            else if (cname != null && calias != null)
                return cname;
            else return null;
        }

        public void Invoke(string rawInput)
        {
            //Split all values by space.
            var split = rawInput.Split(' ');
            //Get the name (command) value.
            var name = split[0];
            //Parse commands back into a string ignoring the first (name/command) value.
            var rawArgs = split.Skip(1).ToString();
            CommandArguments args = null;
            //If raw args is null, empty, or has whitespace.
            if (rawArgs == "" || rawArgs == null)
                args = new CommandArguments();
            else
                args = new CommandArguments(rawArgs.Split(' '));
            //Finally get command.
            var cmd = GetCommand(name);
            if (cmd == null)
            {
                Console.WriteLine($"\"{name}\", is not a valid internal or external command.");
            }
            else if ((name == "--version" || name == "-ver") && cmd == null)
            {
                //Display version.
                if (processor.ApplicationName.GetCopyright() == null || processor.ApplicationName.GetCopyright() == CommandCopyright.EMPTY)
                {
                    Console.WriteLine(
                    $"~~~~~~~~~~~~~~~~~~~~\n\n" +
                    $"{processor.ApplicationName.GetName()} Version: {processor.ApplicationName.GetVersion()}\n" +
                    $"\n~~~~~~~~~~~~~~~~~~~~\n"
                    );
                }
                else
                {
                    Console.WriteLine(
                    $"~~~~~~~~~~~~~~~~~~~~\n\n" +
                    $"{processor.ApplicationName.GetCopyright().ToString()}\n" +
                    $"{processor.ApplicationName.GetName()} Version: {processor.ApplicationName.GetVersion()}\n" +
                    $"\n~~~~~~~~~~~~~~~~~~~~\n"
                    );
                }
            }
            else
            {
                //Invoke the specified commands.
                if (!(cmd.CommandCopyright() == CommandCopyright.EMPTY || cmd.CommandCopyright() == null))
                    Console.WriteLine(cmd.CommandCopyright().ToString());

                cmd.OnInvoke(this, args);
                if (cmd.ThrowSyntax)
                    Console.WriteLine($"USAGE:\nLEGEND: '[]: Optional', '<>: Required', '| or ||: Or', and '& and &&: And'.\n{cmd.CommandSyntax()}");
            }
        }

        public CommandProcessor GetProcessor() => processor;

        internal void SetProcessor(CommandProcessor processor)
        {
            if (!(processor == null))
                this.processor = processor;
        }
    }
}
