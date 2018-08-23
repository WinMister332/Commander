using System;
using CMD = WMCommandFramework.NETStandard;
using CMOS = WMCommandFramework.COSMOS;

namespace TestApplication
{
    /// <summary>
    /// The main class for debuging various .NETStandard Functions.
    /// Do NOT use with your project.
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }

        private CMOS.CommandProcessor cmosProcessor;
        private CMD.CommandProcessor cmdProcessor;
        private CMOS.TerminalUsers users;

        private bool useCosmos = false;

        public Program()
        {
            System.IO.Directory.SetCurrentDirectory("C:\\");
            cmdProcessor = new CMD.CommandProcessor();
            cmosProcessor = new CMOS.CommandProcessor();
            cmdProcessor.Message = new CMD.InputMessage(
                new CMD.MessageText(ConsoleColor.Cyan, "$Administrator"),
                new CMD.MessageText(ConsoleColor.Blue, "@Desmin"),
                CMD.MessageText.WhiteSpace(),
                new CMD.MessageText(ConsoleColor.Green, System.IO.Directory.GetCurrentDirectory())
                );
            cmosProcessor.Message = new CMOS.InputMessage(
                new CMOS.MessageText(ConsoleColor.Cyan, "$Administrator"),
                new CMOS.MessageText(ConsoleColor.Blue, "@DuskOS"),
                CMOS.MessageText.WhiteSpace(),
                new CMOS.MessageText(ConsoleColor.Green, System.IO.Directory.GetCurrentDirectory())
                );
            cmosProcessor.Debug = true;
            cmdProcessor.Debug = true;
        }

        private void Run()
        {
            if (useCosmos)
            {
                while (true)
                {
                    cmosProcessor.Process();
                }
            }
            else
            {
                cmdProcessor.Process();
            }
        }
    }
}
