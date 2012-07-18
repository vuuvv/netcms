using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace vuuvv.utils
{
    public static class StringUtils
    {
        public static string FromCamelCase(string value)
        {
            return Regex.Replace(value, @"([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3_$2$4").ToLower();
        }

        public static string ToCamelCase(string value)
        {
            return value;
        }

        public static string JoinWithPattern(IEnumerable<string> names, string pattern, string separator)
        {
            var pattern_names = from name in names select string.Format(pattern, name);
            return string.Join(separator, pattern_names.ToArray());
        }

        public static string NormalList(IEnumerable<string> names)
        {
            return string.Join(",", names.ToArray());
        }

        public static string NameList(IEnumerable<string> names)
        {
            return JoinWithPattern(names, "`{0}`", ",");
        }

        public static string ParamList(IEnumerable<string> names)
        {
            return JoinWithPattern(names, "@{0}", ",");
        }

        public static string AssignList(IEnumerable<string> names)
        {
            return JoinWithPattern(names, "`{0}`=@{0}", ",");
        }
    }
}
