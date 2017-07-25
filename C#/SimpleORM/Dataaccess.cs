using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Newegg.Internship.CSharpTraining.SimpleORM.DataAccess;

namespace Newegg.Internship.CSharpTraining.SimpleORM
{
    public class Dataaccess : IDataAccessor
    {
        public Dataaccess()
        {
            // TODO: Implement this constructor
        }
        public Dataaccess(string connectionString)
        {
        }
        //SELECT  * FROM tablename WITH(NOLOCK) WHERE condition
        public List<TEntity> Query<TEntity>(string condition) where TEntity : class, new()
        {
            List<TEntity> list = new List<TEntity>();
            StringBuilder sql = new StringBuilder();
            Type type = typeof(TEntity);
            string tablename = getTableName(type);
            string pk = getPK(type);
            sql.Append("SELECT * FROM "+tablename+ " WITH(NOLOCK) WHERE"+condition);
            var infos = type.GetProperties();
            using (var conn = SqlHelper.Instance.GetConnection())
            {
                var reader = SqlHelper.Instance.ExecuteQuery(conn,
                   sql.ToString(),
                   new List<SqlParameter>
                   {
                   });
                TEntity entity = (TEntity)Activator.CreateInstance(type);
                while (reader.Read())
                {
                    foreach (var info in infos)
                    {
                        if (null!=info)
                        {
                            Object[] objects = info.GetCustomAttributes(typeof(FildAttribute),false);
                            info.SetValue(entity,reader[((FildAttribute)objects[0]).rowName],null);
                        }
                    }
                    list.Add(entity);
                }
            }
                return list;
        }
      
        //已经验证成功的插入语句：Insert into dbo.Tina(ID,Name,InDate,InUser,LastEditDate,LastEditUser) values(2,'hhm','07/21/2017','hhm','07/21/17','Tina3') SET IDENTITY_INSERT dbo.Tina ON  ;
        public int Create<TEntity>(TEntity entity) where TEntity : class
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> param = new List<SqlParameter>();
            sql.Append("INSERT INTO ");
            int insertID = 0;
            Type type = typeof(TEntity);
            string tablename = getTableName(type);
            sql.Append(tablename+" (");
            string pk = getPK(type);
            StringBuilder sqlprop = new StringBuilder();
            StringBuilder sqlvalue = new StringBuilder();
            var infos = type.GetProperties();
            foreach (var info in infos)
            {
               if (null!=info)
               {
                  Object[] objects = info.GetCustomAttributes(typeof(FildAttribute),false);
                  foreach (var obj in objects)
                  {
                        if (null!=obj)
                        {
                            string s = ((FildAttribute)obj).rowName;
                            if (!s.Equals(pk))
                            {
                                sqlprop.Append(s+",");
                                if (type.GetProperty(s).GetValue(entity, null).GetType() == typeof(int))
                                {
                                    sqlvalue.Append(type.GetProperty(s).GetValue(entity, null) + ",");
                                }else
                                sqlvalue.Append("'" + type.GetProperty(s).GetValue(entity,null)+"',");
                                
                            }
                            param.Add(new SqlParameter(s, sqlvalue.ToString()));
                        }
                  }
               }
            }
            sql.Append(sqlprop);
            sql.Append(") VALUES (");
            sql.Append(sqlvalue+")");
            sql.Replace(",)",")");
           // sql.Append("SET IDENTITY_INSERT "+tablename+" ON");
            sql.Append("SELECT SCOPE_IDENTITY()");
            Console.WriteLine("sql{0}",sql);
            using (var conn = SqlHelper.Instance.GetConnection())
            {
                insertID = SqlHelper.Instance.ExecuteScalar<int>(conn,
                   sql.ToString(),
                   param); //用来防止sql注入的
            }
                return insertID;
        }
        //UPDATE TOP(1) dbo.MarkTestTable SET Name=@Name,LastEditDate=GETDATE(),LastEditUser='DEMO' WHERE ID=@ID
                public int Update<TEntity>(TEntity entity) where TEntity : class
                {
                    int rowinfect = 0;
                    using (var conn = SqlHelper.Instance.GetConnection())
                    {
                    List<SqlParameter> param = new List<SqlParameter>();
                    Type type = typeof(TEntity);
                    StringBuilder sqlprop = new StringBuilder();
                    StringBuilder sql = new StringBuilder();
                    StringBuilder sqlvalue = new StringBuilder();
                    sql.Append("UPDATE ");
                    string tablename = getTableName(type);
                    sql.Append(tablename+" SET ");
                    string pk = getPK(type);
                    var infos = type.GetProperties();
                    foreach (var info in infos)
                    {
                        if (null!=info)
                        {
                        Object[] objects = info.GetCustomAttributes(typeof(FildAttribute),false);
                            foreach (var obj in objects)
                            {
                                if (null!=obj)
                                {
                                    string s = ((FildAttribute)obj).rowName;
                                    if (!s.Equals(pk))
                                    {
                                        sql.Append(s+"="+ type.GetProperty(s).GetValue(entity, null)+",");
                                        param.Add(new SqlParameter(s, type.GetProperty(s).GetValue(entity, null).ToString()+","));
                                    }
                                }

                            }
                        }
                    }
                    string pkValue = getPKValue<TEntity>(type,entity);
                    sql.Append(" WHERE "+pk+" = "+pkValue);
                    sql.Replace(", WHERE", " WHERE");
                    Console.WriteLine("sql{0}", sql);
                        rowinfect = SqlHelper.Instance.ExecuteNonQuery(conn,
                            sql.ToString(),
                            param);
                    }
                    return rowinfect;
                }
        //"DELETE TOP(1) dbo.MarkTestTable WHERE ID=@ID",
        public int Delete<TEntity>(TEntity entity) where TEntity : class
        {
            int inflect = 0;
            Type type = typeof(TEntity);
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE ");
            string tablename = getTableName(type);
            sql.Append(tablename + " WHERE ");
            string pk = getPK(type);
            string pkValue = getPKValue<TEntity>(type, entity);
            sql.Append(pk+"="+pkValue);
            Console.WriteLine("sql{0}", sql);
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter(pk,pkValue));
            using (var conn = SqlHelper.Instance.GetConnection())
            {
                inflect = SqlHelper.Instance.ExecuteNonQuery(conn,
                    sql.ToString(),
                   param);

            }
            return inflect;
        }
        //只能是tableAttribute FildAttribute
        private string getTableName(Type type)
        {
            string _tableName;
            var tableName = type.GetCustomAttributes(typeof(TableAttribute), false);
            _tableName = ((TableAttribute)tableName[0]).TableName;
            return _tableName;
        }
        private string getPK(Type type)
        {
        string _pk;
            var pk = type.GetCustomAttributes(typeof(TableAttribute),false);
            _pk = ((TableAttribute)pk[0]).PK;
            return _pk;
        }
        private string getPKValue<Tentity>(Type type,Tentity entity)
        {
            string _pkValue = "";
            string pk = getPK(type);
            if (type.GetProperty(pk).GetValue(entity, null) != null)
            {
                _pkValue= type.GetProperty(pk).GetValue(entity,null).ToString();

            }
            else
            {
                Console.WriteLine("null");
            }
            return _pkValue;
        }
        
    }
}
