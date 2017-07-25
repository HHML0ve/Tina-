using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newegg.Internship.CSharpTraining.SimpleORM
{
    //处理表
    class TableAttribute:System.Attribute
    {
        private string _tableName;
        private string _pk;
        public bool _autoIncrement = false;
        //外键要怎么处理？？？单独一个类来管理主外键会更好？
        public TableAttribute(string _tableName,String _pk,bool _autoIncrement)
        {
            this._tableName = _tableName;
            this._pk = _pk;
            this._autoIncrement = _autoIncrement;
        }

        public string PK { get =>_pk; private set=>_pk = value; }
        public string TableName { get => _tableName; set => _tableName = value; }
    }
}
