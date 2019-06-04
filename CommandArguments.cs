using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    public class CommandArguments
    {
        private List<string> args = null;

        internal CommandArguments()
        {
            args = new List<string>(0);
        }

        internal CommandArguments(string[] strx)
        {
            if (!(strx == null || strx.Length <= 0))
                args = strx.GetReturnable().ToList();
        }

        internal CommandArguments(List<string> strx)
        {
            args = strx;
        }

        #region General Functions

        /// <summary>
        /// Returns the maximum number of arguments that where passed into the array by the constructor.
        /// </summary>
        /// <returns>The maximum number of arguments.</returns>
        public int Count() => args.Count;

        /// <summary>
        /// Checks if there are no arguments in the argument array.
        /// </summary>
        /// <returns>True, No arguments. False, Contains arguments.</returns>
        public bool IsEmpty()
        {
            if (Count() <= 0) return true;
            else return false;
        }

        /// <summary>
        /// Gets the argument at the specified index or position within the argument array.
        /// </summary>
        /// <param name="pos">The position within the array.</param>
        /// <returns>The value at the specified position or index.</returns>
        public string GetArgumentAtPosition(int pos)
        {
            if (!(IsEmpty() && pos < 0))
                return args[GetLength(pos)];
            return "";
        }

        private int GetLength()
        {
            return GetLength(args);
        }

        private int GetLength(int raw)
        {
            if (raw >= 1)
                return raw - 1;
            else
                return 0;
        }

        private int GetLength(List<string> s)
        {
            return GetLength(s.Count);
        }

        public override string ToString()
        {
            string s = "";
            foreach (string sx in args)
            {
                if (s == "" || s == null)
                    s = sx;
                else
                    s += $" {sx}";
            }
            return s;
        }

        #endregion

        #region StartsWith Functions
        /// <summary>
        /// Checks if the argument array starts with the specified value.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True, if the value was found in the array.</returns>
        public bool StartsWith(string value)
        {
            if (!IsEmpty())
                if (args[0].ToLower() == value)
                    return true;
            return false;
        }
        
        /// <summary>
        /// Checks if the argument array starts with the specified switch value.
        /// </summary>
        /// <param name="switchValue">The value without '-' to check for as a swtich. Add only '-' for a double switch.</param>
        /// <returns>True, if the array starts with the swich value.</returns>
        public bool StartsWithSwitch(string switchValue)
        {
            if (StartsWith($"-{switchValue}"))
                return true;
            return false;
        }

        #endregion

        ///// <summary>
        ///// Checks if the arg array starts with the specified conditional argument.
        ///// </summary>
        ///// <param name="value">The conditional argument to check.</param>
        ///// <returns>True, if the conditional argument was found.</returns>
        //public bool StartsWithConditional(string header)
        //{
        //    if (StartsWithSwitch($"{header}:"))
        //        return true;
        //    return false;
        //}
        ///// <summary>
        ///// Checks if the arg array starts with the specified valued argument.
        ///// </summary>
        ///// <param name="value">The valued argument to check.</param>
        ///// <returns>True, if the valued argument was found.</returns>
        //public bool StartsWithValued(string key)
        //{
        //    if (StartsWithSwitch($"{key}="))
        //        return true;
        //    return false;
        //}
        ///// <summary>
        ///// Checks if the arg array starts with the specified variable.
        ///// </summary>
        ///// <param name="value">The variable to check.</param>
        ///// <returns>True, if the variable was found.</returns>
        //public bool StartsWithVariable(Variable variable)
        //{
        //    if (StartsWith(variable.ToString()))
        //        return true;
        //    return false;
        //}

        //#endregion

        //#region EndsWith

        ///// <summary>
        ///// Checks if the arg array ends with the specified value.
        ///// </summary>
        ///// <param name="value">The argument to check.</param>
        ///// <returns>True, if the argument was found.</returns>
        //public bool EndsWith(string value)
        //{
        //    if (!IsEmpty())
        //    {
        //        var x = GetArgumentAtPosition(GetLength(args));
        //        if ((x != "" || x != null) && x.Equals(value, StringComparison.CurrentCultureIgnoreCase))
        //            return true;
        //    }
        //    return false;
        //}
        ///// <summary>
        ///// Checks if the arg array ends with the specified switch.
        ///// </summary>
        ///// <param name="value">The switch to check.</param>
        ///// <returns>True, if the switch was found.</returns>
        //public bool EndsWithSwitch(string switchValue)
        //{
        //    if (EndsWith($"-{switchValue}"))
        //        return true;
        //    return false;
        //}
        ///// <summary>
        ///// Checks if the arg array ends with the specified conditiona argument.
        ///// </summary>
        ///// <param name="value">The conditional argument to check.</param>
        ///// <returns>True, if the conditional argument was found.</returns>
        //public bool EndsWithConditional(string header)
        //{
        //    if (EndsWithSwitch($"{header}:"))
        //        return true;
        //    return false;
        //}
        ///// <summary>
        ///// Checks if the arg array ends with the specified valued argument.
        ///// </summary>
        ///// <param name="value">The valued argument to check.</param>
        ///// <returns>True, if the valued argument was found.</returns>
        //public bool EndsWithValued(string key)
        //{
        //    if (EndsWithSwitch($"{key}="))
        //        return true;
        //    return false;

        //}
        ///// <summary>
        ///// Checks if the arg array ends with the specified variable.
        ///// </summary>
        ///// <param name="value">The variable to check.</param>
        ///// <returns>True, if the variable was found.</returns>
        //public bool EndsWithVariable(Variable variable)
        //{
        //    if (ContainsArgument(variable.ToString()))
        //        return true;
        //    return false;
        //}

        //#endregion

        //#region Contains

        ///// <summary>
        ///// Checks if the argument array contains the specified value.
        ///// </summary>
        ///// <param name="value">The value to check.</param>
        ///// <returns>True, if the value was found.</returns>
        //public bool ContainsArgument(string value)
        //{
        //    if (!IsEmpty())
        //    {
        //        foreach (string s in args)
        //            if (s.Equals(value, StringComparison.CurrentCultureIgnoreCase))
        //                return true;

        //    }
        //    return false;
        //}
        ///// <summary>
        ///// Checks if the argument array contains the specified value.
        ///// </summary>
        ///// <param name="value">The value to check.</param>
        ///// <param name="position">The position within the array to check.</param>
        ///// <returns>True, if the value was found.</returns>
        //public bool ContainsArgument(int position, string value)
        //{
        //    if (!IsEmpty())
        //    {
        //        var x = args[position];
        //        if (!(x == "" || x == null) && x.Equals(value, StringComparison.CurrentCultureIgnoreCase))
        //            return true;
        //    }
        //    return false;
        //}
        ///// <summary>
        ///// Checks if the argument array contains the specified switch.
        ///// </summary>
        ///// <param name="value">The switch to check.</param>
        ///// <returns>True, if the switch was found.</returns>
        //public bool ContainsSwitch(string value)
        //{
        //    var x = ContainsArgument($"-{value}");
        //    if (x) return true;
        //    return false;
        //}
        ///// <summary>
        ///// Checks if the argument array contains the specified switch.
        ///// </summary>
        ///// <param name="value">The switch to check.</param>
        ///// <param name="position">The position within the array to check.</param>
        ///// <returns>True, if the switch was found.</returns>
        //public bool ContainsSwitch(int position, string switchValue)
        //{
        //    return ContainsArgument(position, $"-{switchValue}");
        //}
        ///// <summary>
        ///// Checks if the argument array contains the specified conditional argument.
        ///// </summary>
        ///// <param name="value">The conditional argument to check.</param>
        ///// <returns>True, if the conditional argument was found.</returns>
        //public bool ContainsConditional(string header)
        //{
        //    var x = ContainsSwitch($"{header}:");
        //    if (x) return true;
        //    return false;

        //}
        ///// <summary>
        ///// Checks if the argument array contains the specified conditional argument.
        ///// </summary>
        ///// <param name="value">The conditional argument to check.</param>
        ///// <param name="position">The position within the array to check.</param>
        ///// <returns>True, if the conditional argument was found.</returns>
        //public bool ContainsConditional(int position, string header)
        //{
        //    return ContainsSwitch(position, $"{header}:");
        //}
        ///// <summary>
        ///// Checks if the argument array contains the specified valued argument.
        ///// </summary>
        ///// <param name="value">The valued argument to check.</param>
        ///// <returns>True, if the valued argument was found.</returns>
        //public bool ContainsValuedArgument(string key)
        //{
        //    var x = ContainsSwitch($"{key}=");
        //    if (x) return true;
        //    return false;
        //}
        ///// <summary>
        ///// Checks if the argument array contains the specified valued argument.
        ///// </summary>
        ///// <param name="value">The valued argument to check.</param>
        ///// <param name="position">The position within the array to check.</param>
        ///// <returns>True, if the valued argument was found.</returns>
        //public bool ContainsValuedArgument(int position, string key)
        //{
        //    return ContainsSwitch(position, $"{key}=");
        //}
        ///// <summary>
        ///// Checks if the argument array contains the specified variable.
        ///// </summary>
        ///// <param name="value">The variable to check.</param>
        ///// <returns>True, if the variable was found.</returns>
        //public bool ContainsVariable(Variable variable)
        //{
        //    var x = ContainsArgument(variable.ToString());
        //    if (x) return true;
        //    return false;
        //}
        ///// <summary>
        ///// Checks if the argument array contains the specified variable.
        ///// </summary>
        ///// <param name="value">The variable to check.</param>
        ///// <param name="position">The position within the array to check.</param>
        ///// <returns>True, if the variable was found.</returns>
        //public bool ContainsVariable(int position, Variable variable)
        //{
        //    return ContainsArgument(position, variable.ToString());
        //}

        //#endregion

        //#region IndexOf

        ///// <summary>
        ///// Gets the index of the specified argument.
        ///// </summary>
        ///// <param name="value">The argument.</param>
        ///// <returns>The index of the specified argument.</returns>
        //public int IndexOf(string value)
        //{
        //    if (!IsEmpty())
        //    {
        //        for (int i = 0; i < args.Count; i++)
        //        {
        //            var x = args[i];
        //            if (x.Equals(value, StringComparison.CurrentCultureIgnoreCase))
        //                return i;
        //        }
        //    }
        //    return 0;
        //}
        ///// <summary>
        ///// Gets the index of the specified switch.
        ///// </summary>
        ///// <param name="value">The switch.</param>
        ///// <returns>The index of the specified switch.</returns>
        //public int IndexOfSwitch(string switchValue)
        //{
        //    return IndexOf($"-{switchValue}");
        //}
        ///// <summary>
        ///// Gets the index of the specified conditional argument.
        ///// </summary>
        ///// <param name="value">The conditional argument.</param>
        ///// <returns>The index of the specified conditional argument.</returns>
        //public int IndexOfConditional(string header)
        //{
        //    return IndexOfSwitch($"{header}:");
        //}
        ///// <summary>
        ///// Gets the index of the specified valued argument.
        ///// </summary>
        ///// <param name="value">The valued argument.</param>
        ///// <returns>The index of the specified valued argument.</returns>
        //public int IndexOfValued(string key)
        //{
        //    return IndexOfSwitch($"{key}=");
        //}
        ///// <summary>
        ///// Gets the index of the specified variable.
        ///// </summary>
        ///// <param name="value">The variable.</param>
        ///// <returns>The index of the specified variable.</returns>
        //public int IndexOfVariable(Variable variable)
        //{
        //    return IndexOf(variable.ToString());
        //}

        //#endregion

        //#region ValueOf

        ///// <summary>
        ///// Gets the value of a conditional argument.
        ///// -header:value
        ///// </summary>
        ///// <param name="header">The header of the conditional argument.</param>
        ///// <returns>The value of the conditional.</returns>
        //public string ValueOfConditional(string header)
        //{
        //    if (!IsEmpty())
        //    {
        //        var x = GetArgumentAtPosition(IndexOfConditional(header));
        //        var xx = x.Split(':');
        //        var val = xx[1];
        //        return val;
        //    }
        //    return "";
        //}
        ///// <summary>
        ///// Gets the value of the valued argument.
        ///// -key=value;
        ///// </summary>
        ///// <param name="key">The key of the valued argument.</param>
        ///// <returns>The value of the valued.</returns>
        //public string ValueOfValued(string key)
        //{
        //    if (!IsEmpty())
        //    {
        //        var x = GetArgumentAtPosition(IndexOfValued(key));
        //        var xx = x.Split('=');
        //        var val = xx[1];
        //        return val;
        //    }
        //    return "";
        //}

        //#endregion

        //public class Variable
        //{
        //    private char variableHeader = '$';
        //    private string variableValue = "null";

        //    public Variable(char header, string value)
        //    {
        //        variableHeader = header;
        //        variableValue = value;
        //    }

        //    public char GetVariableHeader() => variableHeader;
        //    public string GetVariableValue() => variableValue;

        //    public override string ToString()
        //    {
        //        return $"{GetVariableHeader()}{GetVariableValue()}";
        //    }
        //}
    }
}
