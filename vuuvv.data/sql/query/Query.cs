using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using vuuvv.data.models;
using vuuvv.data.sql.compiler;

namespace vuuvv.data.sql.query
{
    abstract public class Query
    {
        public abstract Compiler GetCompiler();
        public Model Model { get; set; }
    }
}
