using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using WMCommandFramework.Commands;
using WMCommandFramework.Utilities;

namespace WMCommandFramework
{
    internal static class CommandUtilities
    {
        internal static bool DebugMode { get; set; } = false;
        internal static ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.White;
        internal static ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Blue;
        internal static AppName ApplicationName { get; set; } = AppName.WMCommandFramework;
        internal static InputMessage Message { get; set; } = InputMessage.EMPTY;
    }

    public sealed class CommandProcessor
    {
        public static CommandProcessor INSTANCE = null;
        private CommandInvoker invoker = null;

        /// <summary>
        /// Creates a new instance of the <see cref="CommandProcessor"/> class.
        /// </summary>
        public CommandProcessor()
        {
            INSTANCE = this;
            invoker = new CommandInvoker();
            invoker.SetProcessor(this);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CommandProcessor"/> class with the specified invoker.
        /// </summary>
        /// <param name="invoker">A pre-set invoker to utilize in this processor.</param>
        public CommandProcessor(CommandInvoker invoker)
        {
            INSTANCE = this;
            if (!(invoker == null))
            {
                invoker.SetProcessor(this);
                this.invoker = invoker;
            }
        }

        /// <summary>
        /// Get the <see cref="CommandInvoker"/> that was set by the processor.
        /// </summary>
        /// <returns>The <see cref="CommandInvoker"/> that was set.</returns>
        public CommandInvoker GetInvoker() => invoker;

        private ConsoleColor currentFGColor = ConsoleColor.White;
        private ConsoleColor currentBGColor = ConsoleColor.Blue;

        /// <summary>
        /// Gets or Sets whether debug output can be sent to the console.
        /// </summary>
        public bool DebugMode
        {
            get => CommandUtilities.DebugMode;
            set => CommandUtilities.DebugMode = value;
        }

        public ConsoleColor DefaultForegroundColor
        {
            get => CommandUtilities.DefaultForegroundColor;
            set => CommandUtilities.DefaultForegroundColor = value;
        }

        public ConsoleColor DefaultBackgroundColor
        {
            get => CommandUtilities.DefaultBackgroundColor;
            set => CommandUtilities.DefaultBackgroundColor = value;
        }
        /// <summary>
        /// Gets or Sets the name, version, and copyright information of the application this processor is bound to.
        /// </summary>
        public AppName ApplicationName
        {
            get => CommandUtilities.ApplicationName;
            set => CommandUtilities.ApplicationName = value;
        }
        /// <summary>
        /// Gets or Sets a message to display before the input field.
        /// </summary>
        public InputMessage Message
        {
            get => CommandUtilities.Message;
            set
            {
                if (!(value == null || value == InputMessage.EMPTY))
                {
                    value.SetProcessor(this);
                    CommandUtilities.Message = value;
                }
                else
                {
                    var x = InputMessage.EMPTY;
                    x.SetProcessor(this);
                    CommandUtilities.Message = x;
                }
            }
        }

        private bool closeProcessor = false;
        /// <summary>
        /// Stops looping the processor if the processor is looping.
        /// </summary>
        public bool ExitProcessor
        {
            get => closeProcessor;
            set => closeProcessor = value;
        }

        private bool displayEcho = true;
        /// <summary>
        /// Gets or Sets whether the preset InputMessage will be printed to the console.
        /// </summary>
        public bool DisplayEcho
        {
            get => displayEcho;
            set => displayEcho = value;
        }

        private void PrintMessage()
        {
            var a = Message;
            var b = a.GetMessages();
            foreach (InputMessage.Message m in b)
            {
                Console.ForegroundColor = m.GetColor();
                Console.Write(m.GetMessage());
                Console.ForegroundColor = DefaultForegroundColor;
            }
            Console.Write("> ");
        }

        /// <summary>
        /// Processes all input sent to the console and makes sure it's not null before forwarding the command to the invoker.
        /// </summary>
        /// <param name="loopProcessor">Sets whether the processor will loop instead of terminating after checking a command.</param>
        public void Process(bool loopProcessor = false)
        {
            if (loopProcessor)
            {
                //Loop.
                while (true)
                {
                    //Closes the processor loop if told to.
                    if (closeProcessor) break;
                    //Do data.

                    //Write message text to console.
                    if (DisplayEcho)
                        PrintMessage();

                    //Reads the data written into the console.
                    var input = Console.ReadLine();
                    if (!(input == null || input == ""))
                        //Send to the invoker to process the input.
                        invoker.Invoke(input);
                }
            }
            else
            {
                //Don't loop.
                //Do data.

                //Write message text to console.
                if (DisplayEcho)
                   PrintMessage();

                //Reads the data written into the console.
                var input = Console.ReadLine();
                if (!(input == null || input == ""))
                    //Send to the invoker to process the input.
                    invoker.Invoke(input);
            }
        }
    }

    public sealed class CommandInvoker
    {
        private CommandProcessor commandProcessor;
        private List<Command> commands;

        /// <summary>
        /// Creates a new instance of the <see cref="CommandInvoker"/> class.
        /// </summary>
        public CommandInvoker()
        {
            commands = new List<Command>();
            //Register Default Commands.
            Register(new Command[]
            {
                new Help(),
                new Clear(),
                new Echo(),
                new Exit()
            });
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CommandInvoker"/> class
        /// </summary>
        /// <param name="capacity"></param>
        public CommandInvoker(int capacity)
        {
            commands = new List<Command>(capacity);
            //Register Default Commands.
            Register(new Command[]
            {
                new Help(),
                new Clear(),
                new Echo(),
                new Exit()
            });
        }

        /// <summary>
        /// Gets the processor that passes command data to this class. 
        /// </summary>
        /// <returns>The parent <see cref="CommandProcessor"/>.</returns>
        public CommandProcessor GetProcessor() => commandProcessor;

        /// <summary>
        /// Register the specified command.
        /// </summary>
        /// <param name="command">The command to register.</param>
        public void Register(Command command)
        {
            if (!(commands.Contains(command)))
            {
                command.SetInvoker(this);
                commands.Add(command);
            }
        }

        /// <summary>
        /// Register the specified commands.
        /// </summary>
        /// <param name="commands">The commands to register.</param>
        public void Register(Command[] commands)
        {
            foreach (Command cmd in commands)
                Register(cmd);
        }

        /// <summary>
        /// Unregister the specified command.
        /// </summary>
        /// <param name="command">The commands to register.</param>
        public void Unregister(Command command)
        {
            if (commands.Contains(command))
            {
                commands.Remove(command);
                command.UnsetInvoker();
            }
        }

        /// <summary>
        /// Unregister the specified commands.
        /// </summary>
        /// <param name="commands">Commands.</param>
        public void Unregister(Command[] commands)
        {
            foreach (Command cmd in commands)
                Unregister(cmd);
        }

        /// <summary>
        /// Gets all the registered commands.
        /// </summary>
        /// <returns>A collection of all registered commands.</returns>
        public Command[] GetCommands() => commands.ToArray();

        /// <summary>
        /// Gets the name of the command by its name.
        /// </summary>
        /// <returns>The requested command.</returns>
        /// <param name="name">The name of the command.</param>
        public Command GetCommandByName(string name)
        {
            foreach (Command command in GetCommands())
                if (command.Name().Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    return command;
            return null;
        }

        /// <summary>
        /// Gets the command by its alias.
        /// </summary>
        /// <returns>The requested command.</returns>
        /// <param name="alias">The alias of the command.</param>
        public Command GetCommandByAlias(string alias)
        {
            foreach (Command command in GetCommands())
                foreach (string s in command.Aliases())
                    if (s.Equals(alias, StringComparison.CurrentCultureIgnoreCase))
                        return command;
            return null;
        }

        /// <summary>
        /// Gets the command based on the specified name or alias.
        /// </summary>
        /// <returns>The requested command.</returns>
        /// <param name="value">The name or alias of the command.</param>
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
            else
                return null;
        }

        /// <summary>
        /// Override a command with the same name as the specified command.
        /// </summary>
        /// <param name="command">The command to override.</param>
        public void Override(Command command)
        {
            //Override existing command with same name.
            var c1 = GetCommand(command.Name());
            if (!(c1 == null))
                Unregister(c1);
            //Override existing command with one or more of the same aliases.
            foreach (string s in command.Aliases())
            {
                var c2 = GetCommand(s);
                if (!(c2 == null))
                    Unregister(c2);
            }
            //Register the new command.
            Register(command);
        }

        /// <summary>
        /// Override the specified 'oldCommand' with the specified 'newCommand'.
        /// </summary>
        /// <param name="oldCommand">The previous command to override with the new command.</param>
        /// <param name="newCommand">The new command that'll replace the overriden command.</param>
        public void Override(Command oldCommand, Command newCommand)
        {
            Unregister(oldCommand);
            Register(newCommand);
        }

        /// <summary>
        /// Override the command with the specified name or alias with the command specified.
        /// </summary>
        /// <param name="oldCommand">The name of the command to remove.</param>
        /// <param name="newCommand">The replacement command.</param>
        public void Override(string oldCommand, Command newCommand)
        {
            Unregister(GetCommand(oldCommand));
            Register(newCommand);
        }

        /// <summary>
        /// Parses and invokes the command and data passed to it. Will return an error if no valid command was found.
        /// </summary>
        /// <param name="input">The command and arguments to parse and invoke.</param>
        public void Invoke(string input)
        {
            //Split all data in the string by space.
            var x = input.Split(' ');
            //Get the name of the command that was passed.
            var name = x[0];
            //Convert all data in 'x' back to string..
            var xarg = x.Skip(1).ToString(); //Converts back to the original string excluding the name.
            //check if the xarg variable is null, if so, do nothing with it.
            CommandArguments args = null;
            if (xarg == null || xarg == "")
                args = new CommandArguments(new string[0]);
            else
                args = new CommandArguments(ParseArguments(xarg));
            //Finally, get command with the name provided.
            var cmd = GetCommand(name);
            //Check if the command is valid, if not return a not found error.
            if (cmd == null)
            {
                Console.WriteLine($"\"{name}\", is not a valid internal or external command!");
            }
            else if (name == "--version" || name == "-ver")
            {
                if (CommandUtilities.ApplicationName.GetCopyright() == CommandCopyright.VANROS ||
                    CommandUtilities.ApplicationName.GetCopyright() == CommandCopyright.EMPTY)
                    Console.WriteLine(
                        $"~~~~~~~~~| VERSION |~~~~~~~~~\n" +
                        $"{AppName.WMCommandFramework.GetCopyright()}\n" +
                        $"{AppName.WMCommandFramework.GetName()} Version: {AppName.WMCommandFramework.GetVersion()}\n" +
                        $"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
                        );
                else
                    Console.WriteLine(
                        $"~~~~~~~~~| VERSION |~~~~~~~~~\n" +
                        $"{CommandUtilities.ApplicationName.GetCopyright()}\n" +
                        $"{CommandUtilities.ApplicationName.GetName()} Version: {CommandUtilities.ApplicationName.GetVersion()}\n" +
                        $"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"
                        );
            }
            else
            {
                //Do Stuff.
                //First, check command version, then execute command.
                if (args.Count == 0)
                {
                    if (cmd.Copyright() != CommandCopyright.EMPTY)
                        Console.WriteLine($"{cmd.Copyright()}");
                    cmd.OnInvoke(this, args);
                    if (cmd.ThrowSyntax)
                        Console.WriteLine($"USAGE:\nLEGEND: '[]: Optional', '<>: Required', '| or ||: Or', and '& and &&: And'.\n{cmd.Syntax()}");
                }
                else
                {
                    if (args.GetArgumentAtPosition(0) == $"--version" || args.GetArgumentAtPosition(0) == $"-ver")
                    {
                        if (cmd.Copyright() != null && cmd.Copyright() != CommandCopyright.EMPTY)
                        {
                            Console.WriteLine($"~~~~~~~~~~~~~~~~~~~~\n" +
                                $"{cmd.Copyright()}\n" +
                                $"{cmd.Name()} Version: {cmd.Version().ToString()}\n" +
                                $"~~~~~~~~~~~~~~~~~~~~");
                        }
                        else
                        {
                            Console.WriteLine($"~~~~~~~~~~~~~~~~~~~~\n" +
                                $"{cmd.Name()} Version: {cmd.Version().ToString()}\n" +
                                $"~~~~~~~~~~~~~~~~~~~~");
                        }
                    }
                }
            }
        }

        private List<string> ParseArguments(string s, bool showInsideQuotes = true)
        {
            const char splitChar = ' ';
            const char quote = '"';
            bool isInsideQuotes = false;
            List<string> tokens = new List<string>();
            foreach (char c in s)
            {
                if (c == quote)
                {
                    if (isInsideQuotes)
                    {
                        if (showInsideQuotes)
                        {
                            tokens[tokens.Count - 1] += c;
                            isInsideQuotes = !isInsideQuotes;
                        }
                        else
                            isInsideQuotes = !isInsideQuotes;
                    }
                    else
                        isInsideQuotes = !isInsideQuotes;
                }
                else if (!(char.IsLetterOrDigit(c)) && c == splitChar && !(isInsideQuotes))
                    tokens.Add("");
                else
                    tokens[tokens.Count - 1] += c;
            }
            return tokens;
        }

        internal void SetProcessor(CommandProcessor processor)
        {
            if (processor == null)
                commandProcessor = CommandProcessor.INSTANCE;
            else
                commandProcessor = processor;
        }

        internal void UnsetProcessor()
        {
            if (commandProcessor != null)
                commandProcessor = null;
        }
    }

    public sealed class CommandArguments
    {
        private List<string> args = null;

        internal CommandArguments(string[] strx)
        {
            args = new List<string>(strx.Length);
            foreach (string s in strx)
                args.Add(s);
        }

        internal CommandArguments(List<string> strx)
        {
            args = strx;
        }

        #region General Functions

        /// <summary>
        /// Returns the max number of arguments that where passed into the constructor.
        /// </summary>
        public int Count => args.Count;

        /// <summary>
        /// Checks if there are NO arguments that where passed into the constructor.
        /// </summary>
        /// <returns>True, if there was nothing passed into the constructor.</returns>
        public bool IsEmpty()
        {
            if (Count <= 0) return true;
            return false;
        }

        /// <summary>
        /// Gets the argument at the position in the argument array.
        /// </summary>
        /// <param name="position">The position in the argument array.</param>
        /// <returns>The argument found.</returns>
        public string GetArgumentAtPosition(int position)
        {
            if (!(IsEmpty()))
                return args[position];
            return "";
        }

        private int GetLength(List<string> args)
        {
            if (args.Count >= 1)
                return (args.Count - 1);
            else
                return 0;
        }

        #endregion

        #region StartsWith

        /// <summary>
        /// Checks if the arg array starts with the specified value.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <returns>True, if the argument was found.</returns>
        public bool StartsWith(string value)
        {
            if (!IsEmpty())
            {
                if (args[0].Equals(value, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if the arg array starts with the specified switch.
        /// </summary>
        /// <param name="switchValue">The switch to check.</param>
        /// <returns>True, if the switch was found.</returns>
        public bool StartsWithSwitch(string switchValue)
        {
            if (StartsWith($"-{switchValue}"))
                return true;
            return false;
        }
        /// <summary>
        /// Checks if the arg array starts with the specified conditional argument.
        /// </summary>
        /// <param name="value">The conditional argument to check.</param>
        /// <returns>True, if the conditional argument was found.</returns>
        public bool StartsWithConditional(string header)
        {
            if (StartsWithSwitch($"{header}:"))
                return true;
            return false;
        }
        /// <summary>
        /// Checks if the arg array starts with the specified valued argument.
        /// </summary>
        /// <param name="value">The valued argument to check.</param>
        /// <returns>True, if the valued argument was found.</returns>
        public bool StartsWithValued(string key)
        {
            if (StartsWithSwitch($"{key}="))
                return true;
            return false;
        }
        /// <summary>
        /// Checks if the arg array starts with the specified variable.
        /// </summary>
        /// <param name="value">The variable to check.</param>
        /// <returns>True, if the variable was found.</returns>
        public bool StartsWithVariable(Variable variable)
        {
            if (StartsWith(variable.ToString()))
                return true;
            return false;
        }

        #endregion

        #region EndsWith

        /// <summary>
        /// Checks if the arg array ends with the specified value.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <returns>True, if the argument was found.</returns>
        public bool EndsWith(string value)
        {
            if (!IsEmpty())
            {
                var x = GetArgumentAtPosition(GetLength(args));
                if ((x != "" || x != null) && x.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if the arg array ends with the specified switch.
        /// </summary>
        /// <param name="value">The switch to check.</param>
        /// <returns>True, if the switch was found.</returns>
        public bool EndsWithSwitch(string switchValue)
        {
            if (EndsWith($"-{switchValue}"))
                return true;
            return false;
        }
        /// <summary>
        /// Checks if the arg array ends with the specified conditiona argument.
        /// </summary>
        /// <param name="value">The conditional argument to check.</param>
        /// <returns>True, if the conditional argument was found.</returns>
        public bool EndsWithConditional(string header)
        {
            if (EndsWithSwitch($"{header}:"))
                return true;
            return false;
        }
        /// <summary>
        /// Checks if the arg array ends with the specified valued argument.
        /// </summary>
        /// <param name="value">The valued argument to check.</param>
        /// <returns>True, if the valued argument was found.</returns>
        public bool EndsWithValued(string key)
        {
            if (EndsWithSwitch($"{key}="))
                return true;
            return false;

        }
        /// <summary>
        /// Checks if the arg array ends with the specified variable.
        /// </summary>
        /// <param name="value">The variable to check.</param>
        /// <returns>True, if the variable was found.</returns>
        public bool EndsWithVariable(Variable variable)
        {
            if (ContainsArgument(variable.ToString()))
                return true;
            return false;
        }

        #endregion

        #region Contains

        /// <summary>
        /// Checks if the argument array contains the specified value.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True, if the value was found.</returns>
        public bool ContainsArgument(string value)
        {
            if (!IsEmpty())
            {
                foreach (string s in args)
                    if (s.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                        return true;

            }
            return false;
        }
        /// <summary>
        /// Checks if the argument array contains the specified value.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="position">The position within the array to check.</param>
        /// <returns>True, if the value was found.</returns>
        public bool ContainsArgument(int position, string value)
        {
            if (!IsEmpty())
            {
                var x = args[position];
                if (!(x == "" || x == null) && x.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if the argument array contains the specified switch.
        /// </summary>
        /// <param name="value">The switch to check.</param>
        /// <returns>True, if the switch was found.</returns>
        public bool ContainsSwitch(string value)
        {
            var x = ContainsArgument($"-{value}");
            if (x) return true;
            return false;
        }
        /// <summary>
        /// Checks if the argument array contains the specified switch.
        /// </summary>
        /// <param name="value">The switch to check.</param>
        /// <param name="position">The position within the array to check.</param>
        /// <returns>True, if the switch was found.</returns>
        public bool ContainsSwitch(int position, string switchValue)
        {
            return ContainsArgument(position, $"-{switchValue}");
        }
        /// <summary>
        /// Checks if the argument array contains the specified conditional argument.
        /// </summary>
        /// <param name="value">The conditional argument to check.</param>
        /// <returns>True, if the conditional argument was found.</returns>
        public bool ContainsConditional(string header)
        {
            var x = ContainsSwitch($"{header}:");
            if (x) return true;
            return false;

        }
        /// <summary>
        /// Checks if the argument array contains the specified conditional argument.
        /// </summary>
        /// <param name="value">The conditional argument to check.</param>
        /// <param name="position">The position within the array to check.</param>
        /// <returns>True, if the conditional argument was found.</returns>
        public bool ContainsConditional(int position, string header)
        {
            return ContainsSwitch(position, $"{header}:");
        }
        /// <summary>
        /// Checks if the argument array contains the specified valued argument.
        /// </summary>
        /// <param name="value">The valued argument to check.</param>
        /// <returns>True, if the valued argument was found.</returns>
        public bool ContainsValuedArgument(string key)
        {
            var x = ContainsSwitch($"{key}=");
            if (x) return true;
            return false;
        }
        /// <summary>
        /// Checks if the argument array contains the specified valued argument.
        /// </summary>
        /// <param name="value">The valued argument to check.</param>
        /// <param name="position">The position within the array to check.</param>
        /// <returns>True, if the valued argument was found.</returns>
        public bool ContainsValuedArgument(int position, string key)
        {
            return ContainsSwitch(position, $"{key}=");
        }
        /// <summary>
        /// Checks if the argument array contains the specified variable.
        /// </summary>
        /// <param name="value">The variable to check.</param>
        /// <returns>True, if the variable was found.</returns>
        public bool ContainsVariable(Variable variable)
        {
            var x = ContainsArgument(variable.ToString());
            if (x) return true;
            return false;
        }
        /// <summary>
        /// Checks if the argument array contains the specified variable.
        /// </summary>
        /// <param name="value">The variable to check.</param>
        /// <param name="position">The position within the array to check.</param>
        /// <returns>True, if the variable was found.</returns>
        public bool ContainsVariable(int position, Variable variable)
        {
            return ContainsArgument(position, variable.ToString());
        }

        #endregion

        #region IndexOf

        /// <summary>
        /// Gets the index of the specified argument.
        /// </summary>
        /// <param name="value">The argument.</param>
        /// <returns>The index of the specified argument.</returns>
        public int IndexOf(string value)
        {
            if (!IsEmpty())
            {
                for (int i = 0; i < args.Count; i++)
                {
                    var x = args[i];
                    if (x.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                        return i;
                }
            }
            return 0;
        }
        /// <summary>
        /// Gets the index of the specified switch.
        /// </summary>
        /// <param name="value">The switch.</param>
        /// <returns>The index of the specified switch.</returns>
        public int IndexOfSwitch(string switchValue)
        {
            return IndexOf($"-{switchValue}");
        }
        /// <summary>
        /// Gets the index of the specified conditional argument.
        /// </summary>
        /// <param name="value">The conditional argument.</param>
        /// <returns>The index of the specified conditional argument.</returns>
        public int IndexOfConditional(string header)
        {
            return IndexOfSwitch($"{header}:");
        }
        /// <summary>
        /// Gets the index of the specified valued argument.
        /// </summary>
        /// <param name="value">The valued argument.</param>
        /// <returns>The index of the specified valued argument.</returns>
        public int IndexOfValued(string key)
        {
            return IndexOfSwitch($"{key}=");
        }
        /// <summary>
        /// Gets the index of the specified variable.
        /// </summary>
        /// <param name="value">The variable.</param>
        /// <returns>The index of the specified variable.</returns>
        public int IndexOfVariable(Variable variable)
        {
            return IndexOf(variable.ToString());
        }

        #endregion

        #region ValueOf

        /// <summary>
        /// Gets the value of a conditional argument.
        /// -header:value
        /// </summary>
        /// <param name="header">The header of the conditional argument.</param>
        /// <returns>The value of the conditional.</returns>
        public string ValueOfConditional(string header)
        {
            if (!IsEmpty())
            {
                var x = GetArgumentAtPosition(IndexOfConditional(header));
                var xx = x.Split(':');
                var val = xx[1];
                return val;
            }
            return "";
        }
        /// <summary>
        /// Gets the value of the valued argument.
        /// -key=value;
        /// </summary>
        /// <param name="key">The key of the valued argument.</param>
        /// <returns>The value of the valued.</returns>
        public string ValueOfValued(string key)
        {
            if (!IsEmpty())
            {
                var x = GetArgumentAtPosition(IndexOfValued(key));
                var xx = x.Split('=');
                var val = xx[1];
                return val;
            }
            return "";
        }

        #endregion

        public class Variable
        {
            private char variableHeader = '$';
            private string variableValue = "null";

            public Variable(char header, string value)
            {
                variableHeader = header;
                variableValue = value;
            }

            public char GetVariableHeader() => variableHeader;
            public string GetVariableValue() => variableValue;

            public override string ToString()
            {
                return $"{GetVariableHeader()}{GetVariableValue()}";
            }
        }
    }

    public abstract class Command
    {
        private CommandInvoker invoker;

        internal void SetInvoker(CommandInvoker invoker)
            => this.invoker = invoker;

        internal void UnsetInvoker() => invoker = null;

        /// <summary>
        /// Refers to the primary name of the command.
        /// </summary>
        /// <returns>The primary name.</returns>
        public abstract string Name();
        /// <summary>
        /// Refers to the description of the command.
        /// </summary>
        /// <returns>The description.</returns>
        public abstract string Description();
        /// <summary>
        /// Refers to the secondary names of the command.
        /// </summary>
        /// <returns>The secondary names.</returns>
        public abstract string[] Aliases();
        /// <summary>
        /// Refers to the usage (syntax) of the command.
        /// </summary>
        /// <returns>The command syntax.</returns>
        public abstract string Syntax();
        /// <summary>
        /// Refers to the version of the command.
        /// </summary>
        /// <returns>The version.</returns>
        public abstract CommandVersion Version();
        /// <summary>
        /// The copyright of the command.
        /// </summary>
        /// <returns>The copyright.</returns>
        public abstract CommandCopyright Copyright();

        /// <summary>
        /// Tells the command invoker to throw a syntax error.
        /// </summary>
        public bool ThrowSyntax { get; set; } = false;

        #region Invoke

        /// <summary>
        /// This method is ran when the command is invoked by the system or the user.
        /// </summary>
        /// <param name="invoker">The invoker class that initalized the command.</param>
        /// <param name="args">The arguments originally passed to the command.</param>
        public abstract void OnInvoke(CommandInvoker invoker, CommandArguments args);

        #endregion

        #region Command Terminated

        /// <summary>
        /// Runs when the command was terminated by the user or the parent code/system.
        /// -NOT YET IMPLEMENTED!
        /// </summary>
        /// <param name="invoker">The invoker that initialized the command.</param>
        /// <param name="args">The arguments that where originally passed to the command.</param>
        public virtual void OnTerminate(CommandInvoker invoker, CommandArguments args) { }

        #endregion

        #region Command Functions

        //Tells the parent invoker to terminate the current/existing command.
        /// <summary>
        /// Terminates a running command. -NOT YET IMPLEMENTED!
        /// </summary>
        public void Terminate()
        {

        }

        #endregion
    }
}

namespace WMCommandFramework.Utilities
{
    public static class ClassExtentions
    {
        public static Returnable<T> GetReturnable<T>(this List<T> list)
        {
            return new Returnable<T>(list);
        }

        public static Returnable<T> Skip<T>(this T[] ts, int count)
        {
            List<T> tsx = new List<T>();
            //Firstly add to the 'tsx' list.
            foreach (T tx in ts)
                tsx.Add(tx);
            //Remove everything up until the 'count' value.
            int i = 0;
            while (true)
            {
                if (i == count) break;
                tsx.RemoveAt(i);
                i++;
            }
            //Finally, return the new list as a 'Returnable'.
            return tsx.GetReturnable();
        }

        public static string ToString<T>(this List<T> ts, char splitValue = ' ')
        {
            string s = "";
            foreach (T tx in ts)
            {
                var sx = tx.ToString();
                if (s == "" || s == null)
                    s = sx;
                else
                    s += $"{splitValue}{sx}";
            }
            return s;
        }

        public static string ToString<T>(this T[] ts, char splitValue = ' ')
        {
            string s = "";
            foreach (T tx in ts)
            {
                var sx = tx.ToString();
                if (s == "" || s == null)
                    s = sx;
                else
                    s += $"{splitValue}{sx}";
            }
            return s;
        }

        public static string ToString<T>(Returnable<T> returnable, char splitValue = ' ')
        {
            return ToString(returnable.ToArray(), splitValue);
        }
    }
}