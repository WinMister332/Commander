using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace WMCommandFramework
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args) => new Program().Run(args);

        private CommandProcessor processor = null;

        public Program()
        {
            //Initialize the processor.
            processor = new CommandProcessor();
            //(OPTIONAL) Setup the commands and processor.
            processor.DebugMode = true;
            processor.DefaultBackgroundColor = ConsoleColor.DarkCyan;
            processor.DefaultForegroundColor = ConsoleColor.White;
            processor.ApplicationName = new AppName("Desmin", new CommandVersion(1, 2, 1, "BETA"));
            processor.Message = new InputMessage(
                new InputMessage.Message("$Administrator", ConsoleColor.Yellow),
                new InputMessage.Message($"@{processor.ApplicationName.GetName()}", ConsoleColor.DarkCyan)
                );
            
        }

        private void Run(string[] args)
        {
            //Start the processor.
            processor.Process();
        }
    }
}

//$username@application filename