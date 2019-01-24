# WMCommandFramework
The WMCommandFramework is a simple command framework built in C# for COSMOS and .NETStandard applications.

## Nuget - WMCommandFramework.COSMOS
[![NuGet](https://img.shields.io/nuget/dt/WMCommandFramework.COSMOS.svg?style=for-the-badge)](https://www.nuget.org/packages/WMCommandFramework.COSMOS/#)
[![NuGet](https://img.shields.io/nuget/v/WMCommandFramework.COSMOS.svg?style=for-the-badge)](https://www.nuget.org/packages/WMCommandFramework.COSMOS/)

## Nuget - WMCommandFramework.NETStandard
[![NuGet](https://img.shields.io/nuget/dt/WMCommandFramework.NETStandard.svg?style=for-the-badge)](https://www.nuget.org/packages/WMCommandFramework.NETStandard/)
[![NuGet](https://img.shields.io/nuget/v/WMCommandFramework.NETStandard.svg?style=for-the-badge)](https://www.nuget.org/packages/WMCommandFramework.NETStandard/)

## GitHub - WMCommandFramework
[![GitHub issues](https://img.shields.io/github/issues/winmister332/wmcommandframework.svg?style=for-the-badge)](https://github.com/WinMister332/WMCommandFramework/issues)
[![GitHub forks](https://img.shields.io/github/forks/winmister332/wmcommandframework.svg?style=for-the-badge&label=Fork)](https://github.com/WinMister332/WMCommandFramework/network/members)
[![GitHub top language](https://img.shields.io/github/languages/top/winmister332/wmcommandframework.svg?style=for-the-badge)](https://github.com/WinMister332/WMCommandFramework/search?l=c%23)
[![license](https://img.shields.io/github/license/winmister332/wmcommandframework.svg?style=for-the-badge)](https://github.com/WinMister332/WMCommandFramework/blob/master/LICENSE)

### Use with COSMOS
In the COSMOS kernel class implement the CommandProcessor class.
```CSharp
using System;
using Sys = Cosmos.System;
//Import the CommandFramework.
using CMD = WMCommandFramework.COSMOS;

public Kernel : Sys.Kernel
{
  //Add the command processor class.
  private static CMD.ComamndProcessor processor;
  
  protected override void BeforeRun()
  {
    if (processor == null) processor = new CMD.CommandProcessor();
    //Used for displaying version information when a lone --version is used.
    processor.Version = new CMD.ApplicationVersion("WMCommandFramework Example OS", new CMD.CommandCopyright("Vanros Corperation"), new CMD.CommandVersion(1, 1, 0, "STABLE"));
    //Replaces `CommandUtil.CurrentToken`.
    processor.Message = new CMD.InputMessage[] { new CMD.InputMessage(ConsoleColor.Cyan, "$administrator"), new CMD.InputMessage(ConsoleColor.Green, "@WMCommandFrameworkOS"), CMD.InputMessage.NewLine };
    //Register Commands:
    processor.GetInvoker().AddCommand(new ExampleCommand());
    //Loop login prompt until the user logs in.
    while (true)
    {
      var access = processor.LoginPrompt("administrator", "password");
      if (access) break;
    }
  }
  
  protected override void Run()
  {
    processor.Process();
  }
}
```
Once the CommandProcessor was implemented into the COSMOS Kernel you need to create the command.
Create a new class that implements the abstract Command class.
```CSharp
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Commands
{
    public class ExampleCommand : Command
    {
        public override string[] CommandAliases()
        {
            return new string[0]; //Means there's no aliases.
        }

        public override string CommandDesc()
        {
            return "Prints the specified information to the console.";
        }

        public override string CommandName()
        {
            return "example";
        }

        public override string CommandSynt()
        {
            return "<message>";
        }

        public override CommandVersion CommandVersion()
        {
            return new WMCommandFramework.CommandVersion(1,0,1,"b");
        }

        public override void OnCommandInvoked(CommandInvoker invoker, CommandArgs args)
        {
            if (args.IsEmpty()) throw new Exceptions.SyntaxException("The \"message\" argument cannot be null or empty.");
            else
            {
                Console.WriteLine($"{args.GetArgAtPosition(0)}");
            }
        }
    }
}
```
After the class containing the command was created and was implemented into the CommandInvoker class it should work in your OS. Just be sure to register your command with the command invoker if you haven't done so already, but if you followed this README then the command would have been registered before the command was created.

### Use with non-COSMOS applications

Implememnt the CommandProcessor into your MAIN Program class.
```CSharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD = WMCommandFramework.NETStandard;

partial class Program
{
  static void Main(string[] args)
  {
    new Program().Start();
  }
  
  //Add the CommandProcessor class.
  private CMD.CommandProcessor processor;
  
  public Program()
  {
    if (processor == null) processor = new CMD.CommandProcessor();
    //Used for displaying version information when a lone --version is used.
    processor.Version = new CMD.ApplicationVersion("WMCommandFramework Example OS", new CMD.CommandCopyright("Vanros Corperation"), new CMD.CommandVersion(1, 1, 0, "STABLE"));
    //Replaces `CommandUtil.CurrentToken`.
    processor.Message = new CMD.InputMessage[] { new CMD.InputMessage(ConsoleColor.Cyan, "$administrator"), new CMD.InputMessage(ConsoleColor.Green, "@WMCommandFrameworkOS"), CMD.InputMessage.NewLine };
    //Register Commands:
    processor.GetInvoker().AddCommand(new ExampleCommand());
    //The exit command is now included.
  }
  
  public void Start()
  {
    //The WHILE/Close loop is now included in the CommandProcessor class.
    processor.Process();
  }
  
  public CMD.CommandProcessor GetProcessor()
  {
    return processor;
  }
}
```
Once the CommandProcessor was implemented into your MAIN Program class and registered in CommandProcessor you can create the command.}
```CSharp
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Commands
{
    public class ExampleCommand : Command
    {
        public override string[] CommandAliases()
        {
            return new string[0]; //Means there's no aliases.
        }

        public override string CommandDesc()
        {
            return "Prints the specified information to the console.";
        }

        public override string CommandName()
        {
            return "example";
        }

        public override string CommandSynt()
        {
            return "<message>";
        }

        public override CommandVersion CommandVersion()
        {
            return new WMCommandFramework.CommandVersion(1,0,1,"b");
        }

        public override void OnCommandInvoked(CommandInvoker invoker, CommandArgs args)
        {
            if (args.IsEmpty()) throw new Exceptions.SyntaxException("The \"message\" argument cannot be null or empty.");
            else
            {
                Console.WriteLine($"{args.GetArgAtPosition(0)}");
            }
        }
    }
}
```