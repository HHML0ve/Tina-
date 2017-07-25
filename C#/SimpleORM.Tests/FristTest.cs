using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using Newegg.Internship.CSharpTraining.SimpleORM.Tests;
using Newegg.Internship.CSharpTraining.SimpleORM.DataAccess;

namespace Newegg.Internship.CSharpTraining.SimpleORM.Tests
{
    [TestFixture]
    public class FristTest
    {
        /*
        [Test]
       public void test1()
        {
            Person person = new Person
            {
                ID = 02,
                Name = "hhm",
                InDate = new DateTime(),
                InUser = "hhm02",
                LastEditDate = new DateTime(),
                LastEditUser = "hhm03"
            };
            foreach (var attr in typeof(anyClass).GetCustomAttributes(true))
            {
                person = attr as Person;
                if(null != person)
                {
                    Console.WriteLine("anyClass:{0}",person.ID,person.Name);
                }
            }
            Console.ReadLine();
        }
         */
        [Test]
        public void test2()
        {
            Type t1 = "hello".GetType();
            Console.WriteLine(t1.FullName);
            /*

            String s = "world";
            Type t4 = Type.GetType(s,true,true);//对象，是否报错，是否忽略大小写
               */
            Type t2 = typeof(String);
            Type t3 = typeof(AEntity);

            //Object[] obj = t3.GetProperties();
            //.GetNestedTypes();
            //.GetProperties();
            /*
            String tt;
            foreach (var att in obj)
            {
                Console.WriteLine("propertites: {0}",att);
                tt = att.ToString();
                Console.WriteLine("tt:{0}",tt);
            }
            String s = "String Name";
            int k = s.IndexOf(' ');
            Console.WriteLine(s.Substring(k+1));

            Person per = new Person();
            Console.WriteLine(per.GetType().Name);
         
           
              System.Reflection.MemberInfo info = typeof(Person);
              Object[] attributes = info.GetCustomAttributes(true);
              foreach(object att in attributes)
              {
                  Console.WriteLine("propertites: {0}", att.GetType());
              }
              */


            Object[] obj2 = t3.GetCustomAttributes(true);
            foreach (object att in obj2)
            {
                /*
                DeBugInfo dbi = (DeBugInfo)att;
                if (null!=dbi)
                {
                    Console.WriteLine("值: {0}", dbi.BugNo);
                    Console.WriteLine("值: {0}", dbi.Developer);
                    Console.WriteLine("值: {0}", dbi.LastReview);
                    Console.WriteLine("值: {0}", dbi.Message);
                }
                 */
                
            }
            foreach (MethodInfo m in t3.GetMembers())
            {
                foreach (Attribute a in m.GetCustomAttributes(true))
                {
                    /*
                    DeBugInfo dbi = (DeBugInfo)a;
                    if (null != dbi)
                    {
                        Console.WriteLine("值: {0},fro Method", dbi.BugNo,m.Name);
                        Console.WriteLine("值: {0}", dbi.Developer);
                        Console.WriteLine("值: {0}", dbi.LastReview);
                        Console.WriteLine("值: {0}", dbi.Message);
                    }
                     */
                }
            }
        }
        /*
        [Test]
        public void test3()
        {
            anyClass any = new anyClass();
            any.Message = "this is a anyClass";
            any.display();

            Type t = typeof(anyClass);
            //属性
            foreach (var att in t.GetCustomAttributes(false))
            {
                Person person = (Person)att;
                if (null != person)
                {
                    Console.WriteLine("person no:{0}",person.ID);
                    Console.WriteLine("person name:{0}",person.Name);
                }
            }
            //方法
            foreach (MethodInfo m in t.GetMethods())
            {
                foreach(Attribute a in m.GetCustomAttributes(true))
                {
                    Person person = (Person)a;
                    if (null!=person)
                    {
                        Console.WriteLine("Bug:{0},what:{1}",person.Name,m.Name);
                    }
                }
            }
            Console.ReadLine();
        }
         */
        [Test]
        public void test4()
        {

            //通过构造函数确定是哪数据库
            DataAccessor dataaccess = new DataAccessor();

            AEntity entity = new AEntity
            {
                ID = 101,
                Name = "hhmName",
                InDate = new DateTime(2017,7,21),
                InUser = "hhmInuser",
                LastEditDate = new DateTime(2017,7,21),
                LastEditUser = "hhmLastEditUser"
            };
            //插入用户

            /*entity.ID = 102;
            entity.InUser = "hhm";
            entity.LastEditUser = "hhm2";
            entity.InDate = new DateTime(2017,7,19);
            entity.LastEditDate = new DateTime(2017,7,19);*/
            Console.WriteLine("执行插入语句外");
           // dataaccess.Create<AEntity>(entity);
            //更新用户信息
            dataaccess.Update<AEntity>(entity);
            /*
            //删除
            dataaccess.Delete<Entity>(entity);
             */

            //查询:获取***表中，ID大于5的人
            /*
            String condition = "";
           List<AEntity> list =  dataaccess.Query<AEntity>(condition);
            foreach (AEntity p in list)
            {
                Console.WriteLine("select:{0}",p.ID);
            }
             */
        }

        [Test]
        public void test5()
        {
            AEntity entity = new AEntity
            {
                ID = 5,
                Name = "Tom",
                InDate = new DateTime(2017, 7, 22),
                InUser = "hhm",
                LastEditDate = new DateTime(2017, 7, 22),
                LastEditUser = "hhmLastEditUser"
            };
            Dataaccess da = new Dataaccess();
          //  da.Create<AEntity>(entity);
            //da.Update<AEntity>(entity);
            da.Delete<AEntity>(entity);
           // string s = "ID = 101";
            //da.Query<AEntity>(s);
        }
    }
}
