using System;
using System.Collections.Generic;

namespace WMCommandFramework
{
    public sealed class CommandUtils
    {
        private static string currentToken = "";
        /// <summary>
        /// The string of text to display in the prompt.
        /// </summary>
        public static string CurrentToken
        {
            get { return currentToken; }
            set { currentToken = value; }
        }

        private static bool debugMode = false;
        /// <summary>
        /// Shows or hides specific debug data.
        /// </summary>
        public static bool DebugMode
        {
            get { return debugMode; }
            set { debugMode = value; }
        }

        private static bool allowExit = false;
        public static bool AllowExit
        {
            get { return allowExit; }
            set { allowExit = value; }
        }
    }
    public class CommandProcessor
    {
        private CommandInvoker invoker = null;
        public CommandProcessor()
        {
            invoker = new CommandInvoker();
        }

        /// <summary>
        /// Gets the pre-set invoker that is used for registering and using commands within the command processor.
        /// </summary>
        /// <returns>CommandInvoker - The internal command invoker.</returns>
        public CommandInvoker GetInvoker()
        {
            return invoker;
        }

        /// <summary>
        /// Starts the Command Processor.
        /// </summary>
        public void Process()
        {
            try
            {
                Console.Write($"{CommandUtils.CurrentToken} > ");
                var input = Console.ReadLine();
                if (!(input == null) || !(input == "")) invoker.InvokeCommand(input);
            }
            catch (Exception ex)
            {
                if (CommandUtils.DebugMode == true)
                    Console.WriteLine($"An unknown error has occurred! Please restart your computer! Error: {ex.ToString()}");
                else
                    Console.WriteLine("An unknown error has occurred! Please restart your computer!");
            }
        }
    }
}