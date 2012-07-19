using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using vuuvv.data.sql.query;
using vuuvv.data.schemas;
using vuuvv.utils;

namespace vuuvv.data.sql.compiler
{
    public class InsertCompiler : Compiler
    {
        public InsertCompiler(InsertQuery query)
        {
            Query = query;
        }

        public override string AsSql()
        {
            List<string> result = new List<string>();

            result.Add("INSERT INTO");
            result.Add(DBHepler.QuoteName(Query.Table.Name));
            result.Add(string.Format("({0})", StringUtils.NameList(Query.DBColumns)));
            result.Add(string.Format("VALUES ({0})", StringUtils.ParamList(Query.DBColumns)));

            return string.Join(" ", result.ToArray());
        }
    }
}
