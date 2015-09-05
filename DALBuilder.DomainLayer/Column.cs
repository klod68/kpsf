using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DALBuilder.DomainLayer
{

    /// <summary>
    /// Why this class has no behavior?
    /// </summary>
    public class Column
    {
        private Table _table;
        private string _name;
        private int _id;
        private string _dataType;
        private byte _precision;
        private byte _scale;
        private short _maxLength;
        private bool _isIdentity;
        private bool _isRowGuid;
        private bool _isComputed;
        private bool _isRowversion;
        private bool _isPessimisticConcurrencySupport;
        private bool _isOptimisticConcurrencySupport;
        private string _parameterName;
        private bool _isPrimaryKey;

        public Column(){}
        public Table Table
        {
           set { _table = value; }
           get { return _table; }
        }
        public string Name
        {
           set { _name = value; }
           get { return _name; }
        }
        public string ParameterName
        {
           get 
           {
                if (_name.Split(char.Parse(" ")).GetLength(0) > 1)
                {
                     StringBuilder _temp = new StringBuilder();
                     string[] _nameArr = _name.Split(char.Parse(" "));
                     for (byte i = 0; i < _nameArr.GetLength(0); i++)
                     {
                          _temp.Append(_nameArr[i]);
                     }
                     _parameterName = _temp.ToString();
                }
                else
                     _parameterName=_name;

                _parameterName = @"@" + _parameterName.Substring(0, 1).ToLower() + _parameterName.Substring(1);

                return _parameterName;
           }
        }
        public int ID
        {
           set { _id = value; }
           get { return _id; }
        }
        public string DataType
        {
           set { _dataType = value; }
           get 
           {
                switch (_dataType.ToLower())
                {
                     case "char":
                     case "varchar":
                     case "binary":
                     case "varbinary":
                          if (_maxLength > 0)
                               _dataType += "(" + _maxLength.ToString() + ")";
                          else if(_maxLength==-1)
                               _dataType += "(MAX)";
                          break;
                     case "nchar":     
                     case "nvarchar":
                          if (_maxLength > 0)
                               _dataType += "(" + ((short)(_maxLength/2)).ToString() + ")";
                          else if (_maxLength == -1)
                               _dataType += "(MAX)";
                          break;
                     case "decimal":
                     case "numeric":
                          _dataType += "(" + _precision.ToString() + "," + _scale.ToString() + ")";
                          break; 
                     default: 
                          break;
                }
                return _dataType; 
           }
        }
        public byte Precision
        {
           set { _precision = value; }
           get { return _precision; }
        }
        public byte Scale
        {
           set { _scale = value; }
           get { return _scale; }
        }
        public short MaxLength
        {
           set { _maxLength = value; }
           get { return _maxLength; }
        }
        public bool IsIdentity
        {
           set { _isIdentity = value; }
           get { return _isIdentity; }
        }
        public bool IsRowGuid
        {
           set { _isRowGuid = value; }
           get { return _isRowGuid; }
        }
        public bool IsComputed
        {
           set { _isComputed = value; }
           get { return _isComputed; }
        }
		public bool IsRowversion
		{
			set { _isRowversion = value; }
			get { return _isRowversion; }
		}
        public bool IsPrimaryKey
        {
           set { _isPrimaryKey = value; }
           get { return _isPrimaryKey; }
        }
        public bool IsPessimisticConcurrencySupport
        {
	        set { _isPessimisticConcurrencySupport = value; }
	        get { return _isPessimisticConcurrencySupport; }
        }
        public bool IsOptimisticConcurrencySupport
        {
	        set { _isOptimisticConcurrencySupport = value; }
	        get { return _isOptimisticConcurrencySupport; }
        }
       
     }
}
