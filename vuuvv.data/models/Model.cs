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
    public class Model
    {
        [AutoField]
        public int? Id { get; set; }

        protected Table table;
        public Table Table
        {
            get
            {
                if (table == null)
                {
                    Type t = this.GetType();
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

        protected MappedList<string, Field> fields;
        public MappedList<string, Field> Fields
        {
            get
            {
                if (fields == null)
                {
                    Type t = this.GetType();
                    fields = new MappedList<string, Field>();

                    List<MemberInfo> members = t.GetProperties().ToList<MemberInfo>();
                    members.AddRange(t.GetFields());

                    foreach (var m in members)
                    {
                        if (m.Name.ToLower() == "id")
                            continue;
                        var field = (Field)Attribute.GetCustomAttribute(m, typeof(Field));
                        if (field != null)
                        {
                            field.initialize();
                            if (field.Name == null)
                            {
                                field.Name = m.Name;
                            }
                            if (field.DBColumn == null)
                            {
                                field.DBColumn = StringUtils.FromCamelCase(field.Name);
                            }
                            fields.Add(field);
                        }
                    }
                }
                return fields;
            }
        }

        public IEnumerable<string> GetDBColumns(IEnumerable<string> fields)
        {
            return from field in fields select Fields[field].DBColumn;
        }

        public object GetFieldValue(string name) 
        {
            Type t = this.GetType();
            MemberInfo mi = t.GetMember(name)[0];
            if (mi.MemberType == MemberTypes.Field)
            {
                return t.GetField(name).GetValue(this);
            }
            else if (mi.MemberType == MemberTypes.Property)
            {
                return t.GetProperty(name).GetValue(this, null);
            }
            throw new FieldAccessException(string.Format("Field {0} not found in class {1}", name, t.Name));
        }
    }
}
