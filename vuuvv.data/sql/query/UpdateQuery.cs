using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using vuuvv.utils;
using vuuvv.data.models;
using vuuvv.data.schemas.fields;
using vuuvv.data.sql.compiler;

namespace vuuvv.data.sql.query
{
    public class UpdateQuery : Query
    {
        public UpdateQuery(Model model)
        {
            Model = model;
            Fields = Model.Fields;
        }

        public UpdateQuery(Model model, IEnumerable<string> fields)
        {
            Model = model;
            Fields = new MappedList<string, Field>();
            foreach (var field in fields)
            {
                Fields.Add(Model.Fields[field]);
            }
        }

        public UpdateQuery(Model model, MappedList<string, Field> fields)
        {
            Model = model;
            Fields = fields;
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
            return DBHepler.Query(sql, parameters);
        }

        protected override compiler.Compiler GetCompiler()
        {
            return new UpdateCompiler(this);
        }
    }
}
