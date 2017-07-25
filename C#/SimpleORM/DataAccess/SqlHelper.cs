using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;

namespace Newegg.Internship.CSharpTraining.SimpleORM.DataAccess
{
    public class SqlHelper : ISqlHelper
    {
        public const string ConnectionStringCfgKey = "ConnectionString";
        private SqlHelper() { }
        private static readonly Lazy<SqlHelper> LazyInstance = new Lazy<SqlHelper>(
        () => new SqlHelper(), LazyThreadSafetyMode.ExecutionAndPublication);
        static SqlHelper()
        {

        }
        private static ISqlHelper instance = null;
        public static void SetInstance(ISqlHelper fakeSqlHelper)
        {
            instance = fakeSqlHelper;
        }
        public static void ResetInstance()
        {
            instance = null;
        }
        public static ISqlHelper Instance
        {
            get
            {
                if (null != instance) return instance;
                return LazyInstance.Value;
            }
        }
        public SqlConnection GetConnection()
        {
            return GetConnection(null);
        }
        public SqlConnection GetConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                //connectionString =
                //    ConfigurationManager.AppSettings.Get(ConnectionStringCfgKey);
                connectionString = @"Server=SCMISBIZTALK01;UID=intern;Password=Th!sls@l0ngPWD;Database=CSharpTraining";
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(
                "Connection string must be specified " +
                "either from parameter or App.config file.");
            }
            return new SqlConnection(connectionString);
        }
        private void EnsureConnectionOpened(DbConnection conn)
        {
            if (conn.State == ConnectionState.Open) return;
            conn.Open();
        }
        private SqlCommand BuildCommnand(SqlConnection conn,
        string sql,
        List<SqlParameter> parameters)
        {
            var cmd = new SqlCommand(sql, conn);
            if (null != parameters)
            {
                parameters.ForEach(p => cmd.Parameters.Add(p));
            }
            return cmd;
        }
        public SqlDataReader ExecuteQuery(SqlConnection conn,
        string sql,
        List<SqlParameter> parameters)
        {
            var cmd = BuildCommnand(conn, sql, parameters);
            EnsureConnectionOpened(conn);
            return cmd.ExecuteReader();
        }
        public int ExecuteNonQuery(string sql, List<SqlParameter> parameters)
        {
            using (var conn = GetConnection())
            {
                return ExecuteNonQuery(conn, sql, parameters);
            }
        }
        public int ExecuteNonQuery(SqlConnection conn,
        string sql,
        List<SqlParameter> parameters)
        {
            var cmd = BuildCommnand(conn, sql, parameters);
            EnsureConnectionOpened(conn);
            return cmd.ExecuteNonQuery();
        }
        public T ExecuteScalar<T>(SqlConnection conn,
        string sql,
        List<SqlParameter> parameters)
        {
            var cmd = BuildCommnand(conn, sql, parameters);
            EnsureConnectionOpened(conn);
            var result = cmd.ExecuteScalar();
            if (null == result || DBNull.Value == result)
            {
                return default(T);
            }
            return (T)Convert.ChangeType(result, typeof(T));
        }
        public T ExecuteScalar<T>(string sql, List<SqlParameter> parameters)
        {
            using (var conn = GetConnection())
            {
                return ExecuteScalar<T>(conn, sql, parameters);
            }
        }
    }
}
