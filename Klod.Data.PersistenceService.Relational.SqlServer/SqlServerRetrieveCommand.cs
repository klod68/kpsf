using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;

using Klod.Data.PersistenceService.Identifiers;

namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	//ok crr 2009.11.05
	/// <summary>
	/// Implementation of retrieve command to get single or multiple records.
	/// </summary>
	public class SqlServerRetrieveCommand : SqlServerPersistenceCommand
	{
		//variables for retrieve operation
        //TODO: change to private scope
		protected string[] _propertyArray2;
		protected string[] _columnArray;
		protected string _tableName;
		protected SqlDataAdapter _da;
		protected DataSet _ds;
		protected IList<object> _criterias;

		protected Type _type;

		#region constructors
		//Create a retrieveCommand for all objects
		public SqlServerRetrieveCommand(Map map, Type type, SqlCommand command)
			: base(map, CRUD.RetrieveAll, command) { _type = type; } //1 sql pc constructor

		//Retrieve all with a custom command
		public SqlServerRetrieveCommand(Map map, string cmdType, SqlCommand command, Type type, ConcurrencySupportType concurrency, bool returnCollection)
			: base(map, cmdType, command, concurrency, returnCollection) { _type = type; } //7 SQL pc constructor

		//Create the default retrieveCommand
		public SqlServerRetrieveCommand(Map map, ObjectIdentifier oid, SqlCommand command, Type type)
			: base(map, oid, command, CRUD.Retrieve) { _type = type; } //4 sql pc constructor

		//Create a known-type command with OID
		public SqlServerRetrieveCommand(Map map, ObjectIdentifier oid, SqlCommand command, Type type, CRUD cmdType)
			: base(map, oid, command, cmdType) { _type = type; } //4 sql pc constructor

		//Create a custom command with an OID
		public SqlServerRetrieveCommand(Map map, ObjectIdentifier oid, SqlCommand command, Type type, string cmdType, ConcurrencySupportType concurrency)
			: base(map, oid, command, cmdType, concurrency) { _type = type; } //6 sql pc constructor

		public SqlServerRetrieveCommand(Map map, string cmdType, SqlCommand command, Type type, ConcurrencySupportType concurrency, object criteria, bool returnCollection)
			: base(map, cmdType, command, concurrency, returnCollection) //7 sql pc constructor
		{
			_criterias = new List<object>();
			_criterias.Add(criteria);

			_type = type;
		}

		////Create a custom command with multiple custom criterias. Collection and IPersistable support 
		public SqlServerRetrieveCommand(Map map, string cmdType, SqlCommand command, Type type, ConcurrencySupportType concurrency, bool returnCollection, IList<object> criterias)
			: base(map, cmdType, command, concurrency, returnCollection) //7 SQL PC constructor
		{
			_criterias = new List<object>();
			_criterias = criterias;
			_type = type;

		}
		#endregion

		//Make the array for the properties that will be retrieved from objects
		protected override void MakeParametersArray()
		{
			base.MakeParametersArray();
			int counter = 0;
			_tableName = CommandMap.GetReturnTableName();
			counter = CommandMap.GetRetrievedColumnsCount();
			_propertyArray2 = new string[counter];
			_columnArray = new string[counter];

			counter = CommandMap.GetRetrievedColumnsCount();

			for (byte i = 0; i < counter; i++)
			{
				_propertyArray2[i] = CommandMap.GetRetrieveProperty(i);
				_columnArray[i] = CommandMap.GetRetrieveColumn(i);

			}
		}
		//set the parameter values with the incoming criterias, if any
		protected override void SetParametersValues()
		{
			int counter = 0;

			if (_criterias == null)
			{
				//check if there are any oid
				if (ObjectToPersist != null)
				{
					ParametersSet[0].Value = ObjectToPersist.OID.ID;
					counter = ParametersSet.GetLength(0);

					for (byte i = 1; i < counter; i++)
					{
						if (ParametersSet[i].Use == ParameterUse.Regular)
							ParametersSet[i].Value = ObjectToPersist.GetType().GetProperty(CommandMap.GetProperty(i)).GetValue(ObjectToPersist, null);
						else if (ParametersSet[i].Use == ParameterUse.UserName)
							ParametersSet[i].Value = WindowsIdentity.GetCurrent().Name;
						else if (ParametersSet[i].Use == ParameterUse.UserId)
							ParametersSet[i].Value = WindowsIdentity.GetCurrent().User.Value;
					}
				}
			}
			else
			{
				for (byte i = 0; i < _criterias.Count; i++)
				{
					ParametersSet[i].Value = _criterias[i];
				}
			}
		}
		//Return a DataView for multiple records(as a cursor object) 
		//or return a persistentObject (as a record)
		protected override Object ExecuteCommand()
		{
			try
			{
				//get the data item as DataView
				SetDataAdapter();
				_da.Fill(_ds, _tableName);

				if (_ds.Tables[_tableName] == null)
					return null;

				DataView _dvw = _ds.Tables[_tableName].DefaultView;
				if (ObjectToPersist != null)//What if it has one _criteria?
					return MapRecordToObject(_dvw);

				return _dvw;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected void SetDataAdapter()
		{
			_ds = new DataSet();
			_da = new SqlDataAdapter((SqlCommand)ProviderCommand);
			//2010.04.07 crr
			((SqlCommand)_da.SelectCommand).CommandTimeout = base.TimeOut;
		}

		/// <summary>
		///Return a persistentObject with a storedObject of the persisted object
		///and concurrency support data.
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		protected IPersistable MapRecordToObject(DataView record)
		{

			try
			{
				if (record == null || record.Count == 0)
					return null;

				object obj = null;
				object version = null;
				object inUse = null;

				if (!_type.IsAbstract && !_type.IsInterface)
					obj = Activator.CreateInstance(_type);
				else
				{
					return null;
				}
				int counter = CommandMap.GetRetrievedColumnsCount();

				for (byte i = 0; i < counter; i++)
				{
					//if no property or is null then continue the loop
					if ((_propertyArray2[i] == "") || (record[0][_columnArray[i]].GetType() == Type.GetType("System.DBNull")))
						continue;

					//set reference objectidentifier properties for foreign keys
					if (_propertyArray2[i] == "ID") //then you need a collection of oids. TODO: Please use a variable in a configuratin file!!!
					{
						OID oid = new OID(record[0][_columnArray[i]], _columnArray[i], IDDataType.NotSet, IDType.NotSet);

						if (CommandMap.IsRetrievedColumObjectIdentifier(i))
						{
							ObjectToPersist.OID = oid;
							continue;
						}
						ObjectToPersist.AddReference(oid);
						continue;
					}

					//Setting values depending on the object map for the returned table.
					obj.GetType().GetProperty(_propertyArray2[i]).SetValue(obj, record[0][_columnArray[i]], null);

				}

				//return a persistentObject
				//Add concurrency support info in the persistenObject (version, inUse)
				if ((OptimisticField != null) && (OptimisticField != string.Empty))
					version = record[0][OptimisticField];//get field value for optimistic support
				if ((PessimisticField != null) && (PessimisticField != string.Empty))
					inUse = record[0][PessimisticField];//get field value for pessimistic support

				ObjectToPersist.StoredObject = obj;
				//ObjectToPersist = (IPersistable)obj;
				ObjectToPersist.Version = version;
				ObjectToPersist.InUse = inUse;

				return ObjectToPersist;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}
