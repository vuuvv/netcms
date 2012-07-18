using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using vuuvv.utils;

namespace vuuvv.data.schemas.fields
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public abstract class Field : Schema, IMapped
    {
        public string VerboseName { get; set; }
        public bool PrimaryKey { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public bool Blank { get; set; }
        public bool Null { get; set; }
        public string DBColumn { get; set; }
        public object Default { get; set; }
        public string HelpText { get; set; }
        public bool DBIndex { get; set; }

        public virtual void initialize() {}

        public virtual string GetKey() 
        {
            return Name;
        }

        public virtual object GetValue(object value) 
        {
            return value;
        }

        public T GetValue<T>(object value)
        {
            return (T)GetValue(value);
        }
    }
}
