using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    public class CommandVersion
    {
        private int[] verx = new int[0];
        private string tag = "";

        public CommandVersion(int major, int minor, int build, int revision, string tag = "")
        {
            verx = new int[] { major, minor, build, revision };
            this.tag = tag;
        }

        public CommandVersion(int minor, int build, int revision, string tag = "")
        {
            verx = new int[] { minor, build, revision };
            this.tag = tag;
        }

        public CommandVersion(int build, int revision, string tag = "")
        {
            verx = new int[] { build, revision };
            this.tag = tag;
        }

        public override string ToString()
        {
            if (tag != null && tag != "")
            {
                if (verx.Length == 4)
                    return $"{verx[0]}.{verx[1]}.{verx[2]}.{verx[3]}-{tag}";
                else if (verx.Length == 3)
                    return $"{verx[0]}.{verx[1]}.{verx[2]}-{tag}";
                else
                    return $"{verx[0]}.{verx[1]}-{tag}";
            }
            else
            {
                if (verx.Length == 4)
                    return $"{verx[0]}.{verx[1]}.{verx[2]}.{verx[3]}";
                else if (verx.Length == 3)
                    return $"{verx[0]}.{verx[1]}.{verx[2]}";
                else
                    return $"{verx[0]}.{verx[1]}";
            }
        }

        public int GetMajor()
        {
            if (verx.Length == 4)
                return verx[0];
            else
                return 0;
        }

        public int GetMinor()
        {
            if (verx.Length == 4)
                return verx[1];
            else if (verx.Length == 3)
                return verx[0];
            else
                return 0;
        }

        public int GetBuild()
        {
            if (verx.Length == 4)
                return verx[2];
            else if (verx.Length == 3)
                return verx[1];
            else if (verx.Length == 2)
                return verx[0];
            else
                return 0;
        }

        public int GetRevision()
        {
            if (verx.Length == 4)
                return verx[3];
            else if (verx.Length == 3)
                return verx[2];
            else if (verx.Length == 2)
                return verx[1];
            else
                return 0;
        }

        public string GetTag()
        {
            if (tag == "" || tag == null)
                return "";
            return tag;
        }

        public bool IsVersion(CommandVersion version)
        {
            if (this == version)
                return true;
            return false;
        }

        public static CommandVersion CommanderLatest
            => new CommandVersion(0, 2, 2, "DEV");
    }
}
