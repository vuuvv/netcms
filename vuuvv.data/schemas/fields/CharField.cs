using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.data.schemas.fields
{
    public class CharField : Field
    {
        public override object GetValue(object value)
        {
            return value.ToString();
        }
    }
}
