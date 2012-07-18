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
        private static DbProviderFactory Factory
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

        public static DbConnection Connection
        {
            get
            {
                var conn = ContextConnection;
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn = Factory.CreateConnection();
                    conn.ConnectionString = string.Format("Data Source={0}", dbpath);
                    conn.Open();
                }
                return conn;
            }
        }

        private static DbConnection ContextConnection
        {
            get
            {
                return GetContext<DbConnection>(CONNECTION_KEY);
            }
            set
            {
                SetContext(CONNECTION_KEY, value);
            }
        }

        private static DbTransaction ContextTransaction
        {
            get
            {
                return GetContext<DbTransaction>(TRANSACTION_KEY);
            }
            set
            {
                SetContext(TRANSACTION_KEY, value);
            }
        }

        private static T GetContext<T>(string key)
        {
            if (InWebContext)
                return (T)HttpContext.Current.Items[key];
            else
                return (T)CallContext.GetData(key);
        }

        private static void SetContext(string key, object value)
        {
            if (InWebContext)
                HttpContext.Current.Items[key] = value;
            else
                CallContext.SetData(key, value);
        }

        private static bool InWebContext
        {
            get
            {
                return HttpContext.Current != null;
            }
        }

        public static void Close()
        {
            if (ContextConnection != null)
                ContextConnection.Close();
        }

        private static void Prepare(DbCommand cmd, DbConnection conn, DbTransaction trans, string sql, Dictionary<string, object> parameters)
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
                    DbParameter param = Factory.CreateParameter();
                    param.ParameterName = entry.Key;
                    param.Value = entry.Value;
                    cmd.Parameters.Add(param);
                }
            }
        }

        public static object ConvertTo(object obj, Type t)
        {
            Type ot = obj.GetType();
            if (ot == t)
                return obj;
            string method_name = string.Format("To{0}", t.Name);
            var method = typeof(Convert).GetMethod(method_name, new[] { ot });
            return method.Invoke(null, new[] { obj });
        }

        public static T ConvertTo<T>(object obj)
        {
            return (T)ConvertTo(obj, typeof(T));
        }

        public static int Execute(string sql)
        {
            using (DbCommand cmd = Factory.CreateCommand())
            {
                cmd.Connection = Connection;
                cmd.CommandText = sql;
                return cmd.ExecuteNonQuery();
            }
        }

        public static int Execute(string sql, Dictionary<string, Object> args)
        {
            using (DbCommand cmd = Factory.CreateCommand())
            {
                Prepare(cmd, Connection, null, sql, args);
                int rows = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rows;
            }
        }

        public static int Insert(string table, string sql, Dictionary<string, object> args)
        {
            using (DbCommand cmd = Factory.CreateCommand())
            {
                Prepare(cmd, Connection, null, sql, args);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                // Access: cmd.CommandText = "SELECT @@IDENTITY FROM " + table;
                cmd.CommandText = "SELECT last_insert_rowid() FROM " + table;
                DbDataReader reader = cmd.ExecuteReader();
                reader.Read();
                return ConvertTo<int>(reader[0]);
            }
        }

        public static DbDataReader Query(string sql)
        {
            using (DbCommand cmd = Factory.CreateCommand())
            {
                cmd.Connection = Connection;
                cmd.CommandText = sql;

                DbDataReader reader = cmd.ExecuteReader();
                return reader;
            }
        }

        public static DbDataReader Query(string sql, Dictionary<string, object> args)
        {
            using (DbCommand cmd = Factory.CreateCommand())
            {
                Prepare(cmd, Connection, null, sql, args);
                DbDataReader reader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return reader;
            }
        }

        public static object One(string sql, Type t)
        {
            using (DbCommand cmd = Factory.CreateCommand())
            {
                cmd.Connection = Connection;
                cmd.CommandText = sql;

                var obj = cmd.ExecuteScalar();
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    return ConvertTo(obj, t);
                }
            }
        }

        public static T One<T>(string sql)
        {
            return (T)One(sql, typeof(T));
        }

        public static string QuoteName(string name)
        {
            if (name.StartsWith("\"") && name.EndsWith("\""))
            {
                return name;
            }
            return "\"" + name + "\"";
        }
    }
}
