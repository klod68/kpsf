using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Klod.Data.PersistenceService.Relational
{
	/// <summary>
	/// DataType class that represents a parameter in a stored procedure
	/// </summary>
	public class Parameter
	{
		#region variables
		private string _name;
		private object _value;
		private DbType _type;
		private ParameterDirection _direction;
		private ParameterUse _use;
		#endregion

		#region properties
		public string Name
		{
			set { _name = value; }
			get { return _name; }
		}
		public Object Value
		{
			set { _value = value; }
			get { return _value; }
		}
		public DbType Type
		{
			set { _type = value; }
			get { return _type; }
		}
		public ParameterDirection Direction
		{
			set { _direction = value; }
			get { return _direction; }
		}
		public ParameterUse Use
		{
			set { _use = value; }
			get { return _use; }
		}
		#endregion

		#region constructors
		public Parameter(string name, object value, DbType type, ParameterDirection direction, ParameterUse use)
		{
			if (!name.StartsWith("@"))
				name = "@" + name;

			_value = value;
			_name = name;
			_direction = direction;
			_type = type;
			_use = use;
		}
		public Parameter(string name, object value, DbType type, ParameterDirection direction)
		{
			if (!name.StartsWith("@"))
				name = "@" + name;

			_value = value;
			_name = name;
			_direction = direction;
			_type = type;
		}

		public Parameter(string name, object value, DbType type)
		{
			if (!name.StartsWith("@"))
				name = "@" + name;

			_name = name;
			_value = value;
			_direction = ParameterDirection.Input;
			_type = type;
		}

		public Parameter(string name, object value)
		{
			if (!name.StartsWith("@"))
				name = "@" + name;

			_name = name;
			_value = value;
			_direction = ParameterDirection.Input;
			_type = DbType.String;
		}
		#endregion

	}
}
