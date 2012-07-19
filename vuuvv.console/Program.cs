using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

using vuuvv.data;
using vuuvv.data.models;
using vuuvv.data.schemas;
using vuuvv.data.schemas.fields;
using vuuvv.utils;
using vuuvv.data.sql.query;
using vuuvv.data.sql.compiler;

namespace vuuvv.console
{
    class A
    {
        public string Name = "100";
        public static string SName = "s100";
        public string GetName(string name)
        {
            return name;
        }
        public static string SGetName(string name)
        {
            return name;
        }
    }

    class B<T> : A
    {
        public new string GetName(string obj)
        {
            return obj + "obj";
        }
    }

    class Program
    {
        static string test(string name)
        {
            return Regex.Replace(name, @"([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3_$2$4").ToLower();
        }

        static void Main(string[] args)
        {
            Test t = new Test();
            t.Id = 10;
            t.Name = "Jack10";
            t.Address = "U.S Kindom";
            t.EmailTest = "jack@gmail.com";
            DateTime now = DateTime.Now;
            Test.Objects.Update(t);
            Console.WriteLine("reflection escaped: " + (DateTime.Now - now).ToString());
            Console.WriteLine(t.Id);
            /*
            Console.WriteLine(ClassHelper.Field<string>(new A(), "Name"));
            Console.WriteLine(ClassHelper.StaticField<string>(new A(), "SName"));
            Console.WriteLine(ClassHelper.Call<string>(new A(), "GetName", "Hello World"));
            Console.WriteLine(ClassHelper.StaticCall<string>(new A(), "SGetName", "Static Hello World"));
            A a = new A();
            MethodInfo mi = typeof(A).GetMethod("GetName");
            for (int i = 0; i < 1000000; i++)
            {
                a.GetType().GetMethod("GetName");
            }
            Console.WriteLine("escaped: " + (DateTime.Now - now).ToString());
            now = DateTime.Now;
            for (int i = 0; i < 1000000; i++)
            {
                mi.Invoke(a, new object [] {"Hello World"});
            }
            System.Reflection.Assembly.Load("vuuvv.data");
            var a = new AutoField();
            Int64 i = 64;
            Console.WriteLine(a.GetType().Assembly.GetName());
            Console.WriteLine(a.GetValue(i));
            Console.WriteLine(Test.Objects.Table.Name);
            string[] names = {"City", "FirstName", "DOB", "PATId", "RoomNO"};
            foreach (var name in names)
            {
                Console.WriteLine(test(name));
            }

            Console.WriteLine(StringUtils.AssignList(new string[] { "1", "2", "3" }));
            */
            Console.ReadKey();
        }
    }

    [Table]
    class Test : Model
    {
        [CharField]
        public string Name { get; set; }
        [CharField]
        public string Address { get; set; }
        [CharField(DBColumn="email")]
        public string EmailTest { get; set; }

        public static Manager<Test> Objects = new Manager<Test>();
    }
}
