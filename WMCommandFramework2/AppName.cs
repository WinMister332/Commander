using System;
namespace WMCommandFramework
{
    public class AppName
    {
        private string name;
        private CommandCopyright copyright;
        private CommandVersion version;

        public AppName(string name, CommandVersion version)
        {
            this.name = name;
            this.version = version;
            copyright = CommandCopyright.EMPTY;
        }

        public AppName(string name, CommandVersion version, CommandCopyright copyright)
        {
            this.copyright = copyright;
            this.version = version;
            this.name = name;
        }

        public string GetName() => name;
        public CommandCopyright GetCopyright() => copyright;
        public CommandVersion GetVersion() => version;

        public static AppName WMCommandFramework => new AppName(
            "WMCommandFramework",
            new CommandVersion(1,2,0, "BETA"),
            CommandCopyright.VANROS
            );
    }
}
