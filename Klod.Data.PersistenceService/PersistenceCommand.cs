using System;
using System.Collections.Generic;
using System.Text;
using Klod.Data.PersistenceService.Settings;
using Klod.Data.PersistenceService.Persistent;
namespace Klod.Data.PersistenceService
{
	//REFACTORING: Change object collection variable for processing with a IDictionary to include an unique ID. This is the better
	//solution to use same objects in the same command.
	//2012.05.18 crr 
	/// <summary>
	/// Abstract class that represent a CRUD command.
	/// PersistentCommand: a representation of a command operation to manage persistent objects
	/// in a storage(Database, XML document,...). Implementation of the command pattern.
	/// Command Pattern: Encapsute a request as an object, letting 
	/// parameterize clients, with different requests, queue (for transactions, by ex.)
	/// or log requests.
	/// </summary>
	public abstract class PersistenceCommand
	{
		private CommandMap _cmdMap = null;      //holds the command instructions
		//CHECK: Why this property is in both the persistentServer and persistentCommand

		//to store the field names of the concurrency support data
		private string _optimisticField = string.Empty;
		private string _pessimisticField = string.Empty;


		private IPersistable _pObj = null;
		private object _objectForProcess = null;
		private object _providerCommand = null;

		//REFACTORING: use a string token to unique identify same objects
		//2012.05.18 crr
		private IDictionary<string, object> _appObjs;

		#region properties
		public object ProviderCommand
		{
			set { _providerCommand = value; }
			get { return _providerCommand; }
		}
		public CommandMap CommandMap
		{
			set { _cmdMap = value; }
			get { return _cmdMap; }
		}
		public string OptimisticField
		{
			set { _optimisticField = value; }
			get { return _optimisticField; }
		}
		public string PessimisticField
		{
			set { _pessimisticField = value; }
			get { return _pessimisticField; }
		}
		public IPersistable ObjectToPersist
		{
			set { _pObj = value; }
			get { return _pObj; }
		}
		public object ObjectForProcess
		{
			set { _objectForProcess=value; }
			get { return _objectForProcess; }
		}
		public IDictionary<string,object> ObjectCollectionForProcess
		{
			set { _appObjs = value; }
			get { return _appObjs; }
		}
		//2010.04.07 crr a timeOut setting 
		public int TimeOut
		{
			get { return PersistenceServiceConfiguration.Default.CommandTimeout; }
		}
		#endregion

		#region constructor to all commands
		protected PersistenceCommand() { }

		/// <summary>
		/// PersistenceCommand for CRUD operations and concurrency support based on the command type.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="cmdType"></param>
		/// <param name="providerCommand"></param>
		public PersistenceCommand(Map map, CRUD cmdType, object providerCommand)
		{
			_providerCommand = providerCommand;
			_cmdMap = map.GetCommandMap(cmdType.ToString());
			SetConcurrencySupport(cmdType);
			
		}

		/// <summary>
		/// PersistenceCommand for CRUD operations and concurrency support based on the command type. Support multiple objects
		/// for process identified with a string ID.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="cmdType"></param>
		/// <param name="providerCommand"></param>
		/// <param name="objectsForProcess"></param>
		public PersistenceCommand(Map map, CRUD cmdType, object providerCommand, IDictionary<string,object> objectsForProcess)
		{
			_appObjs = objectsForProcess;
			_providerCommand = providerCommand;
			_cmdMap = map.GetCommandMap(cmdType.ToString());
			SetConcurrencySupport(cmdType);
		}
		/// <summary>
		/// PersistentSupport for CRUD(insert) operations.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="objectForProcess"></param>
		/// <param name="providerCommand"></param>
		public PersistenceCommand(Map map, object objectForProcess, object providerCommand)
		{
			_providerCommand = providerCommand;

			_objectForProcess = objectForProcess; //the new object
			_cmdMap = map.GetCommandMap(CRUD.Create.ToString()); //must get the proper command instructions from the map
		}

		//TEST
		/// <summary>
		/// PersistentCommand for new records with identifier.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="oid"></param>
		/// <param name="objectForProcess"></param>
		/// <param name="providerCommand"></param>
		public PersistenceCommand(Map map, ObjectIdentifier oid,object objectForProcess, object providerCommand)
		{
			_providerCommand = providerCommand;
			_objectForProcess = objectForProcess;
			_pObj = MakePersistable(objectForProcess, oid);
			_cmdMap = map.GetCommandMap(CRUD.Create.ToString());
		}

		/// <summary>
		/// PersistentCommand for custom commands, concurrency and collection support
		/// </summary>
		/// <param name="map"></param>
		/// <param name="providerCommand"></param>
		/// <param name="cmdType"></param>
		/// <param name="concurrency"></param>
		/// <param name="returnCollection"></param>
		public PersistenceCommand(Map map, string cmdTypeIdentifier, object providerCommand, ConcurrencySupportType concurrency, bool returnCollection)
		{
			_providerCommand = providerCommand;
			_cmdMap = map.GetCommandMap(cmdTypeIdentifier);   //get the custom command instruction from the map

			if (!returnCollection)
				_pObj = MakePersistable();

			SetConcurrencySupport(concurrency);
		}

		/// <summary>
		/// PersistentSupport for CRUD (update and delete) operations and possible concurrency support
		/// </summary>
		/// <param name="map"></param>
		/// <param name="objectForProcess"></param>
		/// <param name="providerCommand"></param>
		/// <param name="cmdType"></param>
		public PersistenceCommand(Map map, IPersistable objectForProcess, object providerCommand, CRUD cmdType)
		{
			_providerCommand = providerCommand;

			_pObj = objectForProcess; //insert,updates and delete need the object for processing. Retrieve operations need only the OID
			_cmdMap = map.GetCommandMap(cmdType.ToString()); //must get the proper command instructions from the map

			SetConcurrencySupport(cmdType);
		}

		/// <summary>
		/// PersistentObject for CRUD (Update and delete) operations, custom command and concurrency support.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="objectForProcess"></param>
		/// <param name="providerCommand"></param>
		/// <param name="cmdType"></param>
		/// <param name="concurrency"></param>
		public PersistenceCommand(Map map, IPersistable objectForProcess, object providerCommand, string cmdType, ConcurrencySupportType concurrency)
		{
			_providerCommand = providerCommand;

			_cmdMap = map.GetCommandMap(cmdType); //must get the proper command instruction from the map
			_pObj = objectForProcess;

			SetConcurrencySupport(concurrency);
		}

		/// <summary>
		/// PersistentSupport for CRUD (retrieve) operations and possible concurrency support
		/// </summary>
		/// <param name="map"></param>
		/// <param name="oid"></param>
		/// <param name="providerCommand"></param>
		/// <param name="cmdType"></param>
		public PersistenceCommand(Map map, ObjectIdentifier oid, object providerCommand, CRUD cmdType)
		{
			_providerCommand = providerCommand;

			_pObj = MakePersistable(null, oid);// Retrieve operations need only the OID
			_cmdMap = map.GetCommandMap(cmdType.ToString()); //must get the proper command instructions from the map

			SetConcurrencySupport(cmdType);
		}

		/// <summary>
		/// PersistentObject for retrieve operations, custom command and concurrency support.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="oid"></param>
		/// <param name="providerCommand"></param>
		/// <param name="cmdType"></param>
		/// <param name="concurrency"></param>
		public PersistenceCommand(Map map, ObjectIdentifier oid, object providerCommand, string cmdType, ConcurrencySupportType concurrency)
		{
			_providerCommand = providerCommand;
			_pObj = MakePersistable(null, oid);// Retrieve operations need only the OID
			_cmdMap = map.GetCommandMap(cmdType);   //must get the proper command instruction from the map

			SetConcurrencySupport(concurrency);
		}
		
		/// <summary>
		/// PersistentCommand for custom commands, concurrency, collection and multiple processed objects support
		/// </summary>
		/// <param name="map"></param>
		/// <param name="cmdTypeIdentifier"></param>
		/// <param name="providerCommand"></param>
		/// <param name="concurrency"></param>
		/// <param name="returnCollection"></param>
		/// <param name="objectsForProcess"></param>
		public PersistenceCommand(Map map, string cmdTypeIdentifier, object providerCommand, ConcurrencySupportType concurrency, bool returnCollection, IDictionary<string,object> objectsForProcess)
		{
			_providerCommand = providerCommand;
			_cmdMap = map.GetCommandMap(cmdTypeIdentifier);   //get the custom command instruction from the map

			if(!returnCollection)
				_pObj = MakePersistable();
			_appObjs = objectsForProcess;
			SetConcurrencySupport(concurrency);
		}
		#endregion

		protected void SetConcurrencySupport(ConcurrencySupportType concurrency)
		{
			if (concurrency == ConcurrencySupportType.None)
			{
				_optimisticField = string.Empty;
				_pessimisticField = string.Empty;
			}
			else if (concurrency == ConcurrencySupportType.Optimistic)
			{
				_optimisticField = PersistenceServiceConfiguration.Default.optimisticFieldName;
				_pessimisticField = string.Empty;
			}
			else if ((concurrency == ConcurrencySupportType.PessimisticUniqueIdentifier) || (concurrency == ConcurrencySupportType.PessimisticUserName))
			{
				_optimisticField = string.Empty;
				_pessimisticField = PersistenceServiceConfiguration.Default.pessimisticFieldName;
			}
		}
		protected void SetConcurrencySupport(CRUD cmdType)
		{
			//concurrency support settings
			if ((cmdType == CRUD.DeleteCheckTimestamp) || (cmdType == CRUD.RetrieveWithTimestamp) | (cmdType == CRUD.UpdateCheckTimestamp))
			{
				_optimisticField = PersistenceServiceConfiguration.Default.optimisticFieldName;
				_pessimisticField = string.Empty;
			}
			else if ((cmdType == CRUD.DeleteCheckLock) || (cmdType == CRUD.RetrieveAndLock) || (cmdType == CRUD.UpdateCheckLock))
			{
				_optimisticField = string.Empty;
				_pessimisticField = PersistenceServiceConfiguration.Default.pessimisticFieldName;
			}
			else
			{
				_optimisticField = string.Empty;
				_pessimisticField = string.Empty;
			}
		}

		public abstract IPersistable MakePersistable();
		public abstract IPersistable MakePersistable(object persistableObj, ObjectIdentifier oid);
		/// <summary>
		/// Abstract method to Run the operation. Must be implemeted by subtypes executing the providerCommand.
		/// </summary>
		/// <returns></returns>
		public abstract object Run();
		//public abstract object[] Run(bool returnMultiple);

	}
}
