using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.NETStandard
{
    public class CommandCopyright
    {
        private string dev = "";
        private int year = -1;
        private bool used = false;

        public CommandCopyright()
        {
            dev = "";
            year = -1;
            used = false;
        }

        public CommandCopyright(string developer)
        {
            dev = developer;
            year = Year;
            used = true;
        }

        public CommandCopyright(string developer, int baseYear)
        {
            if ((baseYear <= 1975)) baseYear = 1975;
            dev = developer;
            year = baseYear;
            used = true;
        }

        public int BaseYear
        {
            get => year;
            set => year = value;
        }

        public int Year
        {
            get
            {
                var yearx = DateTime.Now.Year;
                return yearx;
            }
        }

        public override string ToString()
        {
            if (!(used) || year == -1 && (dev == "" || dev == null))
                return "";
            else
            {
                return $"Copyright (c) {YearStamp()} {dev}, All Rights Reserved!";
            }
        }

        private string YearStamp()
        {
            string stamp = "";
            if ((!(BaseYear < 1975 || Year < 1975) && (BaseYear > 0 && BaseYear >= 1975)))
                stamp = $"{BaseYear}-{Year}";
            else if ((BaseYear == Year))
                stamp = Year.ToString();
            else if ((BaseYear >= Year))
            {
                year = Year;
                stamp = year.ToString();
            }
            return stamp;
        }

        public static CommandCopyright VanrosCopyright()
        {
            var copyright = new CommandCopyright("Vanros Corperation", 2017);
            return copyright;
        }
    }
}
