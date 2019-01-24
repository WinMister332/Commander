using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using WMCommandFramework2;
using WMCommandFramework2.Attributes;

namespace WMCommandFramework2.Commands
{
    [HiddenCommand()]
    public class CVersion : Command
    {
        public CVersion()
        {
        }

        public override string[] Aliases()
        {
            return new string[] { "--ver", "--v", "--wmver", "--appver", "--cmdver" };
        }

        public override string Copyright()
        {
            return "";
        }

        public override string Description()
        {
            return "Displays the current version.";
        }

        public override string Name()
        {
            return "--version";
        }

        public override void OnInvoke(CommandInvoker invoker, CommandArguments args)
        {
            //Do stuff.
            //Display version info.
            Console.WriteLine(
                $"~~~~~~~~~~~~~~~\n" +
            	$"{CommandUtilities.ApplicationName.GetCopyright().ToString()}" +
            	$"{CommandUtilities.ApplicationName.GetName()} Version: {CommandUtilities.ApplicationName.GetVersion().ToString()}" +
                $"~~~~~~~~~~~~~~~");
        }

        public override string Version()
        {
            return "1.0.0-RN";
        }
    }
}
