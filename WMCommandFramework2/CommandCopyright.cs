using System;
namespace WMCommandFramework
{
    public sealed class CommandCopyright
    {
        private string developer = "";
        private short yearDeveloped = 2019;

        public CommandCopyright(string developer, short developmentYear = 2019)
        {
            this.developer = developer;
            yearDeveloped = developmentYear;
        }

        public override string ToString()
        {
            if (!(developer == null || developer == ""))
                return $"Copyright (C) {formatDate()} {developer}, All Rights Reserved.";
            return $"Copyright (C) {formatDate()}, All Rights Reserved.";
        }

        private string formatDate()
        {
            if (yearDeveloped < currentYear)
                return $"{yearDeveloped}-{currentYear}";
            else if (yearDeveloped == currentYear)
                return $"{currentYear}";
            else if (yearDeveloped > currentYear)
                return $"{currentYear}";
            else
                return "";
        }

        private readonly short currentYear = (short)DateTime.Today.Year;

        public static CommandCopyright EMPTY => new CommandCopyright("", 2018);

        internal static CommandCopyright VANROS => new CommandCopyright("Vanros Corperation");
    }
}
