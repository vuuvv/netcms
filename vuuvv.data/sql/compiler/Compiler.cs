using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using vuuvv.data.sql.query;

namespace vuuvv.data.sql.compiler
{
    public abstract class Compiler
    {
        public Query Query { get; set; }
        public abstract string AsSql();
    }
}
