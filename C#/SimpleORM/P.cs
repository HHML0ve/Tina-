using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newegg.Internship.CSharpTraining.SimpleORM
{
    public class P
    {
        static void Main(string[] args)
        {
            DataAccessor dataaccess = new DataAccessor();
            AEntity entity = new AEntity
            {
                ID = 101,
                Name = "hhm",
                InDate = new DateTime(),
                InUser = "hhm",
                LastEditDate = new DateTime(),
                LastEditUser = "hhm"
            };
            dataaccess.Update<AEntity>(entity);
        }
    }
}
