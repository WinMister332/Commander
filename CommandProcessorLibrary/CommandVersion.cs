using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework
{
    public class CommandVersion
    {
        private int major = -1;
        private int minor = -1;
        private int build = -1;
        private int revision = 0;
        private string tag = "";

        public CommandVersion(int major, int minor, int build, int revision, string tag = "")
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.revision = revision;
            this.tag = tag;
        }

        public CommandVersion(int minor, int build, int revision, string tag = "")
        {
            new CommandVersion(-1, minor, build, revision, tag);
        }

        public CommandVersion(int build, int revision, string tag = "")
        {
            new CommandVersion(-1, -1, build, revision, tag);
        }

        public bool IsVersion(CommandVersion version)
        {
            if (version == this) return true;
            return false;
        }

        public bool IsGreater(CommandVersion version)
        {
            if ((this.major > version.major && this.minor > version.minor && this.build > version.build && this.revision > version.revision) || (this.major > version.major || this.minor > version.minor || this.build > version.build || this.revision > version.revision))
                return true;
            return false;
        }

        public int GetMajor()
        {
            if (major <= -1)
            {
                return -1;
            }
            return major;
        }

        public int GetMinor()
        {
            if (minor <= -1)
            {
                return -1;
            }
            return minor;
        }

        public int GetBuild()
        {
            if (build <= -1)
            {
                return -1;
            }
            return build;
        }

        public int GetRevision()
        {
            if (revision <= -1)
            {
                return -1;
            }
            return revision;
        }

        public string GetTag()
        {
            if (tag == "" || tag == null)
            {
                return "";
            }
            return tag;
        }

        public override string ToString()
        {
            string x = "";
            if (tag == null || tag == "")
            {
                if (major <= -1)
                {
                    if (minor <= -1)
                    {
                        x = $"{build}.{revision}";
                    }
                    else
                    {
                        x = $"{minor}.{build}.{revision}";
                    }
                }
                else
                {
                    if (minor <= -1)
                    {
                        var b = major;
                        major = -1;
                        minor = b;
                        x = $"{minor}.{build}.{revision}";
                    }
                    else
                    {
                        x = $"{major}.{minor}.{build}.{revision}";
                    }
                }
            }
            else
            {
                if (major <= -1)
                {
                    if (minor <= -1)
                    {
                        x = $"{build}.{revision}-{tag}";
                    }
                    else
                    {
                        x = $"{minor}.{build}.{revision}-{tag}";
                    }
                }
                else
                {
                    if (minor <= -1)
                    {
                        var b = major;
                        major = -1;
                        minor = b;
                        x = $"{minor}.{build}.{revision}-{tag}";
                    }
                    else
                    {
                        x = $"{major}.{minor}.{build}.{revision}-{tag}";
                    }
                }
            }
            return x;
        }

        public static CommandVersion Parse(string version)
        {
            CommandVersion cv = null;
            var a = $@"{version}";
            string[] dat = null;
            if (a.Contains("-"))
            {
                dat = a.Split('-');
                var dx = dat[0].Split('.');
                var tag = dat[1];
                if (dx.Length == 4)
                {
                    cv.major = int.Parse(dx[0]);
                    cv.minor = int.Parse(dx[1]);
                    cv.build = int.Parse(dx[2]);
                    cv.revision = int.Parse(dx[3]);
                }
                else if (dx.Length == 3)
                {
                    cv.major = -1;
                    cv.minor = int.Parse(dx[0]);
                    cv.build = int.Parse(dx[1]);
                    cv.revision = int.Parse(dx[2]);
                }
                else if (dx.Length == 2)
                {
                    cv.major = -1;
                    cv.minor = -1;
                    cv.build = int.Parse(dx[0]);
                    cv.revision = int.Parse(dx[1]);
                }
                else return null;
                cv.tag = tag;
            }
            else
            {
                var dx = dat = a.Split('.');
                if (dx.Length == 4)
                {
                    cv.major = int.Parse(dx[0]);
                    cv.minor = int.Parse(dx[1]);
                    cv.build = int.Parse(dx[2]);
                    cv.revision = int.Parse(dx[3]);
                }
                else if (dx.Length == 3)
                {
                    cv.major = -1;
                    cv.minor = int.Parse(dx[0]);
                    cv.build = int.Parse(dx[1]);
                    cv.revision = int.Parse(dx[2]);
                }
                else if (dx.Length == 2)
                {
                    cv.major = -1;
                    cv.minor = -1;
                    cv.build = int.Parse(dx[0]);
                    cv.revision = int.Parse(dx[1]);
                }
                else return null;
            }
            return cv;
        }
    }
}
