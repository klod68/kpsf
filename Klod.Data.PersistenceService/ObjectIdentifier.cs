using System;
using System.Collections.Generic;
using System.Text;

namespace Klod.Data.PersistenceService
{
	/// <summary>
	/// Base class for an ObjectIdentifier.
	/// </summary>
	public abstract class ObjectIdentifier
	{
		private IDDataType _dataType;
		private IDType _type;
		private object _id;
		private Type _objType;
		private string _fieldName; //represent the referenced column or field in the persistent storage.

		/// <summary>
		/// Data type of the ID (numeric, guid or string).
		/// </summary>
		public IDDataType DataType
		{
			set { _dataType = value; }
			get { return _dataType; }
		}
		public object ID
		{
			set
			{
				if (value != null && value != DBNull.Value)
					_id = value;
			}
			get
			{
				if (_id != null && _id != DBNull.Value)
					return _id;
				return null;
			}
		}
		/// <summary>
		/// Type of the ID: for an object, for a persistent object or a reference object.
		/// </summary>
		public IDType Type
		{
			set { _type = value; }
			get { return _type; }
		}

		/// <summary>
		/// Type value of the referenced object.
		/// </summary>
		public Type ReferenceObject
		{
			set { _objType = value; }
			get { return _objType; }
		}

		public string FieldName
		{
			set { _fieldName = value; }
			get { return _fieldName; }
		}
		public ObjectIdentifier() { }
		public ObjectIdentifier(object id)
		{
			_id = id;
		}
		public ObjectIdentifier(object id, string fieldName)
		{
			_fieldName = fieldName;
			_id = id;
		}
		
		public ObjectIdentifier(object id, string fieldName, IDDataType dataType)
		{
			_id = id;
			_fieldName = fieldName;
			_dataType = dataType;
		}
		
		public ObjectIdentifier(object id, string fieldName, IDDataType dataType, IDType type)
		{
			_id = id;
			_fieldName = fieldName;
			_dataType = dataType;
			_type = type;

		}

		public byte ToByte()
		{
			Byte b = 0;
			if (Byte.TryParse(_id.ToString(), out b))
				return b;
			return b;
		}
		public short ToInt16()
		{
			short s = 0;
			if (Int16.TryParse(_id.ToString(), out s))
				return s;
			return s;
		}
		public int ToInt32()
		{
			int i = 0;
			if (Int32.TryParse(_id.ToString(), out i))
				return i;
			return i;
		}
		public long ToInt64()
		{
			long l;
			if (Int64.TryParse(_id.ToString(), out l))
				return l;
			return l;
		}
		public Guid ToGuid()
		{
			try
			{
				Guid guid = new Guid(_id.ToString());
				return guid;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


	}
}
