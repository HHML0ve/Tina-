using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Newegg.Internship.CSharpTraining.SimpleORM
{
    //处理字段
     [AttributeUsage(
        AttributeTargets.All,//使用范围
        AllowMultiple = false,//是否允许元素中存在多个实例
        Inherited = false//Attribute是否可以被继承
        )]
    public class FildAttribute:System.Attribute
    {
        private string _rowName;
        private string _type;
        private int _length;
        public  string message;
        
        public string rowName { get=>_rowName; set =>_rowName = value; }
        public string type { get=>_type; set=>_type = value; }
        public int Length { get =>_length; set=>_length = value; }
        public string Message { get; set; }
       
        public FildAttribute(string _rowName,string _type,int _length)
        {
            this._rowName = _rowName;
            this._type = _type;
            this._length = _length;
        }
    }

}
