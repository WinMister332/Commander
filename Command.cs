using System;
using System.Collections.Generic;
using System.Text;
using CMD = Commander;

namespace Commander
{
    public abstract class Command
    {
        public abstract string CommandName();
        public abstract string CommandDescription();
        public abstract string CommandSyntax();
        public abstract string[] CommandAliases();
        public abstract CommandVersion CommandVersion();
        public virtual CommandCopyright CommandCopyright() { return CMD.CommandCopyright.EMPTY; }
        public abstract void OnInvoke(CommandInvoker invoker, CommandArguments args);
        public bool ThrowSyntax { get; set; } = false;
    }

    //**setDebug = true
}
