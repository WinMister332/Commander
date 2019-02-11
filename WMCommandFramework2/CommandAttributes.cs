using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace WMCommandFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class CommandAttribute : Attribute
    { 
        public virtual CommandAttribute GetCommandAttribute()
        {
            return GetAttribute<CommandAttribute>(this);
        }

        public static T GetAttribute<T>(object clazz) where T: Attribute
        {
            return (T)clazz.GetType().GetCustomAttributes(typeof(T), true)[0];
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor)]
    public class HiddenCommand : CommandAttribute
    {
        private bool hide = false;

        public HiddenCommand()
        {
            hide = true;
        }

        public HiddenCommand(bool hideCmd)
        {
            hide = hideCmd;
        }

        public bool IsHidden() => hide;
    }
}
