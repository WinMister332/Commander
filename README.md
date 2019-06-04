# Commander
Commander contains useful code vital to creating a custom command-line or terminal in .NETCore, .NETStandard, WinForms/WPF, and a [COSMOS](http://www.github.com/CosmosOS/Cosmos) KERNEL. 

In COMMANDER V1 Update I will re-release the NUGET library, (Hopefully with an icon), and released along side 'Desmin', a custom windows kernel planned to support all native windows commands, AuraOS commands, DuskOS commands, and some linux commands (such as 'git').

If you'd like to help with either project, send me an email at 'vanrosservice@gmail.com' or send me a DM on Gitter (Same GitHub Name/Username).

## GitHub
[![GitHub Last Commit (master)](https://img.shields.io/github/last-commit/WinMister332/Commander/master.svg?style=popout-square)](https://github.com/WinMister332/Commander/graphs/commit-activity)
[![GitHub issues](https://img.shields.io/github/issues/winmister332/Commander.svg?style=popout-square)](https://github.com/WinMister332/Commander/issues)
[![GitHub forks](https://img.shields.io/github/forks/winmister332/Commander.svg?style=popout-square)](https://github.com/WinMister332/Commander/network/members)
[![GitHub top language](https://img.shields.io/github/languages/top/winmister332/Commander.svg?style=popout-square)](https://github.com/WinMister332/Commander/search?l=c%23)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/winmister332/Commander.svg?style=popout-square)](https://github.com/WinMister332/Commander/pulls?q=is%3Aopen+is%3Apr)
[![license](https://img.shields.io/github/license/winmister332/Commander.svg?style=popout-square)](https://github.com/WinMister332/Commander/blob/master/LICENSE)

### Quick Setup
Inorder to use use commands within Commander you must setup your class to use the CommandFramework.

Firstly, import the framework to your class.
Add the following line to the 'using' section of your class.
`using CMD = Commander`

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
