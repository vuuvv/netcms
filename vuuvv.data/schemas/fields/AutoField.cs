using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.data.schemas.fields
{
    public class AutoField : Field
    {
        public override void initialize()
        {
            PrimaryKey = true;
        }

        public override object GetValue(object value)
        {
            return DBHepler.ConvertTo(value, typeof(int));
        } 
    }
}
