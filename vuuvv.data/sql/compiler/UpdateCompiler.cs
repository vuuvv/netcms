using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using vuuvv.utils;
using vuuvv.data.sql.query;

namespace vuuvv.data.sql.compiler
{
    public class UpdateCompiler : Compiler
    {
        public UpdateCompiler(UpdateQuery query)
        {
            Query = query;
        }

        public override string AsSql()
        {
            List<string> result = new List<string>();

            result.Add("UPDATE");
            result.Add(DBHepler.QuoteName(Query.Table.Name));
            result.Add("SET");
            result.Add(StringUtils.AssignList(Query.DBColumns));
            result.Add(string.Format("WHERE id={0}", Query.Model.Id));

            return string.Join(" ", result.ToArray());
        }
    }
}
