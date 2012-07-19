using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using vuuvv.utils;
using vuuvv.data.models;
using vuuvv.data.sql.compiler;
using vuuvv.data.schemas;
using vuuvv.data.schemas.fields;

namespace vuuvv.data.sql.query
{
    abstract public class Query
    {
        protected abstract Compiler GetCompiler();
        public Model Model { get; set; }
        public MappedList<string, Field> Fields { get; set; }

        public IEnumerable<string> DBColumns
        {
            get
            {
                return from field in Fields.Values select field.DBColumn;
            }
        }

        public Compiler compiler;
        public Compiler Compiler
        {
            get
            {
                if (compiler == null)
                {
                    compiler = GetCompiler();
                }
                return compiler;
            }
        }

        private Table table;
        public Table Table
        {
            get
            {
                if (table == null)
                {
                    table = ClassHelper.Property<Table>(Model, "Table");
                }
                return table;
            }
        }

        public abstract object DoQuery();
    }
}
