using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework
{
    public class CommandArgs
    {
        private List<string> args = new List<string>();

        public CommandArgs() { new CommandArgs(new string[] { }); }

        public CommandArgs(string[] argsx)
        {
            foreach (string s in argsx)
            {
                args.Add(s);
            }
        }

        /// <summary>
        /// Checks to see if there are no args.
        /// </summary>
        /// <returns>Whether there are args.</returns>
        public bool isEmpty()
        {
            if (args.Count == 0) return true;
            return false;
        }

        /// <summary>
        /// Gets the argument at the following position.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public string GetArgAtPosition(int pos)
        {
            return args[pos];
        }

        /// <summary>
        /// Checks if there is a switch anywhere in the arg string.
        /// </summary>
        /// <param name="switch_key">The swich key to check for.</param>
        /// <returns>Whether there is a switch value.</returns>
        public bool ContainsSwitch(string switch_key)
        {
            if (args.Contains($"-{switch_key}") || args.Contains($"/{switch_key}")) return true;
            return false;
        }

        /// <summary>
        /// Checks if there is a segmented switch anywhere in the arg array.
        /// </summary>
        /// <param name="segment">The segment to search for.</param>
        /// <returns>True if the array contains the segmented switch.</returns>
        public bool ContainsSegmentedSwitch(string segment)
        {
            if (args.Contains($"-{segment}:")) return true;
            return false;
        }

        /// <summary>
        /// Checks to see if the specified arg is anywhere in the argument stirng.
        /// </summary>
        /// <param name="arg">The arg to search for.</param>
        /// <returns>Whether the arg was found.</returns>
        public bool ContainsArg(string arg)
        {
            if (args.Contains(arg)) return true;
            return false;
        }

        /// <summary>
        /// Gets the index of the specified arg.
        /// </summary>
        /// <param name="arg">The arg to get the index of.</param>
        /// <returns>The index of the specified arg.</returns>
        public int GetIndexOfArg(String arg)
        {
            if (ContainsArg(arg))
            {
                var index = args.IndexOf(arg);
                return index;
            }
            return 0;
        }

        /// <summary>
        /// Gets the index of the swithc witht he following name.
        /// </summary>
        /// <param name="switch_key">The name of the switch key.</param>
        /// <returns>The index of the switch with the key.</returns>
        public int GetIndexOfSwitch(string switch_key)
        {
            if (ContainsSwitch(switch_key))
            {
                if (args.Contains($"-{switch_key}"))
                {
                    var index = args.IndexOf($"-{switch_key}");
                    return index;
                }
                else if (args.Contains($"/{switch_key}"))
                {
                    var index = args.IndexOf($"/{switch_key}");
                    return index;
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets the index of the segmented switch with the following segment.
        /// </summary>
        /// <param name="segment">The segment of the segmented switch.</param>
        /// <returns>The segmented switch value.</returns>
        public string GetSegmentedSwitchValue(string segment)
        {
            if (ContainsSegmentedSwitch(segment))
            {
                string data = "";
                foreach (string segmented in args)
                {
                    if (segmented.StartsWith($"-{segment}:"))
                    {
                        var x = segmented.Split(':');
                        data = x[1];
                        break;
                    }
                }
                return data;
            }
            return null;
        }

        /// <summary>
        /// Skips the argument at the following location.
        /// </summary>
        /// <param name="index">The index to split from.</param>
        /// <returns>A reformmated string array.</returns>
        public string[] Skip(int index)
        {
            var x = args.ToString();
            var l = x.Substring(index, x.Length);
            var s = l.Split(' ');
            var ls = new List<string>();
            foreach (string sx in s)
                ls.Add(sx);
            return ls.ToArray();
        }

        /// <summary>
        /// Skips the argument with the following name.
        /// </summary>
        /// <param name="arg">The string to skip from.</param>
        /// <returns>The newly formatted string only containing the data that was skipped from.</returns>
        public string[] Skip(string arg)
        {
            return Skip(GetIndexOfArg(arg));
        }
    }
}
