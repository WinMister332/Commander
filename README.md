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
  
  protected void BeforeRun()
  {
    if (processor == null) processor = new CommandProcessor();
    CommandUtils.CurrentToken = "CommandFramework"; //Adds the "CommandFramework" string in front of the command input.
    CommandUtils.DebugMode = true; //Shows any errors that was produced by the *CommandFramework* if any.
    
    //Register Commands Below.
    processor.GetInvoker().AddCommand(new ExampleCommand()); //Registers the command created below.
  }
  
  public void Run()
  {
    //Don't need to add a while loop with COSMOS as the Run method loops.
    processor.Process();
  }
}
```
Once the CommandProcessor was implemented into the COSMOS Kernel you need to create the command.
Create a new class and implement the Command interface to the class.
```CSharp
using System;
using System.Text;
using System.Collections.Generic;

public class ExampleCommand : Command
{
  public string CommandName()
  {
    return "example"; //The primary name of the command.
  }
  public string CommandDesc()
  {
    return "Displays and example prompt based on the arguments.";
  }
  public string CommandSynt()
  {
    return "[args]";
  }
  public string[] CommandAliases()
  {
    return new String[] {"exmple", "ex"};
  }
  public void OnCommandInvoked(CommandInvoker invoker, CommandArgs args)
  {
    //When the command is processed this method is invoked.
    if (args.isEmpty())
    {
      new Exception("The command must have arguments.");
    }
    else if (args.ContainsSwitch("a"))
    {
      Console.WriteLine("The argument -a was found.");
    }
    else if (args.ContainsSwitch("b"))
    {
      Console.WriteLine("The argument -b was found.");
    }
    else if (arg.ContainsSwitch("a") && arg.Contains("b"))
    {
      Console.WriteLine("The arguments -a and -b was found!");
    }
    else if (arg.ContainsSegmentedSwitch("c"))
    {
      var segmentedArgValue = arg.GetSegmentedSwitchValue("c");
      Console.WriteLine($"A segmented switch was found in the string array. It's value is: {segmentedArgValue}!");
    }
  }
}
```
After the class containing the command was created and was implemented into the CommandInvoker class it should work in your OS.

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
using System.Text;
using System.Collections.Generic;

public class ExampleCommand : Command
{
  public string CommandName()
  {
    return "example"; //The primary name of the command.
  }
  public string CommandDesc()
  {
    return "Displays and example prompt based on the arguments.";
  }
  public string CommandSynt()
  {
    return "[args]";
  }
  public string[] CommandAliases()
  {
    return new String[] {"exmple", "ex"};
  }
  public void OnCommandInvoked(CommandInvoker invoker, CommandArgs args)
  {
    //When the command is processed this method is invoked.
    if (args.isEmpty())
    {
      new Exception("The command must have arguments.");
    }
    else if (args.ContainsSwitch("a"))
    {
      Console.WriteLine("The argument -a was found.");
    }
    else if (args.ContainsSwitch("b"))
    {
      Console.WriteLine("The argument -b was found.");
    }
    else if (arg.ContainsSwitch("a") && arg.Contains("b"))
    {
      Console.WriteLine("The arguments -a and -b was found!");
    }
    else if (arg.ContainsSegmentedSwitch("c"))
    {
      var segmentedArgValue = arg.GetSegmentedSwitchValue("c");
      Console.WriteLine($"A segmented switch was found in the string array. It's value is: {segmentedArgValue}!");
    }
  }
}
```
When implementing the CommandProcessor into your own application it's recommended you add a close command so you can freely close the application.
```CSharp
using System;
using System.Text;
using System.Collections.Generic;

public class ExitCommand : Command
{
  public string CommandName()
  {
    return "exit";
  }
  public string CommandDesc()
  {
    return "Closes the application.";
  }
  public string CommandSynt()
  {
    return "";
  }
  public string[] CommandAliases()
  {
    return new String[] {"close", "terminate", "quit", "stop", "end"};
  }
  public void OnCommandInvoked(CommandInvoker invoker, CommandArgs args)
  {
    Program.canExit = true;
    return;
  }
}
```
