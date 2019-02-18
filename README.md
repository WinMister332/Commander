# WMCommandFramework
WMCommandFramework is a simple command framework and library designed for creating, managing, and processing commands for .NETStandard, .NETCore, and WinForms-based applications.

## GitHub
[![GitHub Last Commit (master)](https://img.shields.io/github/last-commit/WinMister332/WMCommandFramework/master.svg?style=popout-square)](https://github.com/WinMister332/WMCommandFramework/graphs/commit-activity)
[![GitHub issues](https://img.shields.io/github/issues/winmister332/wmcommandframework.svg?style=popout-square)](https://github.com/WinMister332/WMCommandFramework/issues)
[![GitHub forks](https://img.shields.io/github/forks/winmister332/wmcommandframework.svg?style=popout-square)](https://github.com/WinMister332/WMCommandFramework/network/members)
[![GitHub top language](https://img.shields.io/github/languages/top/winmister332/wmcommandframework.svg?style=popout-square)](https://github.com/WinMister332/WMCommandFramework/search?l=c%23)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/winmister332/wmcommandframework.svg?style=popout-square)](https://github.com/WinMister332/WMCommandFramework/pulls?q=is%3Aopen+is%3Apr)
[![license](https://img.shields.io/github/license/winmister332/wmcommandframework.svg?style=popout-square)](https://github.com/WinMister332/WMCommandFramework/blob/master/LICENSE)

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
