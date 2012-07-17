using System;
using System.IO;
using System.Data;
using System.Web;
using System.Data.Common;
using System.Data.SQLite;
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;

namespace vuuvv.data
{
    public class DBHepler
    {
        public static string dbpath = "App_Data/cms.db";

        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
        private const string CONNECTION_KEY = "CONTEXT_CONNECTION";

        private static DbProviderFactory _factory;
        private static DbProviderFactory factory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = DbProviderFactories.GetFactory("System.Data.SQLite");
                }
                return _factory;
            }
        }

        public static DbConnection connection
        {
            get
            {
                var conn = context_connection;
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn = factory.CreateConnection();
                    conn.ConnectionString = string.Format("Data Source={0}", dbpath);
                    conn.Open();
                }
                return conn;
            }
        }

        private static DbConnection context_connection
        {
            get
            {
                return get_context<DbConnection>(CONNECTION_KEY);
            }
            set
            {
                set_context(CONNECTION_KEY, value);
            }
        }

        private static DbTransaction context_transaction
        {
            get
            {
                return get_context<DbTransaction>(TRANSACTION_KEY);
            }
            set
            {
                set_context(TRANSACTION_KEY, value);
            }
        }

        private static T get_context<T>(string key)
        {
            if (in_web_context)
                return (T)HttpContext.Current.Items[key];
            else
                return (T)CallContext.GetData(key);
        }

        private static void set_context(string key, object value)
        {
            if (in_web_context)
                HttpContext.Current.Items[key] = value;
            else
                CallContext.SetData(key, value);
        }

        private static bool in_web_context
        {
            get
            {
                return HttpContext.Current != null;
            }
        }

        public static void close()
        {
            if (context_connection != null)
                context_connection.Close();
        }

        private static void prepare(DbCommand cmd, DbConnection conn, DbTransaction trans, string sql, Dictionary<string, object> parameters)
        {
            cmd.Connection = conn;
            cmd.CommandText = sql;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            if (parameters != null)
            {
                foreach (var entry in parameters)
                {
                    DbParameter param = factory.CreateParameter();
                    param.ParameterName = entry.Key;
                    param.Value = entry.Value;
                    cmd.Parameters.Add(param);
                }
            }
        }

        public static object convert_to(object obj, Type t)
        {
            Type ot = obj.GetType();
            if (ot == t)
                return obj;
            string method_name = string.Format("To{0}", t.Name);
            var method = typeof(Convert).GetMethod(method_name, new[] { ot });
            return method.Invoke(null, new[] { obj });
        }

        public static T convert_to<T>(object obj)
        {
            return (T)convert_to(obj, typeof(T));
        }

        public static int execute(string sql)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                return cmd.ExecuteNonQuery();
            }
        }

        public static int execute(string sql, Dictionary<string, Object> args)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                prepare(cmd, connection, null, sql, args);
                int rows = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rows;
            }
        }

        public static int insert(string table, string sql, Dictionary<string, object> args)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                prepare(cmd, connection, null, sql, args);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                // Access: cmd.CommandText = "SELECT @@IDENTITY FROM " + table;
                cmd.CommandText = "SELECT last_insert_rowid() FROM " + table;
                DbDataReader reader = cmd.ExecuteReader();
                reader.Read();
                return convert_to<int>(reader[0]);
            }
        }

        public static DbDataReader query(string sql)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;

                DbDataReader reader = cmd.ExecuteReader();
                return reader;
            }
        }

        public static DbDataReader query(string sql, Dictionary<string, object> args)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                prepare(cmd, connection, null, sql, args);
                DbDataReader reader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return reader;
            }
        }

        public static object one(string sql, Type t)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;

                var obj = cmd.ExecuteScalar();
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    return convert_to(obj, t);
                }
            }
        }

        public static T one<T>(string sql)
        {
            return (T)one(sql, typeof(T));
        }
    }
}
