using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.Data;
using System.Threading;
using Klod.Data.PersistenceService;
using Klod.Data.PersistenceService.Persistent;
using Klod.Data.PersistenceService.Identifiers;

namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	public abstract class SqlServerPersistenceCommand : PersistenceCommand
	{
		#region variables
		//sql variables for all operations
		private SqlServerParameter[] _paramArray;

		public SqlServerParameter[] ParametersSet
		{
			set { _paramArray = value; }
			get { return _paramArray; }
		}

		#endregion
		#region public methods
		//used for Retrieve all operations
		public SqlServerPersistenceCommand(Map map, CRUD cmdType, SqlCommand command)
			: base(map, cmdType, command) { } //2 PC constructor

		//Used for Create commands
		public SqlServerPersistenceCommand(Map map, object objectForProcess, SqlCommand command)
			: base(map, objectForProcess, command) { }//set command type using the CRUD enumeration //3 PC constructor

		//Used for Update and Delete commands
		public SqlServerPersistenceCommand(Map map, IPersistable objectForProcess, SqlCommand command, CRUD cmdType)
			: base(map, objectForProcess, command, cmdType) { }//set command type using the CRUD enumeration //4 PC constructor


		//used for Retrieve and delete with an OID and a well-known command type
		public SqlServerPersistenceCommand(Map map, ObjectIdentifier oid, SqlCommand command, CRUD cmdType)
			: base(map, oid, command, cmdType) { }//set command type using the CRUD enumeration //5 PC constructor


		//Used for Updates and Deletes with a persistent object, a custom command and concurrency suppport
		public SqlServerPersistenceCommand(Map map, IPersistable objectForProcess, SqlCommand command, string cmdType, ConcurrencySupportType concurrency)
			: base(map, objectForProcess, command, cmdType, concurrency) { } //6 PC constructor

		//used for Retrieve and Delete with an OID, a custom command type and concurrency support
		public SqlServerPersistenceCommand(Map map, ObjectIdentifier oid, SqlCommand command, string cmdType, ConcurrencySupportType concurrency)
			: base(map, oid, command, cmdType, concurrency) { } //7 PC constructor

		//Used for Retrieve custom commands that return collections.
		public SqlServerPersistenceCommand(Map map, string cmdType, SqlCommand command, ConcurrencySupportType concurrency, bool returnCollection)
			: base(map, cmdType, command, concurrency, returnCollection) { } //8 PC constructor		

		public SqlServerPersistenceCommand(Map map, SqlCommand command, CRUD cmdType, IDictionary<string,object> objectsForProcess)
			: base(map, cmdType, command, objectsForProcess) { }

		public SqlServerPersistenceCommand(Map map, string cmdTypeIdentifier, SqlCommand command, ConcurrencySupportType concurrency, bool returnCollection, IDictionary<string,object> objectsForProcess)
			: base(map, cmdTypeIdentifier, command, concurrency, returnCollection, objectsForProcess) { } 

		/// <summary>
		/// Command pattern: Run method implementation. Template method to be implemented by subclasses.
		/// </summary>
		/// <returns></returns>
		public override object Run()
		{
			try
			{
				SetCommand();
				MakeParametersArray();
				SetParametersValues();

				SetCommandParameters();

				//Hook method to be implemented by subtypes
				return ExecuteCommand();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}
		#endregion
		#region protected methods

		protected void SetCommand()
		{
			try
			{
				((SqlCommand)ProviderCommand).CommandText = CommandMap.Name;
				if (CommandMap.Name.Contains("adhoc"))
					((SqlCommand)ProviderCommand).CommandType = CommandType.Text;
				((SqlCommand)ProviderCommand).CommandType = CommandType.StoredProcedure; //WARNING: Hard coded to stored procs. Do ad hoc queries also, please!!

			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}
		protected virtual void MakeParametersArray()
		{
			_paramArray = new SqlServerParameter[CommandMap.ParametersCount];

			SqlDbType type = SqlDbType.VarChar;
			ParameterDirection direction = ParameterDirection.Input;
			ParameterUse use = ParameterUse.Regular;

			for (byte i = 0; i < CommandMap.ParametersCount; i++)
			{
				if (CommandMap.GetParameterDataType(i) != "")
					type = GetDbTypeFromRelationalType(CommandMap.GetParameterDataType(i));

				if (CommandMap.GetParameterDirection(i) != "")
					direction = (ParameterDirection)Enum.Parse(direction.GetType(), CommandMap.GetParameterDirection(i));

				if (CommandMap.GetParameterUse(i) != "")
					use = (ParameterUse)Enum.Parse(use.GetType(), CommandMap.GetParameterUse(i));


				_paramArray[i] = new SqlServerParameter(CommandMap.GetParameter(i), null, type, direction, use);

			}

		}

		protected abstract void SetParametersValues();

		protected void SetCommandParameters()
		{
			try
			{
				if (_paramArray == null || ProviderCommand == null)
					return;
				((SqlCommand)ProviderCommand).Parameters.Clear();

				foreach (SqlServerParameter parm in _paramArray)
				{
					((SqlCommand)ProviderCommand).Parameters.Add(parm.SqlParameter);
				}
			}
			catch 
			{
			}

		}

		//hook method for Run custom implementation
		//CHECK: Review the connection open/close process
		protected virtual object ExecuteCommand()
		{
			try
			{
				byte retry = 0;
				((SqlCommand)ProviderCommand).Connection.Open();

				//2010.04.07 crr
				((SqlCommand)ProviderCommand).CommandTimeout = base.TimeOut;

				if (((SqlCommand)ProviderCommand).Connection.State != ConnectionState.Open)
				{
					SqlConnection.ClearPool(((SqlCommand)ProviderCommand).Connection);
					//retry to open
					((SqlCommand)ProviderCommand).Connection.Open();
				}
			Retry:
				if (((SqlCommand)ProviderCommand).Connection.State == ConnectionState.Open)
				{
					((SqlCommand)ProviderCommand).ExecuteNonQuery();
					if (((SqlCommand)ProviderCommand).Parameters.Contains("@RETURN_VALUE"))
						return GetObjectIdenfierFromReturnValue(((SqlCommand)ProviderCommand).Parameters["@RETURN_VALUE"].Value);//return an ObjectIdentifier

					return true;
				}
				else
				{
					if (retry == 1)
					{
						throw new Exception("Cannot open a connection.");
					}
					Thread.Sleep(500);
					retry++;
					goto Retry;
				}
			}
			catch (SqlException ex)
			{
				throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				((SqlCommand)ProviderCommand).Connection.Close();
				//((SqlCommand)ProviderCommand).Connection.Dispose();
			}
		}

		//helper methods
		protected SqlDbType GetDbTypeFromRelationalType(string type)
		{
			try
			{
				type = FormatType(type);
				SqlDbType _type = SqlDbType.VarChar;
				_type = (SqlDbType)Enum.Parse(_type.GetType(), type, true);
				return _type;
			}
			catch (Exception ex)
			{
				throw new Exception("Error setting the SqlDbType.", ex);
			}
		}

		//Refactoring: Encapsulate multiple if...then logic in a method
		protected string FormatType(string type)
		{

			if (type.IndexOf('(') > 0)
				type = type.Substring(0, type.IndexOf('('));

			if (type.IndexOf('[') > 0)
				type = type.Substring(0, type.IndexOf('['));
			return type;
		}

		//Refactoring: Encapsulate conditional logic into a method
		protected bool IsNumber(string type)
		{
			return (type.Contains("int") || type.Contains("money") || //int: this includes all kind of integers
				type.Contains("decimal") || type.Contains("numeric")) ||
				type.Contains("float") || type.Contains("real");
		}
		
		//Refactoring: Encapsulate conditional login into a method
		protected IDDataType GetIDDataTypeFromRelationalType(string type)
		{
			IDDataType _type = IDDataType.Number;

			if (IsNumber(type))
				_type = IDDataType.Number;
			else if (type.Contains("uniqueidentifier"))
				_type = IDDataType.Guid;
			else if (type.Contains("char"))
				_type = IDDataType.String;

			return _type;
		}

		protected ObjectIdentifier GetObjectIdenfierFromReturnValue(object returnValue)
		{
			OID oid = new OID();
			oid.ID = returnValue;
			return oid;
		}
		#endregion

		public override IPersistable MakePersistable()
		{
			return new PersistentObject();
		}

		public override IPersistable MakePersistable(object persistableObj, ObjectIdentifier oid)
		{
			return new PersistentObject(persistableObj, oid);
		}
	}
}
