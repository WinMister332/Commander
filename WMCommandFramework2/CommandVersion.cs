using System;
namespace WMCommandFramework
{
    public class CommandVersion
    {
        private int[] version = new int[0];
        private string tag = "";

        public CommandVersion(int major, int minor, int build, int revision, string tag = "")
        {
            if (tag == null || tag == "") tag = "";
            this.tag = tag;
            version = new int[] { major, minor, build, revision };
        }

        public CommandVersion(int minor, int build, int revision, string tag = "")
        {
            if (tag == null || tag == "") tag = "";
            this.tag = tag;
            version = new int[] { minor, build, revision };
        }

        public CommandVersion(int build, int revision, string tag = "")
        {
            if (tag == null || tag == "") tag = "";
            this.tag = tag;
            version = new int[] { build, revision };
        }

        /// <summary>
        /// The MAJOR version.
        /// </summary>
        public int Major
        {
            get
            {
                if (version.Length == 4)
                    return version[0];
                else
                    return 0;
            }
        }

        /// <summary>
        /// The MINOR version.
        /// </summary>
        public int Minor
        {
            get
            {
                if (version.Length == 4)
                    return version[1];
                else if (version.Length == 3)
                    return version[0];
                else return 0;
            }
        }

        /// <summary>
        /// The BUILD version.
        /// </summary>
        public int Build
        {
            get
            {
                if (version.Length == 4)
                    return version[2];
                else if (version.Length == 3)
                    return version[1];
                else if (version.Length == 2)
                    return version[0];
                else return 0;
            }
        }

        /// <summary>
        /// The REBUILD version.
        /// </summary>
        public int Revision
        {
            get
            {
                if (version.Length == 4)
                    return version[3];
                else if (version.Length == 3)
                    return version[2];
                else if (version.Length == 2)
                    return version[1];
                else return 0;
            }
        }

        /// <summary>
        /// Optional display tag.
        /// </summary>
        public string Tag
        {
            get
            {
                if (!(tag == "" | tag == null))
                    return tag;
                else
                    return "";
            }
            private set => tag = value;
        }

        private double VersionNumber()
        {
            if (version.Length == 4)
            {
                return double.Parse($"0.0{Major}{Minor}{Build}{Revision}");
            }
            else if (version.Length == 3)
            {
                return double.Parse($"0.000{Minor}{Build}{Revision}");
            }
            else if (version.Length == 2)
            {
                return double.Parse($"0.00000{Build}{Revision}");
            }
            else return 0.0;
        }

        /// <summary>
        /// Checks if this version is newer then the specified version
        /// </summary>
        /// <returns><c>true</c>, if this version is newer then the specified version, <c>false</c> if this version is older or equal to this version. <paramref name="version"</returns>
        /// <param name="version">The version to compare.</param>
        public bool VersionNewer(CommandVersion version)
        {
            if (VersionNumber() > version.VersionNumber())
                return true;
            return false;
        }

        /// <summary>
        /// Checks if this version is older then the specified version
        /// </summary>
        /// <returns><c>true</c>, if this version is older then the specified version, <c>false</c> if this version is newer or equal to this version. <paramref name="version"</returns>
        /// <param name="version">The version to compare.</param>
        public bool VersionOlder(CommandVersion version)
        {
            if (VersionNumber() < version.VersionNumber())
                return true;
            return false;
        }

        /// <summary>
        /// Checks if this version is equal to the specified version.
        /// </summary>
        /// <returns><c>true</c>, if this version is equal to the specified version, <c>false</c> if this version is older or newer to this version.<paramref name="version"</returns>
        /// <param name="version">The version to compare.</param>
        public bool VersionEqual(CommandVersion version)
        {
            if (VersionNumber() == version.VersionNumber())
                return true;
            return false;
        }

        public override string ToString()
        {
            if (version.Length == 4)
            {
                if (tag != null || tag != "")
                    return $"{Major}.{Minor}.{Build}.{Revision}-{tag}";
                return $"{Major}.{Minor}.{Build}.{Revision}";
            }
            else if (version.Length == 3)
            {
                if (tag != null || tag != "")
                    return $"{Minor}.{Build}.{Revision}-{tag}";
                return $"{Minor}.{Build}.{Revision}";
            }
            else if (version.Length == 2)
            {
                if (tag != null || tag != "")
                    return $"{Build}.{Revision}-{tag}";
                return $"{Build}.{Revision}";
            }
            else return $"0.0.0.0";
        }

        public static CommandVersion BLANK = new CommandVersion(0,0,0,0);
    }
}
