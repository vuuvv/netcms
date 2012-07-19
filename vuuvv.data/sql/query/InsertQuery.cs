using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using vuuvv.data.models;
using vuuvv.data.schemas.fields;
using vuuvv.data.schemas;
using vuuvv.utils;
using vuuvv.data.sql.compiler;

namespace vuuvv.data.sql.query
{
    public class InsertQuery : Query
    {
        public InsertQuery(Model model)
        {
            Model = model;
            Fields = Model.Fields;
        }

        public InsertQuery(Model model, IEnumerable<string> fields)
        {
            Model = model;
            Fields = new MappedList<string, Field>();
            foreach (var field in fields)
            {
                Fields.Add(Model.Fields[field]);
            }
        }

        public InsertQuery(Model model, MappedList<string, Field> fields)
        {
            Model = model;
            Fields = fields;
        }

        protected override Compiler GetCompiler()
        {
            return new InsertCompiler(this) as Compiler;
        }

        public override object DoQuery()
        {
            string sql = Compiler.AsSql();
            Console.WriteLine(sql);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var field in Fields.Values)
            {
                parameters.Add(field.DBColumn, Model.GetFieldValue(field.Name));
            }
            DBHepler.Query(sql, parameters);
            return DBHepler.One<int>("SELECT last_insert_rowid() FROM " + DBHepler.QuoteName(Table.Name));
        }
    }
}
