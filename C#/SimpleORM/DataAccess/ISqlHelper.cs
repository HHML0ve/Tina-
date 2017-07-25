using System.Collections.Generic;
using System.Data.SqlClient;

namespace Newegg.Internship.CSharpTraining.SimpleORM.DataAccess
{
    public interface ISqlHelper
    {
        SqlConnection GetConnection();

        SqlConnection GetConnection(string connectionString);

        SqlDataReader ExecuteQuery(SqlConnection conn, string sql, List<SqlParameter> parameters);

        int ExecuteNonQuery(string sql, List<SqlParameter> parameters);
        int ExecuteNonQuery(SqlConnection conn, string sql, List<SqlParameter> parameters);

        T ExecuteScalar<T>(string sql, List<SqlParameter> parameters);
        T ExecuteScalar<T>(SqlConnection conn, string sql, List<SqlParameter> parameters);
    }
}
