using System;
using CMD = WMCommandFramework.NETStandard;

namespace TestApplication
{
    class Program
    {
        private CMD.CommandProcessor processor = null;

        public Program()
        {
            processor = new CMD.CommandProcessor();
            processor.Debug = true;
            processor.Version = new CMD.ApplicationVersion("Test Application", new CMD.CommandCopyright("Vanros Corperation"), new CMD.CommandVersion(1,0));
            processor.Message = new CMD.InputMessage[] { new CMD.InputMessage(ConsoleColor.Cyan, "Enter Command"), CMD.InputMessage.ResetColor};

        }
    }
}
