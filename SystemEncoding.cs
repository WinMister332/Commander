using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    public class SystemEncoding
    {
        private int code = 0000;
        private string name = "";
        private string description = "";

        internal SystemEncoding(string name, string desc, int code)
        {
            this.name = name;
            description = desc;
            this.code = code;
        }

        public int GetCodePage() => code;
        public string GetName() => name;
        public string GetDescription() => description;

    }
}
