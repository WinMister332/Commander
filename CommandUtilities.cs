using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    public sealed class CommandUtilities
    {
        public static bool Debug { get; set; } = false;
        public static bool Echo { get; set; } = true;
        public static AppName ApplicationName { get; set; } = AppName.Commander;
        public static TerminalMessage Message { get; set; } = TerminalMessage.EMPTY;
    }

    public static class ClassExtentions
    {
        public static Returnable<T> GetReturnable<T>(this List<T> list)
        {
            return new Returnable<T>(list);
        }
        public static Returnable<T> GetReturnable<T>(this T[] tarr)
        {
            return new Returnable<T>(tarr);
        }

        public static Returnable<T> Skip<T>(this T[] ts, int count)
        {
            List<T> tsx = new List<T>();
            for (int i = 0; i < ts.Length; i++)
            {
                if (i >= count)
                    tsx.Add(ts[i]);
            }
            return tsx.GetReturnable();
        }

        public static string ToString<T>(this List<T> ts, char splitValue = ' ')
        {
            string s = "";
            foreach (T tx in ts)
            {
                var sx = tx.ToString();
                if (s == "" || s == null)
                    s = sx;
                else
                    s += $"{splitValue}{sx}";
            }
            return s;
        }
        public static string ToString<T>(this T[] ts, char splitValue = ' ')
        {
            string s = "";
            foreach (T tx in ts)
            {
                var sx = tx.ToString();
                if (s == "" || s == null)
                    s = sx;
                else
                    s += $"{splitValue}{sx}";
            }
            return s;
        }
        public static string ToString<T>(this Returnable<T> returnable, char splitValue = ' ')
        {
            return ToString(returnable.ToArray(), splitValue);
        }

        public static bool IsNullOrEmpty(this List<string> list)
        {
            if (list.Count <= 0 || list == null)
                return true;
            return false;
        }
        public static bool IsNullOrEmpty(this string k)
        {
            if (k == "" || k == null)
                return true;
            return false;
        }
    }
}
