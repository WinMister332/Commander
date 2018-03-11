# CommandFramework
CommandFramework is a simple command parser/invoker built in C# for the COSMOS operating system library.
The library was built to work with COSMOS/.NET Core, however, you can use this library in any application even console applications.

### Use with COSMOS
In the COSMOS kernel class implement the CommandProcessor class.
```CSharp
using System;
using Sys = Cosmos.System;
//Import the CommandFramework.
using WMCommandFramework;

public Kernel : Sys.Kernel
{
  //Add the command processor class.
  private static ComamndProcessor processor;
  
  protected override void BeforeRun()
  {
    if (processor == null) processor = new CommandProcessor();
    CommandUtils.CurrentToken = "CommandFramework"; //Adds the "CommandFramework" string in front of the command input.
    CommandUtils.DebugMode = true; //Shows any errors that was produced by the *CommandFramework* if any.
    
    //Register Commands Below.
    processor.GetInvoker().AddCommand(new ExampleCommand()); //Registers the command created below.
  }
  
  protected override void Run()
  {
    //Don't need to add a while loop with COSMOS as the Run method loops.
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
            return new string[] { "print", "return" };
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

partial class Program
{
  static void Main(string[] args)
  {
    new Program().Start();
  }
  
  //Add the CommandProcessor class.
  private CommandProcessor processor;
  //Add the Boolean that tells the Program to exit.
  internal static bool canExit = false;
  
  public Program()
  {
    if (processor == null) processor = new CommandProcessor();
    CommandUtils.CurrentToken = "CommandFramework"; //Adds the "CommandFramework" string in front of the command input.
    CommandUtils.DebugMode = true; //Shows any errors that was produced by the *CommandFramework* if any.
    
    //Register Commands Below.
    processor.GetInvoker().AddCommand(new ExampleCommand()); //Registers the command created below.
    processor.GetInvoker().AddCommand(new ExitCommand()); //Registers the ExitCommand.
  }
  
  public void Start()
  {
    //In non-COSMOS applications that doesn't loop you need to add a while loop.
    while (true)
    {
      if (canExit) break;
      processor.Process();
    }
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
            return new string[] { "print", "return" };
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
When implementing the CommandProcessor into your own application it's recommended you add a close command so you can freely close the application.
```CSharp
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Commands
{
    public class ExitCommand : Command
    {
        public override string[] CommandAliases()
        {
            return new string[] { "close", "stop", "terminate", "quit", "escape", "end" };
        }

        public override string CommandDesc()
        {
            return "Closes the application.";
        }

        public override string CommandName()
        {
            return "exit";
        }

        public override string CommandSynt()
        {
            return "";
        }

        public override CommandVersion CommandVersion()
        {
            return new WMCommandFramework.CommandVersion(1,0,1,"b");
        }

        public override void OnCommandInvoked(CommandInvoker invoker, CommandArgs args)
        {
            CommandUtils.AllowExit = true;
        }
    }
}
```
