# CommandFramework
CommandFramework is a simple command parser/invoker built in C# for the COSMOS operating system library.
The library was built to work with COSMOS/.NET Core, however, you can use this library in any application even console applications.

### Use with COSMOS
Once this library is referenced in the project, you must initialize it.
Be sure your class is the equivalent to that of the class below.
```CSharp
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Sys = Cosmos.System;
using CMD = WMCommandFramework.COSMOS;

public class Kernel : Sys.Kernel
{
    //The command processor.
    private static CMD.CommandProcessor processor;

    protected override void BeforeRun()
    {
        //Initializes the CommandProcessor.
        processor = new CMD.CommandProcessor();
        processor.Message = new CMOS.InputMessage(
                new CMOS.MessageText(ConsoleColor.Cyan, "$Administrator"),
                new CMOS.MessageText(ConsoleColor.Blue, "@DuskOS"),
                CMOS.MessageText.WhiteSpace(),
                new CMOS.MessageText(ConsoleColor.Green, System.IO.Directory.GetCurrentDirectory())
        );
        
        //Enable debug output from the command processor.
        processor.Debug = true;
        //The version of the current application.
        processor.Version = new CMD.ApplicationVersion("DuskOS", new CMD.CommandCopyright("WinMister332"), new CMD.CommandVersion(1,0,0,0, "BETA"));
    }
    
    protected override void Run()
    {
        //Process command input.
        processor.Process();
    }
    
    public CMD.CommandProcessor GetProcessor()
    {
        return processor;
    }
}
```
Once the command processor is initialized we need to register new commands or even override internal or existing commands.
Create a new class for your command. We'll create a simple 'Echo' command.
Be sure the command implements the 'Command' class.
```CSharp
public class EchoCommand : CMD.Command
{
    public override string[] Aliases()
    {
        throw new NotImplementedException();
    }

    public override string Description()
    {
         throw new NotImplementedException();
    }

    public override void Invoke(CMD.CommandInvoker invoker, CMD.CommandArgs args)
    {
        throw new NotImplementedException();
    }

    public override string Name()
    {
        throw new NotImplementedException();
    }

    public override string Syntax()
    {
        throw new NotImplementedException();
    }

    public override CMD.CommandVersion Version()
    {
        throw new NotImplementedException();
    }
)
```
When the 'Command' class is implemented it will throw all the values which could be a problem when the command processor calls the various functions.
Replace the default code in the functions with the code required for your command.
```CSharp
    public override string[] Aliases()
    {
        return new string[] { "print" };
    }

    public override string Description()
    {
         return "Prints a message to the terminal.";
    }

    //Runs when the command is processed then invoked.
    public override void Invoke(CMD.CommandInvoker invoker, CMD.CommandArgs args)
    {
        if (!(arg.IsEmpty()))
        {
            var message = args.GetArgAtPosition(0);
            Console.WriteLine(message);
            return; //Optional return - Safety to return to the previous function.
        }
    }

    public override string Name()
    {
        return "echo";
    }

    public override string Syntax()
    {
        return "<message>";
    }

    public override CMD.CommandVersion Version()
    {
        return new CMD.CommandVersion(1,0,0,0, "BETA");
    }
```
Since we have an 'Echo' command built-in to the command processor we need to override the internal command. At this current point in time, there's no function or method to easially do such a thing, so we have to manually override the internal command with our custom one.
As of currently the only way to do this is by getting the primary class of the internal command, to do this you need to do so VIA the 'processor' class.
```CSharp
protected override void BeforeRun()
{
    var invoker = processor.GetInvoker(); //This is required to register or unregister commands. But, it'll also help us remove a command by-name.
    var xcmd = invoker.GetCommandByName(); //If you don't know the exact name of a command and only know an alias you can use 'GetCommandByAlias()' if you don't know whether the command is the name or alias you can use 'GetCommand()'.
    //Check that the command is NOT null.
    if (xcmd != null)
    {
        //Remove the internal 'echo' command.
        invoker.RemoveCommand(xcmd);
    }
}
```
The above will remove the internal echo command allowing us to add our new one.
To add a command add the following code to your Kernel 'BeforeRun()' function.
`invoker.AddCommand(new Echo()); //This'll add our new command to the invoker.`
