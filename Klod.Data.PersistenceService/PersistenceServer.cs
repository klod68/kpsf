using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Configuration;

using Klod.Data.PersistenceService.Settings;
using Klod.Data.PersistenceService.Resources;

namespace Klod.Data.PersistenceService
{
	//REFACTORING: Can this class be defined as a Monostate? (See Martin...)
	//2012.05.28 crr: Change all protected attributes to private. Add getters and setters.
	//Template methods (Store, Change, Retrieve and Destroy): define the skeleton of an algorithm,
	//deferring some steps to subclasses.
	//Factory method (SetServer): defer implementation of the creation of objects to subtypes.
	//Facade: this class is the entry point of the whole persistence sub-system.
	/// <summary>
	/// This abstract class implements multiple GRASP principles and design patterns
	/// </summary>
	public abstract class PersistenceServer
	{
		#region constants variables
		const string CANNOT_STORE_NEW = "Cannot store the new object.";
		const string CANNOT_UPDATE = "Cannot update the object.";
		const string CANNOT_RETRIEVE = "Cannot retrieve the object.";
		const string CANNOT_DESTROY = "Cannot destroy the object.";
		const string CANNOT_TRANSACTION = "Cannot make the transaction.";
		const string NO_MAP = "Cannot load the map.";
		const string NO_CMD_MAP = "Cannot load the command map.";
		#endregion

		#region private attributes
		//Use abstract classes for objects properties. 
		//make them private.
		private PersistenceCommand _cmd = null; //delegate operations
		private Map _map = null;
		private object _providerConnection = null; //represent any object connection from a source provider
		private string _connectionString = string.Empty;
		private string _connectionName = string.Empty;
		private ConcurrencySupportType _concurrency = ConcurrencySupportType.None;
		#endregion
		
		#region constructors
		public PersistenceServer(){}

		public PersistenceServer(object providerConnection)
		{
			_providerConnection = providerConnection;
		}
		public PersistenceServer(string connectionName)
		{
			if(connectionName == string.Empty)
				return;

			_connectionName = connectionName;
			_connectionString = GetConnectionStringByName(connectionName);
			_providerConnection = MakeProviderConnection();
		}
		public PersistenceServer(object providerConnection, ConcurrencySupportType concurrency)
		{
			_providerConnection = providerConnection;
			_concurrency = concurrency;
		}
		public PersistenceServer(string connectionString, string connectionName)
		{
			_connectionString = connectionString;

			_providerConnection = MakeProviderConnection();
			if(connectionName != string.Empty)
				StoreConnectionString(connectionString, connectionName);
		}
		public PersistenceServer(string connectionString, string connectionName, ConcurrencySupportType concurrency)
		{
			_concurrency = concurrency;
			_connectionString = connectionString;
			_providerConnection = MakeProviderConnection();
			if(connectionName != string.Empty)
				StoreConnectionString(_connectionString, connectionName);
		}
		public PersistenceServer(string connectionName, ConcurrencySupportType concurrency)
		{
			if(connectionName == string.Empty)
				return;
			_concurrency = concurrency;
			_connectionName = connectionName;
			_connectionString = GetConnectionStringByName(connectionName);
			_providerConnection = MakeProviderConnection();
		}
		#endregion

		#region public properties
		/// <summary>
		/// Store the underlying persistent command that execute the related operation: (store, change, destroy or retrive).
		/// </summary>
		public PersistenceCommand Command
		{
			set { _cmd = value; }
			get { return _cmd; }
		}
		/// <summary>
		/// Store the set of instructions for the command run operation.
		/// </summary>
		public Map Map
		{
			set { _map = value; }
			get { return _map; }
		}
		/// <summary>
		/// Store the object thar represents a connection for specific providers.
		/// </summary>
		public object ProviderConnection
		{
			set { _providerConnection = value; }
			get { return _providerConnection; }
		}
		/// <summary>
		///A property to store specific provider connection data.
		///Support multiple connections. 
		/// </summary>
		public string ConnectionString
		{
			set { _connectionString = value; }
			get { return _connectionString; }
		}
		/// <summary>
		/// Give a name for id the connection string in a setting file.
		/// </summary>
		public string ConnectionName
		{
			set { _connectionName = value; }
			get { return _connectionName; }
		}
		/// <summary>
		/// Store and retrieve the ConcurrencySupportType 
		/// for optimistic and pessimistic concurrency support.
		/// </summary>
		public ConcurrencySupportType ConcurrencySupport
		{
			set { _concurrency = value; }
			get { return _concurrency; }
		}
		#endregion

		#region set server properties

		#region deprecated code
				//The SetServer method set the server and return one of them. This object
		//must be created by a Factory to instantiate the appropiate concrete PersistenceServer.
		/// <summary>
		/// Factory method that support late binding to a concrete PersistenceServer.
		/// Use configuration files for all the settings.
		/// </summary>
		/// <returns></returns>
		//public static PersistenceServer SetServer()
		//{
		//     PersistenceServer server;
		//     Assembly a = Assembly.Load(PersistenceServiceConfiguration.Default.PersistenceServiceAssembly);
		//     Type t = a.GetType(PersistenceServiceConfiguration.Default.PersistenceServerClass);
		//     server = (PersistenceServer)Activator.CreateInstance(t, PersistenceServiceConfiguration.Default.Connection, PersistenceServiceConfiguration.Default.ConnectionName);
		//     server.ConcurrencySupport = (ConcurrencySupportType)Enum.Parse(typeof(ConcurrencySupportType), PersistenceServiceConfiguration.Default.ConcurrencySupport);
		//     return server;
		//}
		/// <summary>
		/// Factory method that support late binding to a concrete PersistenceServer.
		/// Use the a parameter for the connectionName stored in the configuration file.
		/// Other parameters are the default in the configuration file.
		/// </summary>
		/// <param name="connectionName"></param>
		/// <returns></returns>
		//public static PersistenceServer SetServer(string connectionName)
		//{
		//     PersistenceServer server;
		//     Assembly a = Assembly.Load(PersistenceServiceConfiguration.Default.PersistenceServiceAssembly);
		//     Type t = a.GetType(PersistenceServiceConfiguration.Default.PersistenceServerClass);
		//     server = (PersistenceServer)Activator.CreateInstance(t, connectionName);
		//     server.ConcurrencySupport = (ConcurrencySupportType)Enum.Parse(typeof(ConcurrencySupportType), PersistenceServiceConfiguration.Default.ConcurrencySupport);
		//     return server;
		//}
		//public static PersistenceServer SetServer(string connectionString, string connectionName)
		//{
		//     PersistenceServer server;
		//     Assembly a = Assembly.Load(PersistenceServiceConfiguration.Default.PersistenceServiceAssembly);
		//     Type t = a.GetType(PersistenceServiceConfiguration.Default.PersistenceServerClass);
		//     server = (PersistenceServer)Activator.CreateInstance(t, connectionString, connectionName);
		//     server.ConcurrencySupport = (ConcurrencySupportType)Enum.Parse(typeof(ConcurrencySupportType), PersistenceServiceConfiguration.Default.ConcurrencySupport);
		//     return server;
		//}
		/// <summary>
		/// Factory method that support late binding to a concrete PersistenceServer.
		/// Use the a setting file property (ConnectionName) to get the connectionString.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="persistenceServerType"></param>
		/// <param name="connectionName"></param>
		/// <returns></returns>
		//public static PersistenceServer SetServer(string assembly, string persistenceServerType, string connectionName)
		//{
		//     PersistenceServer server;
		//     Assembly a = Assembly.Load(assembly);
		//     Type t = a.GetType(persistenceServerType);
		//     server = (PersistenceServer)Activator.CreateInstance(t, connectionName);
		//     return server;
		//}
		/// <summary>
		/// Factory method that support late binding to a concrete PersistenceServer.
		/// This method gets the connectionString from a configuration file and store it.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="persistenceServerType"></param>
		/// <param name="connectionString"></param>
		/// <param name="connectionName"></param>
		/// <returns></returns>
		//public static PersistenceServer SetServer(string assembly, string persistenceServerType, string connectionString, string connectionName)
		//{
		//     PersistenceServer server;
		//     Assembly a = Assembly.Load(assembly);
		//     Type t = a.GetType(persistenceServerType);
		//     server = (PersistenceServer)Activator.CreateInstance(t, connectionString, connectionName);
		//     server.ConcurrencySupport = ConcurrencySupportType.None;
		//     return server;
		//}
		/// <summary>
		/// Factory method that support late binding to a concrete PersistenceServer.
		/// This method gets the connectionString from a configuration file and store it.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="persistenceServerType"></param>
		/// <param name="connectionString"></param>
		/// <param name="connectionName"></param>
		/// <param name="concurrency"></param>
		/// <returns></returns>
		//public static PersistenceServer SetServer(string assembly, string persistenceServerType, string connectionString, string connectionName, ConcurrencySupportType concurrency)
		//{
		//     PersistenceServer server;
		//     Assembly a = Assembly.Load(assembly);
		//     Type t = a.GetType(persistenceServerType);
		//     server = (PersistenceServer)Activator.CreateInstance(t, connectionString, connectionName);
		//     server.ConcurrencySupport = concurrency;
		//     return server;
		//}

		#endregion
		/// <summary>
		/// Set connection properties if the default constructor is used.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="connectionName"></param>
		public void SetConnection(string connectionString, string connectionName)
		{
			_connectionString = connectionString;

			_providerConnection = MakeProviderConnection();
			if(connectionName != string.Empty)
				StoreConnectionString(connectionString, connectionName);
		}
		#endregion

		#region connection get/set/store
		/// <summary>
		/// Set and stores the connection string value with a name
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="connectionName"></param>
		public virtual void StoreConnectionString(string connectionString, string connectionName)
		{
			try
			{
				//save the local config file
				SettingsProperty cs = new SettingsProperty(connectionName, Type.GetType("System.String"), new System.Configuration.LocalFileSettingsProvider(), false, connectionString, SettingsSerializeAs.String, new SettingsAttributeDictionary(), false, false);
				PersistenceServiceConfiguration.Default.Properties.Add(cs);
				PersistenceServiceConfiguration.Default.Save();
				_connectionString = connectionString;
				_connectionName = connectionName;
			}
			catch {  }

		}
		/// <summary>
		/// Get the connection string value from a configuration file.
		/// </summary>
		/// <param name="connectionName"></param>
		public virtual string GetConnectionStringByName(string connectionName)
		{
			try
			{
				_connectionString = PersistenceServiceConfiguration.Default[connectionName].ToString();//Properties[connectionName].DefaultValue.ToString();
				_connectionName = connectionName;
				return _connectionString;
			}
			catch { return null; }
		}
		//Template method: define the skeleton of an algorithm, deferring some
		//steps to subclasses.
		//Factory method: defer implementation of the creation of objects
		//to subtypes.
		/// <summary>
		/// Insert an object into a persistent storage.
		/// </summary>
		/// <param name="appObject"></param>
		/// <param name="oid"></param>
		/// <returns></returns>
		#endregion

		#region Store (insert)
		//2010.03.13 ok crr
		/// <summary>
		/// Insert any object into a persistent storage.
		/// </summary>
		/// <param name="appObject"></param>
		/// <param name="mapType"></param>
		/// <returns></returns>
		public ObjectIdentifier Store(object appObject)
		{
			try
			{
				//Get a dematerializer object and the Map with the property-parameter conversion data
				if (appObject == null)
					return null;
				//set the _map
				GetMap(appObject.GetType());

				//Factory Methods: let the subclasses define the creation of concrete objects
				//make the specific command to execute an Insert
				_cmd = MakeStoreCommand(_map, appObject);//1 MakeStoreCommand

				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				//Command Pattern: A request is encapsulated in a class.
				//Subtypes will take into account the concurrency support
				return (ObjectIdentifier)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(Messages_en_US.CANNOT_STORE_NEW, ex);
			}
		}
		//ok crr 2009.10.31
		/// <summary>
		/// Insert any object into a persistent storage. Can use another map type than its.
		/// </summary>
		/// <param name="appObject"></param>
		/// <param name="mapType"></param>
		/// <returns></returns>
		public ObjectIdentifier Store(object appObject,Type mapType)
		{
			try
			{
				//Get a dematerializer object and the Map with the property-parameter conversion data
				if (appObject == null)
					return null;

				//set the _map variable
				GetMap(mapType);

				//make the specific command to execute an Insert
				//Factory Methods: let the subclasses define the creation of concrete objects
				_cmd = MakeStoreCommand(_map, appObject);//1 MakeStoreCommand

				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				//Command Pattern: A request is encapsulated in a class.
				//Subtypes will take into account the concurrency support
				return (ObjectIdentifier)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_STORE_NEW, ex);
			}
		}
		//ok crr 2009.10.31
		/// <summary>
		/// Store values from different application objects,
		/// using or not a particular Type for mapping.
		/// </summary>
		/// <param name="mapType"></param>
		/// <param name="forUpdate"></param>
		/// <param name="appObjects"></param>
		/// <returns></returns>
		public ObjectIdentifier Store(Type mapType, IDictionary<string, object> appObjects)
		{
			try
			{
				//set the _map
				GetMap(mapType);

				//make the specific command to execute an Insert
				_cmd = MakeStoreCommand(_map, appObjects);//2 MakeStoreCommand
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				//Command Pattern: A request is encapsulated in a class.
				//Subtypes will take into account the concurrency support
				return (ObjectIdentifier)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_STORE_NEW, ex);
			}

		}
		//ok crr 2009.10.31
		/// <summary>
		/// Store multiple properties using a custom command and particular mappings.
		/// </summary>
		/// <param name="cmdType"></param>
		/// <param name="mapType"></param>
		/// <param name="appObjects"></param>
		/// <returns></returns>
		public ObjectIdentifier Store(string cmdIdentifier, Type mapType, IDictionary<string, object> appObjects)
		{
			try
			{
				GetMap(mapType);

				//make the specific command to execute an Insert with custom command
				_cmd = MakeStoreCommand(_map, cmdIdentifier, appObjects);//4 makeStoreCommand
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);
				//Command Pattern: A request is encapsulated in a class.
				//Subtypes will take into account the concurrency support
				return (ObjectIdentifier)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_STORE_NEW, ex);
			}
		}
		#endregion

		#region Retrieve
		//ok crr 2009.10.31
		/// <summary>
		/// Retrieve all. Return a DataView. Receive the class type as parameter
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public DataView Retrieve(Type mapType)
		{
			try
			{
				GetMap(mapType);

				_cmd = MakeRetrieveCommand(_map, mapType);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				return _cmd.Run() as DataView;
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_RETRIEVE, ex);
			}
		}
		//ok crr 2009.10.31
		/// <summary>
		/// Return an persistent object or a list of records using a custom command identifier.
		/// </summary>
		/// <param name="mapType"></param>
		/// <param name="cmdType"></param>
		/// <param name="returnCollection"></param>
		/// <returns></returns>
		public object Retrieve(Type mapType, string cmdIdentifier, bool returnCollection)
		{
			try
			{
				GetMap(mapType);

				_cmd = MakeRetrieveCommand(_map, mapType, cmdIdentifier, returnCollection);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				if (returnCollection)
					return _cmd.Run() as DataView;

				return (IPersistable)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_RETRIEVE, ex);
			}

		}
		//ok crr 2009.10.31
		/// <summary>
		/// Retrieve an object from a persistent storage object giving its OID.
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public IPersistable Retrieve(ObjectIdentifier objectId, Type type)
		{
			try
			{
				GetMap(type);

				_cmd = MakeRetrieveCommand(_map, objectId, type);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				return (IPersistable)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_RETRIEVE, ex);
			}
		}
		//ok crr 2009.10.31
		/// <summary>
		///Retrieve method for custom retrieve operations giving an OID. 
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="type"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		public IPersistable Retrieve(ObjectIdentifier objectId, Type type, string cmdType)
		{
			try
			{
				GetMap(type);

				_cmd = MakeRetrieveCommand(_map, objectId, type, cmdType);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				return (IPersistable)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_RETRIEVE, ex);
			}
		}
		/// <summary>
		/// Retrieve object with custom criteria and command. Return collection support.
		/// </summary>
		/// <param name="criteria"></param>
		/// <param name="mapType"></param>
		/// <param name="cmdType"></param>
		/// <param name="returnCollection"></param>
		/// <returns></returns>
		public object Retrieve(object criteria, Type mapType, string cmdIdentifier, bool returnCollection)
		{
			try
			{
				GetMap(mapType);

				_cmd = MakeRetrieveCommand(_map, criteria, mapType, cmdIdentifier, returnCollection);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				if (returnCollection)
					return (DataView)_cmd.Run();
				return (IPersistable)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_RETRIEVE, ex);
			}
		}
		/// <summary>
		/// Retrieve object(s) with custom criterias. Collection return support.
		/// </summary>
		/// <param name="mapType"></param>
		/// <param name="cmdType"></param>
		/// <param name="returnCollection"></param>
		/// <param name="criterias"></param>
		/// <returns></returns>
		public object Retrieve(Type mapType, string cmdIdentifier, bool returnCollection, IList<object> criterias)
		{
			try
			{

				GetMap(mapType);

				_cmd = MakeRetrieveCommand(_map, mapType, cmdIdentifier, returnCollection, criterias);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				if (returnCollection)
					return (DataView)_cmd.Run();

				return (IPersistable)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_RETRIEVE, ex);
			}
		}
		#endregion

		#region Update
		/// <summary>
		/// Update a current persistent object using its oid. 
		/// </summary>
		/// <param name="oid"></param>
		/// <param name="appObject"></param>
		/// <returns></returns>
		public object Change(ObjectIdentifier oid, object appObject)
		{
			try
			{
				GetMap(appObject.GetType());

				//make the specific command to execute an Update
				//_cmd = MakeStoreCommand(pObject,_map);
				_cmd = MakeChangeCommand(_map, oid, appObject);
				if (_cmd == null || _cmd.CommandMap == null)
					throw new Exception(NO_CMD_MAP);

				//Command Pattern: A request is encapsulated in a class.
				//Subtypes will take into account the concurrency support
				return _cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_UPDATE, ex);
			}
		}
		/// <summary>
		/// Update a persistentObject using a particular Type for mapping.
		/// </summary>
		/// <param name="pObject"></param>
		/// <param name="mapType"></param>
		/// <returns></returns>
		public bool Change(ObjectIdentifier oid, object appObject, Type mapType)
		{
			try
			{
				GetMap(mapType);

				//make the specific command to execute an Update
				_cmd = MakeChangeCommand(_map, oid, appObject);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				//Command Pattern: A request is encapsulated in a class.
				//Subtypes will take into account the concurrency support
				return (bool)_cmd.Run();

			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_UPDATE, ex);
			}
		}
		/// <summary>
		/// Updates a record using multiple object properties.
		/// </summary>
		/// <param name="mapType"></param>
		/// <param name="appObjects"></param>
		/// <returns></returns>
		public bool Change(Type mapType, IDictionary<string, object> appObjects)
		{
			try
			{
				GetMap(mapType);

				//make the specific command to execute an Insert
				_cmd = MakeChangeCommand(_map, appObjects);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				//Command Pattern: A request is encapsulated in a class.
				//Subtypes will take into account the concurrency support
				return (bool)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_STORE_NEW, ex);
			}

		}
		/// <summary>
		/// Update a record with a custom command and a set of object properties.
		/// </summary>
		/// <param name="cmdTypeIdentifier"></param>
		/// <param name="mapType"></param>
		/// <param name="appObjects"></param>
		/// <returns></returns>
		public bool Change(string cmdTypeIdentifier, Type mapType, IDictionary<string, object> appObjects)
		{
			try
			{
				GetMap(mapType);

				//make the specific command to execute an Insert with custom command
				_cmd = MakeChangeCommand(_map, cmdTypeIdentifier, appObjects);//4 makeStoreCommand

				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				//Command Pattern: A request is encapsulated in a class.
				//Subtypes will take into account the concurrency support
				return (bool)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_STORE_NEW, ex);
			}
		}
		#endregion

		#region Destroy
		/// <summary>
		/// Destroy a persistent object using its identifier.
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool Destroy(ObjectIdentifier oid, Type type)
		{

			try
			{
				GetMap(type);

				_cmd = MakeDeleteCommand(_map, oid);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				return (bool)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_DESTROY, ex);
			}
		}
		/// <summary>
		/// Custom command for destroying a persisent object using its identifier.
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="type"></param>
		/// <param name="cmdType"></param>
		/// <returns></returns>
		public bool Destroy(ObjectIdentifier oid, Type type, string cmdType)
		{
			try
			{
				GetMap(type);
		
				_cmd = MakeDeleteCommand(_map, oid, cmdType);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				return (bool)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_DESTROY, ex);
			}
		}
		#endregion

		#region Multiple operations
		/// <summary>
		/// Execute a SQL explicit transaction procedure with multiple objects.
		/// </summary>
		/// <param name="objType"></param>
		/// <param name="cmdType"></param>
		/// <param name="appObjects"></param>
		/// <returns></returns>
		public bool ExecuteTransaction(Type mapType, string cmdIdentifier, IDictionary<string, object> appObjects)
		{
			try
			{
				GetMap(mapType);

				_cmd = MakeTransactionCommand(_map, cmdIdentifier, false, appObjects);
				if (_cmd == null)
					throw new Exception(NO_CMD_MAP);

				//Command Pattern: A request is encapsulated in a class.
				return (bool)_cmd.Run();
			}
			catch (Exception ex)
			{

				throw new Exception(CANNOT_TRANSACTION, ex);
			}
		}
		
		#endregion

		#region protected methods

		#region Make dematerializer and get map
		//abstract methods to be implemented by subtypes
		protected abstract Dematerializer MakeDematerializer();

		//REFACTORING: get commmon code and create a separate procedure
		//2012.05.19 crr: centralize the get map operation.
		protected void GetMap(Type objType)
		{
			//Get a dematerializer object and the Map with the property-parameter conversion data
			//Factory Methods: let the subclasses define the creation of concrete objects
			_map = MakeDematerializer().GetMap(objType);

			if(_map == null)
				throw new Exception(NO_MAP);
		}
		#endregion

		#region MakeStoreCommand

		/// <summary>
		/// Make an insert command for one single object.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="appObject"></param>
		/// <returns></returns>
		protected abstract PersistenceCommand MakeStoreCommand(Map map, object appObject); //in use
	
		/// <summary>
		/// Make an insert command for multiple object properties.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="appObjects"></param>
		/// <returns></returns>
		protected abstract PersistenceCommand MakeStoreCommand(Map map, IDictionary<string, object> appObjects); //in use

		/// <summary>
		/// Make an insert command for multiple object properties and a custom command identifier.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="cmdIdentifier"></param>
		/// <param name="appObjects"></param>
		/// <returns></returns>
		protected abstract PersistenceCommand MakeStoreCommand(Map map, string cmdTypeIdentifier, IDictionary<string, object> appObjects); //in use
		#endregion

		#region MakeChangeCommand
		//update multiple properties with multiple objects and default command type (create/update)
		protected abstract PersistenceCommand MakeChangeCommand(Map map, IDictionary<string, object> appObjects); //in use

		//update multiple properties with multiple objects and custom command type
		protected abstract PersistenceCommand MakeChangeCommand(Map map, string cmdTypeIdentifier, IDictionary<string, object> appObjects); //in use

		//update and object with default command type (update)
		protected abstract PersistenceCommand MakeChangeCommand(Map map, ObjectIdentifier oid, object appObject);

		#endregion

		#region MakeRetrieveCommand
		//retrieve all with default commant type
		protected abstract PersistenceCommand MakeRetrieveCommand(Map map, Type type);//in use

		//retrieve with the oid and default command type
		protected abstract PersistenceCommand MakeRetrieveCommand(Map map, ObjectIdentifier oid, Type type); //In use

		//retrieve one or multiple with custom command and no oid
		protected abstract PersistenceCommand MakeRetrieveCommand(Map map, Type type, string cmdType, bool returnCollection);//TEST MakeRetrieveCommand

		//retrieve with oid and custom command
		protected abstract PersistenceCommand MakeRetrieveCommand(Map map, ObjectIdentifier oid, Type type, string cmdType);//in use

		//retrieve one or multiple with custom command, a single criteria
		protected abstract PersistenceCommand MakeRetrieveCommand(Map map, object criteria, Type type, string cmdType, bool returnCollection);//in use

		//retrieve one or multiple with cusstom command and multiple criterias
		protected abstract PersistenceCommand MakeRetrieveCommand(Map map, Type type, string command, bool returnCollection, IList<object> criterias);//in use
		#endregion

		#region MakeDeleteCommand
		//delete command with OID or other criterias
		protected abstract PersistenceCommand MakeDeleteCommand(Map map, ObjectIdentifier oid);
		protected abstract PersistenceCommand MakeDeleteCommand(Map map, ObjectIdentifier oid, string cmdType);
		#endregion

		protected abstract CompositeCommand MakeCompositeCommand();

		//transaction command with custom command type and multiple application
		protected abstract PersistenceCommand MakeTransactionCommand(Map map, string cmdType, bool returnCollection, IDictionary<string,object> appObjs);

		//to configure the specific providerConnection object
		protected abstract object MakeProviderConnection();

		#endregion
	}
}
