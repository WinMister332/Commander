using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework
{
    public interface Command
    {
        /// <summary>
        /// The default name of the command!
        /// </summary>
        /// <returns>The name of the command.</returns>
        string CommandName();
        /// <summary>
        /// The description of the command that will display in help.
        /// </summary>
        /// <returns>The description of the command.</returns>
        string CommandDesc();
        /// <summary>
        /// The syntax of the command.
        /// Note: Add syntax after the command as the command that was inputted is placed before the syntax.
        /// </summary>
        /// <returns>The syntax of the command.</returns>
        string CommandSynt();
        /// <summary>
        /// Alliases of the command. If this name or value is typed the same command will also run.
        /// </summary>
        /// <returns>Allieses for the command.</returns>
        string[] CommandAliases();
        /// <summary>
        /// The version of the command or object associated with the command.
        /// </summary>
        /// <returns>The current CommandVersion if the --version argument was found in the argument array.</returns>
        CommandVersion CommandVersion();
        /// <summary>
        /// Code that is ran when the command is invoked by the parent command invoker.
        /// </summary>
        /// <param name="invoker">The invoker that invoked the command.</param>
        /// <param name="args">The argument that where processed.</param>
        void OnCommandInvoked(CommandInvoker invoker, CommandArgs args);
    }
}
