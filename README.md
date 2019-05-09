# WMCommandFramework
WMCommandFramework is a simple command framework and library designed for creating, managing, and processing commands for .NETStandard, .NETCore, and WinForms-based applications.

NOTE: WMCommandFramework ([WMCMDX](https://github.com/WinMister332/WMCommandFramework/tree/WMCMDX)) Will no longer be updated under this name, it will be updated as "Commander" (Command Utilities Library) instead.

### Quick Setup
Inorder to use use commands within WMCommandFramework you must setup your class to use the CommandFramework.

Firstly, import the framework to your class.
Add the following line to the 'using' section of your class.
`using CMD = WMCommandFramework`

Once you've added that line to your class, you must invoke the CommandProcessor and utilize it.
Above your constructor, add this field.
`private static CMD.CommandProcessor processor = null;`

Now we must initialize the 'processor' field before we can use it. In your constructor initialize the field.
Copy this code into your constructor to initialize the 'processor' field.
`processor = new CMD.CommandProcessor();`

Now that we've initialized the CommandProcessor you must set it up inorder to use it. We must set some properties that will be our defaults the Processor and invoker will use when creating, processing, and invoking commands.
Add the following code below the code initializing the processor field in your constructor and modify it as you need.
```CSharp
//Set the debug mode.
processor.DebugMode = true;
//Sets the echo of the input line.
//Change this to what you need for your application.
processor.Message = new CMD.InputMessage(
  new CMD.InputMessage.Message("$Administrator", Color.Yellow),
  new CMD.InputMessage.Message("$Desmin", Color.DarkAqua),
  new CMD.InputMessage.Message(" "),
  new CMD.InputMessage.Message("/", Color.Green),
  CMD.InputMessage.Message.ResetColor
);
//Sets the AppName of the current application.
//Change this to what you need for your application.
processor.ApplicationName = new CMD.AppName("Desmin", new CommnandVersion(1, 0, 0, "BETA"), new CommandCopyright("NerdHub"));
```
The code above will setup the processor using the defaults you preset for your application.
Next, you need to add the following code in your method of your class that will be invoked by your project.
`processor.Process(true);`
That code will tell the processor to begin processing any text that was passed into the terminal and continue until the exit command is passed.

For instuctions on creating commands or using the framework with COSMOS check the WIKI.
