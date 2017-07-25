using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Diagnostics;

namespace Newegg.Internship.CSharpTraining.SimpleORM
{
    /*
     实体类
         */
     [Table("dbo.Tina", "ID",true)]
    public class AEntity
    {
        private int _id;
        private String _name;
        private DateTime _inDate;
        private String _inUser;
        private DateTime _lastEditDate;
        private String _lastEditUser;
      
        [Fild("ID","int",0,message ="primary key")]
        public int ID { get =>_id; set => _id = value; }
        [Fild("Name", "nvarchar",50)]
        public String Name { get { return _name; } set { _name = value; } }
        [Fild("InDate","datetime",0)]
        public DateTime InDate { get => _inDate; set => _inDate = value; }
        [Fild("InUser", "varchar", 15)]
        public String InUser { get => _inUser; set => _inUser = value; }
        [Fild("LastEditDate","datetime",0)]
        public DateTime LastEditDate { get => _lastEditDate; set => _lastEditDate = value; }
        [Fild("LastEditUser","varchar",15)]
        public String LastEditUser { get => _lastEditUser; set => _lastEditUser = value; }

    }
}
