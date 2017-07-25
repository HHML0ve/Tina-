using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Newegg.Internship.CSharpTraining.SimpleORM.DataAccess;

namespace Newegg.Internship.CSharpTraining.SimpleORM
{
    public class DataAccessor : IDataAccessor
    {
        public DataAccessor()
        {
            // TODO: Implement this constructor
        }
        public DataAccessor(string connectionString)
        {
        }
        /*
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
             */
        //SELECT * FROM tablename WITH(NOLOCK) WHERE ID=@ID
        //不一定查主键
        public List<TEntity> Query<TEntity>(string condition) where TEntity : class, new()
        {
            Type type = typeof(TEntity);
            StringBuilder sql = new StringBuilder();
            StringBuilder sbproperties = new StringBuilder();
            sql.Append("SELECT *");
            var infos = type.GetProperties();
            /*  如果不想select * 就下面这种做法
            foreach (var info in infos)
            {
                object[] objs = type.GetCustomAttributes(typeof(FildAttribute),false);
                sbproperties.Append(((FildAttribute)objs[0]).rowName);
            }
             */
            //获取表的名字
            var tablename = type.GetCustomAttributes(typeof(TableAttribute), false);
            if (null != tablename)
            {
                sql.Append(((TableAttribute)tablename[0]).TableName);
            }
            sql.Append("WITH(NOLOCK) WHERE"+condition);
            List<TEntity> list = new List<TEntity>();
            using (var conn = SqlHelper.Instance.GetConnection())
            {
                var reader = SqlHelper.Instance.ExecuteQuery(conn,
                    sql.ToString(),
                    new List<SqlParameter>
                    {
                    });
               
                //通过反射由类型创建对应object实例.这里指的是创建TEntity的实例。参数必须是ActivationContext对象
                TEntity o = (TEntity)Activator.CreateInstance(type);
                while (reader.Read())
                {
                    foreach (var info in infos)
                    {
                        //获取typeof(***)的属性对应的特性信息
                        object[] objs = info.GetCustomAttributes(typeof(FildAttribute), false);
                        //将从数据库中得到的表中列赋值给实例对象0。
                        info.SetValue(o, reader[((FildAttribute)objs[0]).rowName],null);
                    }
                    list.Add(o);
                } 
            }
            return list;
        }
        /*
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

             */
        //"INSERT INTO dbo.MarkTestTable(Name, InDate, InUser) VALUES(@Name, GETDATE(), 'Demo') SELECT SCOPE_IDENTITY()",
        //已经验证成功的插入语句：Insert into dbo.Tina(ID,Name,InDate,InUser,LastEditDate,LastEditUser) values(2,'hhm','07/21/2017','hhm','07/21/17','Tina3') SET IDENTITY_INSERT dbo.Tina ON  ;
        public int Create<TEntity>(TEntity entity) where TEntity : class
        {
            Console.WriteLine("执行插入语句内");
            Type type = typeof(TEntity);
            
            //表的主键（int）
            String PK = "";
            object[] pkO = type.GetCustomAttributes(typeof(TableAttribute), false);
            if (null != pkO)
            {
                PK = ((TableAttribute)pkO[0]).PK;
            }
            Console.WriteLine("PK:{0}", PK);
            int insertID = 0;
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO ");
            //获取表的名字
            var tablename = type.GetCustomAttributes(typeof(TableAttribute),false);
            if (null != tablename) {
            sql.Append(((TableAttribute)tablename[0]).TableName);
            }
            StringBuilder sbProperties = new StringBuilder();
            sbProperties.Append("(");
            StringBuilder sbValue = new StringBuilder();
            sbValue.Append("VALUES(");
            var infos = type.GetProperties();
            List<SqlParameter> praras = new List<SqlParameter>();
            foreach (var info in infos)
            {
                object[] objs = info.GetCustomAttributes(typeof(FildAttribute),false);
                if (null != info)
                {
                    string s =((FildAttribute)objs[0]).rowName;
                    if (!PK.Equals(s))
                    {
                        sbProperties.Append(s + ",");
                        if (type.GetProperty(s).GetValue(entity, null).GetType() == typeof(int))
                        {
                            sbValue.Append(type.GetProperty(s).GetValue(entity, null) + ",");
                        }
                        else {
                            sbValue.Append("'" + type.GetProperty(s).GetValue(entity, null) + "',");
                        }
                    }
                }
               
            }
            sbProperties.Append(")");
            sql.Append(sbProperties);
            sbValue.Append(")");
            sql.Append(sbValue);
            sql.Replace(",)",")");
           // sql.Append(" SELECT SCOPE_IDENTITY()");
            sql.Append("SET IDENTITY_INSERT "+ ((TableAttribute)tablename[0]).TableName + " ON");
            Console.WriteLine("sql:{0}",sql.ToString());
            using (var conn = SqlHelper.Instance.GetConnection())
            {
                insertID = SqlHelper.Instance.ExecuteScalar<int>(conn,
                    sql.ToString(),
                    new List<SqlParameter>
                    {
                        //用来防止sql注入的
                    });
            }
            
            return insertID;
        }
        /*
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
             */
        //UPDATE TOP(1) dbo.MarkTestTable SET Name=@Name,LastEditDate=GETDATE(),LastEditUser='DEMO' WHERE ID=@ID
        public int Update<TEntity>(TEntity entity) where TEntity : class
        {
            Type type = typeof(TEntity);
            
            //1、构建sql语句
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE ");
            //2、获取表名
            var tablename = type.GetCustomAttributes(typeof(TableAttribute),false);
            if (null!=tablename)
            {
            sql.Append(((TableAttribute)tablename[0]).TableName);
            }
            sql.Append(" SET");

            //主键名,主键内容----实体的
            string PK = "";
            String pk_value = null;
            var pkO = type.GetCustomAttributes(typeof(TableAttribute), false);
            if (null != pkO)
            {
                PK = ((TableAttribute)pkO[0]).PK;
            }

            //3、获取更新的属性 Name=@Name,LastEditDate=GETDATE(),LastEditUser='DEMO'
            var infos = type.GetProperties();

            foreach (var info in infos)
            {
                object[] objs = type.GetCustomAttributes(typeof(FildAttribute),false);
                string properties = "";
                object value = null;
                foreach (var obj in objs)
                {
                    properties = ((FildAttribute)obj).rowName;
                     value= type.GetProperty(properties).GetValue(entity, null);
                }
                    sql.Append(properties+"="+value);
                    Console.WriteLine("properties{0},value{1}",properties,value);
            }
            sql.Append(" WHERE "+PK+"="+pk_value);
            Console.WriteLine("update_sql: {0}",sql);
            int rowEffected = 0;
            
            using (var conn = SqlHelper.Instance.GetConnection())
            {
                
                rowEffected = SqlHelper.Instance.ExecuteNonQuery(conn,
                    sql.ToString(),
                    new List<SqlParameter> {

                    });
            }
     

            return rowEffected;
        }
        /*
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
             */
        //"DELETE TOP(1) dbo.MarkTestTable WHERE ID=@ID",
        public int Delete<TEntity>(TEntity entity) where TEntity : class
        {
            StringBuilder sql = new StringBuilder();
            Type type = typeof(TEntity);
            var tablename = type.GetCustomAttributes(typeof(TableAttribute), false);
            if (null != tablename)
            {
                sql.Append(((TableAttribute)tablename[0]).TableName);
            }
            String PK = "";
            object[] pkO = type.GetCustomAttributes(typeof(TableAttribute), false);
            if (null != pkO)
            {
                PK = ((TableAttribute)pkO[0]).PK;
            }
            sql.Append("DELETE TOP(1)"+tablename+"WHERE "+PK);
            var rowEffected = 0;
            using (var conn = SqlHelper.Instance.GetConnection())
            {
                rowEffected = SqlHelper.Instance.ExecuteNonQuery(conn,
                    "",
                    new List<SqlParameter>
                    { 
                    });
            
            }
            return rowEffected;
        }
    }
}
