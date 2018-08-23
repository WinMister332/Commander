using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.COSMOS
{
    public class CommandUtils
    {
        private static string _osName = "";
        private static bool _debug = false;
        private static InputMessage inputMessage = null;
        private static ApplicationVersion _applicationVersion = ApplicationVersion.CommandFrameworkVersion;
        private static TerminalUser _currentUser = null;

        /// <summary>
        /// Whether debug information should be printed to the current terminal.
        /// </summary>
        public static bool DebugMode
        {
            get => _debug;
            set => _debug = value;
        }

        /// <summary>
        /// The message to display in every command input prompt.
        /// </summary>
        public static InputMessage InputMessage
        {
            get => inputMessage;
            set
            {
                if (value != null) inputMessage = value;
            }
        }

        /// <summary>
        /// The version of the current application.
        /// </summary>
        public static ApplicationVersion Version
        {
            get => _applicationVersion;
            set => _applicationVersion = value;
        }

        /// <summary>
        /// The name of the current operating system.
        /// </summary>
        public static string OSName
        {
            get => _osName;
            set => _osName = value;
        }

        /// <summary>
        /// The currently logged in user that was set by the login prompt.
        /// </summary>
        public static TerminalUser CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }
    }
}
