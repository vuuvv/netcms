using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.utils
{
    public interface IMapped<TKey>
    {
        TKey GetKey();
    }
}
