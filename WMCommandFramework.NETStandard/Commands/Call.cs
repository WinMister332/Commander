using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.NETStandard.Commands
{
    public class Call : Command
    {
        public override string[] Aliases()
        {
            return new string[] { "embed" };
        }

        public override string Description()
        {
            return "Starts an application inside the console.";
        }

        public override void Invoke(CommandInvoker invoker, CommandArgs args)
        {
            //Call
            var path = args.GetArgAtPosition(0);
            path.Replace('\\', System.IO.Path.DirectorySeparatorChar);
            path.Replace('/', System.IO.Path.DirectorySeparatorChar);
            var process = new System.Diagnostics.Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
        }

        public override string Name()
        {
            return "call";
        }

        public override string Syntax()
        {
            return "<path>";
        }

        public override CommandVersion Version()
        {
            return new CommandVersion(1, 0, 3, "b");
        }
    }
}
