using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newegg.Internship.CSharpTraining.SimpleORM.DataAccess;

namespace Newegg.Internship.CSharpTraining.SimpleORM.Tests.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = InsertData("Mark");
            RetrieveData(id);
            UpdateData(id,"Mark II");
            RetrieveData(id);
            DeleteData(id);

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
        }

        private static void DeleteData(int id)
        {
            Console.WriteLine();
            Console.WriteLine("Deleting data from database ...");

            using (var conn = SqlHelper.Instance.GetConnection())
            {
                var rowEffected = SqlHelper.Instance.ExecuteNonQuery(conn,
                    "DELETE TOP(1) dbo.MarkTestTable WHERE ID=@ID",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@ID", SqlDbType.Int) {Value = id}
                    });

                Console.WriteLine("{0} row effected", rowEffected);
            }
        }

        private static void UpdateData(int id, string newValue)
        {
            Console.WriteLine();
            Console.WriteLine("Updating data into database ...");
             
            using (var conn = SqlHelper.Instance.GetConnection())
            {
                var rowEffected = SqlHelper.Instance.ExecuteNonQuery(conn,
                    "UPDATE TOP(1) dbo.MarkTestTable SET Name=@Name,LastEditDate=GETDATE(),LastEditUser='DEMO' WHERE ID=@ID",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@ID", SqlDbType.Int) {Value = id},
                        new SqlParameter("@Name", SqlDbType.NVarChar, 50) {Value = newValue}
                    });

                Console.WriteLine("{0} row effected", rowEffected);
            }
        }

        private static int InsertData(string name)
        {
            Console.WriteLine();
            Console.WriteLine("Inserting data into database ...");

            int insertedId;

            using (var conn = SqlHelper.Instance.GetConnection())
            {
                insertedId = SqlHelper.Instance.ExecuteScalar<int>(conn,
                    "INSERT INTO dbo.MarkTestTable(Name, InDate, InUser) VALUES(@Name, GETDATE(), 'Demo') SELECT SCOPE_IDENTITY()",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@Name", SqlDbType.NVarChar, 50) {Value = name}
                    });

                Console.WriteLine("New record has been inserted, id {0}.", insertedId);
            }

            return insertedId;
        }

        private static void RetrieveData(int id)
        {
            Console.WriteLine();
            Console.WriteLine("Retrieving data from database ...");
             
            using (var conn = SqlHelper.Instance.GetConnection())
            {
                var reader = SqlHelper.Instance.ExecuteQuery(conn,
                    "SELECT ID, Name, InDate FROM dbo.MarkTestTable WITH(NOLOCK) WHERE ID=@ID",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@ID", SqlDbType.Int) {Value = id}
                    });

                var rowCount = 0;

                while (reader.Read())
                {
                    var retrievedID = reader.GetInt32(0);
                    var name = reader.GetString(1);
                    var timestamp = reader.GetDateTime(2);

                    Console.WriteLine("\tID:{0}\tName:{1}\tTimestamp:{2}", retrievedID, name, timestamp);

                    rowCount++;
                }

                Console.WriteLine("Total {0} row(s) retrieved.", rowCount);
            } 
        }
    }
}
