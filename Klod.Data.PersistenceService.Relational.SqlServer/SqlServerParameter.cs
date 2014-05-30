using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	//ok crr 2009.11.05
	/// <summary>
	/// Lite implementation of a Sql server paremeter.
	/// </summary>
	public class SqlServerParameter
	{
		private SqlParameter _parameter;
		private ParameterUse _use;



		public SqlServerParameter()
		{
			_parameter = new SqlParameter();
		}

		public SqlServerParameter(string name, object value, SqlDbType type, ParameterDirection direction, ParameterUse use)
		{
			_parameter = new SqlParameter();
			_parameter.ParameterName = name;
			_parameter.Value = value;
			_parameter.SqlDbType = type;
			_parameter.Direction = direction;
			_use = use;
		}

		public string Name
		{
			set { _parameter.ParameterName = value; }
			get { return _parameter.ParameterName; }
		}

		public SqlDbType SqlDbType
		{
			set { _parameter.SqlDbType = value; }
			get { return _parameter.SqlDbType; }
		}

		public object Value
		{
			set { _parameter.Value = value; }
			get { return _parameter.Value; }
		}

		public DbType DbType
		{
			set { _parameter.DbType = value; }
			get { return _parameter.DbType; }
		}

		public ParameterDirection Direction
		{
			set { _parameter.Direction = value; }
			get { return _parameter.Direction; }
		}

		public SqlParameter SqlParameter
		{
			get { return _parameter; }
		}
		public ParameterUse Use
		{
			set { _use = value; }
			get { return _use; }
		}

	}
}
