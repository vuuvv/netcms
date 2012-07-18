using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using vuuvv.data;
using vuuvv.data.models;
using vuuvv.data.schemas;
using vuuvv.data.schemas.fields;
using vuuvv.utils;

namespace vuuvv.console
{
    class Program
    {
        static string test(string name)
        {
            return Regex.Replace(name, @"([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3_$2$4").ToLower();
        }

        static void Main(string[] args)
        {
            System.Reflection.Assembly.Load("vuuvv.data");
            var a = new AutoField();
            Int64 i = 64;
            Console.WriteLine(a.GetType().Assembly.GetName());
            Console.WriteLine(a.GetValue(i));
            Console.WriteLine(TestMEE123ab.Objects.Table.Name);
            string[] names = {"City", "FirstName", "DOB", "PATId", "RoomNO"};
            foreach (var name in names)
            {
                Console.WriteLine(test(name));
            }

            Console.WriteLine(StringUtils.AssignList(new string[] { "1", "2", "3" }));
            Console.ReadKey();
        }
    }

    [Table]
    class TestMEE123ab : Model
    {
        public static Manager<TestMEE123ab> Objects = new Manager<TestMEE123ab>();
    }
}
