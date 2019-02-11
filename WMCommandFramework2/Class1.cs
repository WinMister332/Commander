﻿using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using WMCommandFramework.Commands;
using WMCommandFramework.Utilities;

namespace WMCommandFramework
{
    public static class CommandUtilities
    {
        public static bool DebugMode { get; set; } = false;
        public static ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Blue;
        public static AppName ApplicationName { get; set; } = AppName.WMCommandFramework;
        public static InputMessage Message { get; set; } = InputMessage.EMPTY;
    }

    public sealed class CommandProcessor
    {
        public static CommandProcessor INSTANCE = null;
        private CommandInvoker invoker = null;

        public CommandProcessor()
        {
            INSTANCE = this;
            invoker = new CommandInvoker();
            invoker.SetProcessor(this);
        }

        public CommandProcessor(CommandInvoker invoker)
        {
            INSTANCE = this;
            if (!(invoker == null))
            {
                invoker.SetProcessor(this);
                this.invoker = invoker;
            }
        }

        public CommandInvoker GetInvoker() => invoker;

        private ConsoleColor currentFGColor = ConsoleColor.White;
        private ConsoleColor currentBGColor = ConsoleColor.Blue;

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

        public AppName ApplicationName
        {
            get => CommandUtilities.ApplicationName;
            set => CommandUtilities.ApplicationName = value;
        }

        public InputMessage Message
        {
            get => CommandUtilities.Message;
            set => CommandUtilities.Message = value;
        }

        private bool closeProcessor = false;

        public bool ExitProcessor
        {
            get => closeProcessor;
            set => closeProcessor = value;
        }

        private bool displayEcho = true;
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
                }
                else
                {
                    if (args.GetCommandAtPosition(0) == $"--version" || args.GetCommandAtPosition(0) == $"-ver")
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

        public CommandArguments(string[] strx)
        {
            args = new List<string>(strx.Length);
            foreach (string s in strx)
                args.Add(s);
        }

        public CommandArguments(List<string> strx)
        {
            args = strx;
        }

        #region General Functions

        public int Count => args.Count;

        public bool IsEmpty()
        {
            if (Count <= 0) return true;
            return false;
        }

        public string GetCommandAtPosition(int position)
        {
            if (!(IsEmpty()))
                return args[position];
            return "";
        }

        #endregion
    }

    public abstract class Command
    {
        private CommandInvoker invoker;

        internal void SetInvoker(CommandInvoker invoker)
            => this.invoker = invoker;

        internal void UnsetInvoker() => invoker = null;

        public abstract string Name();
        public abstract string Description();
        public abstract string[] Aliases();
        public abstract CommandVersion Version();
        public abstract CommandCopyright Copyright();

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