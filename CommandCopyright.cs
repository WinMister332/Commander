using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    public class CommandCopyright
    {
        private string developer = "";
        private int yearDeveloped = 2019;

        public CommandCopyright(string developer)
        {
            if (!(developer == null || developer == ""))
                this.developer = developer;
            else
                this.developer = "Commander";

            yearDeveloped = DateTime.Now.Year;
        }

        public CommandCopyright(string developer, int year)
        {
            if (!(developer == null || developer == ""))
                this.developer = developer;
            else
                this.developer = "Commander";

            if (year <= 1980)
                yearDeveloped = DateTime.Now.Year;
            else
                yearDeveloped = year;
        }

        public override string ToString()
        {
            if (DateTime.Now.Year > yearDeveloped)
                return $"Copyright © {yearDeveloped}-{DateTime.Now.Year} {developer}, All Rights Reserved.";
            else
                return $"Copyright © {yearDeveloped} {developer}, All Rights Reserved.";
        }

        public static CommandCopyright VanrosCopyright => new CommandCopyright("Vanros Corperation", 2018);
        public static CommandCopyright EMPTY => null;
    }
}
