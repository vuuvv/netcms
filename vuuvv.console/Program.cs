using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using vuuvv.data;

namespace vuuvv.console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Reflection.Assembly.Load("vuuvv.data");
            Console.Write("1");
            Console.ReadKey();
        }
    }
}
