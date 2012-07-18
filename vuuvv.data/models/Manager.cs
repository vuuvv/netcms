using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using vuuvv.utils;

using vuuvv.data.schemas;
using vuuvv.data.schemas.fields;

namespace vuuvv.data.models
{
    public class Manager<T> 
        where T : Model
    {
        protected Table table;
        public Table Table
        {
            get
            {
                if (table == null)
                {
                    Type t = typeof(T);
                    table = (Table)Attribute.GetCustomAttribute(t, typeof(Table));
                    if (table.Name == null)
                    {
                        var name = StringUtils.FromCamelCase(t.Name);
                        table.Name = "vuuvv_" + name;
                    }
                }
                return table;
            }
        }

        protected MappedList<Field> fields;
        public MappedList<Field> Fields
        {
            get
            {
                if (fields == null)
                {
                    Type t = typeof(T);
                    fields = new MappedList<Field>();

                    List<MemberInfo> members = t.GetProperties().ToList<MemberInfo>();
                    members.AddRange(t.GetFields());

                    foreach (var m in members)
                    {
                        var field = (Field)Attribute.GetCustomAttribute(m, typeof(Field));
                        if (field != null)
                        {
                            field.initialize();
                            if (field.Name == null)
                            {
                                field.Name = StringUtils.FromCamelCase(m.Name);
                            }
                            fields.Add(field);
                        }
                    }
                }
                return fields; 
            }
        }

        public virtual T Get(int pk) 
        {
            return null;
        }

        public virtual int Insert(T model)
        {
            return Insert(model, Fields.Keys.ToArray());
        }

        public virtual int Insert(T model, string[] fields)
        {
            return 0;
        }

        public virtual T Update(T model, string[] fields)
        {
            return model;
        }

        public virtual int delete()
        {
            return 0;
        }
    }
}
