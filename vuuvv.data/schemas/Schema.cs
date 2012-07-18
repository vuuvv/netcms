using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.data.schemas
{
    public abstract class Schema : Attribute
    {
        public string Name { get; set; }
    }
}
