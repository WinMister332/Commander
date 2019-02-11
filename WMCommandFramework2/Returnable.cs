using System;
using System.Collections;
using System.Collections.Generic;

namespace WMCommandFramework
{
    public class Returnable<T>
    {
        private List<T> tlist = null;

        public Returnable(List<T> ts)
        {
            tlist = new List<T>();
            Add(ts.ToArray());
        }

        public Returnable(T[] ts)
        {
            tlist = new List<T>();
            Add(ts);
        }

        internal void Add(T t)
        {
            if (!(tlist.Contains(t)))
                tlist.Add(t);
        }

        internal void Add(T[] ts)
        {
            foreach (T t in ts)
                Add(t);
        }

        public T[] ToArray() => tlist.ToArray();
        public List<T> ToList() => tlist;
    }
}
