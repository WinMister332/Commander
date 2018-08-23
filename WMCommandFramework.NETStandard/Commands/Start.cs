using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.NETStandard.Commands
{
    public class Start : Command
    {
        public override string[] Aliases()
        {
            return new string[] { "run"  };
        }

        public override string Description()
        {
            return "Starts an application or program outside the console.";
        }

        public override void Invoke(CommandInvoker invoker, CommandArgs args)
        {
            //Run.
            var path = args.GetArgAtPosition(0);
            path.Replace('\\', System.IO.Path.DirectorySeparatorChar);
            path.Replace('/', System.IO.Path.DirectorySeparatorChar);
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = path;
            process.StartInfo.CreateNoWindow = false;
            process.Start();
        }

        public override string Name()
        {
            return "start";
        }

        public override string Syntax()
        {
            return "<application path>";
        }

        public override CommandVersion Version()
        {
            return new CommandVersion(1, 0, 3, "b");
        }
    }
}
