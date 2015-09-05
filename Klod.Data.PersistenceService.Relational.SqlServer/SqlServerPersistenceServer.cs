using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Klod.Data.PersistenceService.Persistent;

namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	/*
	*TODO: need deeply refactoring in the concurrency conditional code.
	*Delegate all Factory methods (MakeX) to state objects based on concurrency settings. The concrete Factory procedure
	*will be executed by the concrete instance of the state class
	*/
	//to store a local command object.
	/// <summary>
	/// PersistenceServer implementation for SQL Server.
	/// </summary>
	public class SqlServerPersistenceServer : PersistenceServer
	{

		public SqlServerPersistenceServer() : base() { }

		public SqlServerPersistenceServer(SqlConnection connection) : base(connection) { }

		public SqlServerPersistenceServer(SqlConnection connection, ConcurrencySupportType concurrency) : base(connection, concurrency) { }

		public SqlServerPersistenceServer(string connectionString, string connectionName)
			: base(connectionString, connectionName) { }

		public SqlServerPersistenceServer(string connectionName)
			: base(connectionName) { }

		public SqlServerPersistenceServer(string connectionString, string connectionName, ConcurrencySupportType concurrency)
			: base(connectionString, connectionName, concurrency) { }

		public SqlServerPersistenceServer(string connectionName, ConcurrencySupportType concurrency)
			: base(connectionName, concurrency) { }



		protected override Dematerializer MakeDematerializer()
		{
			return SqlServerDematerializer.GetInstance(); //_dematerializer;
		}

		#region MakeStoreCommand
		/// <summary>
		/// Factory method to return a store command for INSERTIONS of new data. Default type to stored procedure.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="map"></param>
		/// <param name="appObj"></param>
		/// <returns></returns>
		protected override PersistenceCommand MakeStoreCommand(Map map, object appObj)
		{
			SqlCommand cmd = ((SqlConnection) ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
			return new SqlServerCreateCommand(map, appObj, cmd);
		}

		/// <summary>
		/// Factory method for insert commands with multiple objects.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="forUpdate"></param>
		/// <param name="appObjects"></param>
		/// <returns></returns>
		protected override PersistenceCommand MakeStoreCommand(Map map, IDictionary<string,object> appObjects)
		{
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            return new SqlServerTransactionCommand(map, cmd, CRUD.Create, appObjects);
		}

		/// <summary>
		/// Factory method for insert commands, custom command types and multiple properties in multiple objects.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="cmdType"></param>
		/// <param name="appObjects"></param>
		/// <returns></returns>
		protected override PersistenceCommand MakeStoreCommand(Map map, string cmdTypeIdentifier, IDictionary<string,object> appObjects)
		{
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            return new SqlServerTransactionCommand(map, cmd, cmdTypeIdentifier, ConcurrencySupportType.None, false, appObjects);
		}
		
		#endregion

		#region MakeChangeCommand
		protected override PersistenceCommand MakeChangeCommand(Map map, IDictionary<string,object> appObjects)
		{
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            return new SqlServerTransactionCommand(map, cmd, CRUD.Update, appObjects);
		}
		protected override PersistenceCommand MakeChangeCommand(Map map, string cmdTypeIdentifier, IDictionary<string,object> appObjects)
		{
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            return new SqlServerTransactionCommand(map, cmd, cmdTypeIdentifier, ConcurrencySupportType.None, false, appObjects);
		}
		/// <summary>
		/// Factory method for update command and default command types.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="oid"></param>
		/// <param name="appObject"></param>
		/// <returns></returns>
		protected override PersistenceCommand MakeChangeCommand(Map map, ObjectIdentifier oid, object appObject)
		{
			SqlServerPersistenceCommand _pcmd = null;

			PersistentObject persistentObject = new PersistentObject(appObject, oid);

			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //REVIEW: An opportunity to implement the state and factory method pattern delegating the creation 
            //of the command to state classes that encapsulate the factory of the command based on the concurrency support state
            if (ConcurrencySupport == ConcurrencySupportType.None)
				_pcmd = new SqlServerUpdateCommand(map, persistentObject, cmd);
			else if(ConcurrencySupport == ConcurrencySupportType.Optimistic)
				_pcmd = new SqlServerUpdateCommand(map, persistentObject, cmd, CRUD.UpdateCheckTimestamp);
			else if(ConcurrencySupport == ConcurrencySupportType.PessimisticUniqueIdentifier || ConcurrencySupport == ConcurrencySupportType.PessimisticUserName)
				_pcmd = new SqlServerUpdateCommand(map, persistentObject, cmd, CRUD.UpdateCheckLock);

			return _pcmd;
		}

		#endregion

		#region MakeRetrieveCommand
		/// <summary>
		/// Retrieve all command, based on type.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		protected override PersistenceCommand MakeRetrieveCommand(Map map, Type type)
		{
			SqlServerPersistenceCommand pcmd = null;
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            pcmd = new SqlServerRetrieveCommand(map, type, cmd);
			return pcmd;
		}
		/// <summary>
		/// Retrieve one or records collection with a custom command type.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="type"></param>
		/// <param name="cmdType"></param>
		/// <param name="returnCollection"></param>
		/// <returns></returns>
		protected override PersistenceCommand MakeRetrieveCommand(Map map, Type type, string cmdType, bool returnCollection)
		{
			SqlServerPersistenceCommand pcmd = null;
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            pcmd = new SqlServerRetrieveCommand(map, cmdType, cmd, type, ConcurrencySupport, returnCollection);
			return pcmd;

		}

		/// <summary>
		/// Factory method to return a retrieve command with concurrency support.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="map"></param>
		/// <param name="oid"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		protected override PersistenceCommand MakeRetrieveCommand(Map map, ObjectIdentifier oid, Type type)
		{
			SqlServerPersistenceCommand pcmd = null;
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (ConcurrencySupport == ConcurrencySupportType.None)
				pcmd = new SqlServerRetrieveCommand(map, oid, cmd, type);
			else if (ConcurrencySupport == ConcurrencySupportType.Optimistic)
				pcmd = new SqlServerRetrieveCommand(map, oid, cmd, type, CRUD.RetrieveWithTimestamp);
			else if (ConcurrencySupport == ConcurrencySupportType.PessimisticUniqueIdentifier || ConcurrencySupport == ConcurrencySupportType.PessimisticUserName)
				pcmd = new SqlServerRetrieveCommand(map, oid, cmd, type, CRUD.RetrieveAndLock);
			return pcmd;
		}
		/// <summary>
		/// Factory method to support custom operations
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="map"></param>
		/// <param name="oid"></param>
		/// <param name="type"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		protected override PersistenceCommand MakeRetrieveCommand(Map map, ObjectIdentifier oid, Type type, string cmdType)
		{
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            return new SqlServerRetrieveCommand(map, oid, cmd, type, cmdType, ConcurrencySupport);
		}
		/// <summary>
		/// Factory method to support custom criteria and command.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="map"></param>
		/// <param name="criteria"></param>
		/// <param name="type"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		protected override PersistenceCommand MakeRetrieveCommand(Map map, object criteria, Type type, string cmdType, bool returnCollection)
		{
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            return new SqlServerRetrieveCommand(map, cmdType, cmd, type, ConcurrencySupport, criteria, returnCollection);
		}

		/// <summary>
		/// Factory method to support custom command and a set of criterias.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="type"></param>
		/// <param name="cmdType"></param>
		/// <param name="returnPersistent"></param>
		/// <param name="criterias"></param>
		/// <returns></returns>
		protected override PersistenceCommand MakeRetrieveCommand(Map map, Type type, string cmdType, bool returnPersistent, IList<object> criterias)
		{
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            return new SqlServerRetrieveCommand(map, cmdType, cmd, type, ConcurrencySupport, returnPersistent, criterias);
		}

		#endregion

		//TODO:add methods for delete by a custom criteria
		#region MakeDeleteCommand

		protected override PersistenceCommand MakeDeleteCommand(Map map, ObjectIdentifier oid)
		{
			SqlServerPersistenceCommand _pcmd = null;
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            //Verify concurrency and pass proper crud operation
            if (ConcurrencySupport == ConcurrencySupportType.None)
				_pcmd = new SqlServerDeleteCommand(map, oid, cmd);
			else if (ConcurrencySupport == ConcurrencySupportType.Optimistic)
				_pcmd = new SqlServerDeleteCommand(map, oid, cmd, CRUD.DeleteCheckTimestamp);
			else if (ConcurrencySupport == ConcurrencySupportType.PessimisticUniqueIdentifier || ConcurrencySupport == ConcurrencySupportType.PessimisticUserName)
				_pcmd = new SqlServerDeleteCommand(map, oid, cmd, CRUD.DeleteCheckLock);
			return _pcmd;
		}

		protected override PersistenceCommand MakeDeleteCommand(Map map, ObjectIdentifier oid, string cmdType)
		{
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            return new SqlServerDeleteCommand(map, oid, cmd, cmdType, ConcurrencySupport);
		}

		#endregion

		#region MakeTransactionCommands
		protected override CompositeCommand MakeCompositeCommand()
		{
			return new SqlServerCompositeCommand();
		}

		protected override PersistenceCommand MakeTransactionCommand(Map map, string cmdType, bool returnCollection, IDictionary<string, object> appObjs)
		{
			SqlCommand cmd = ((SqlConnection)ProviderConnection).CreateCommand();
			return new SqlServerTransactionCommand(map, cmd, cmdType, ConcurrencySupport, returnCollection, appObjs);
		}
		#endregion

		protected override object MakeProviderConnection()
		{
			if (ConnectionString == string.Empty)
				return null;

			if (ProviderConnection != null)
				return ProviderConnection;

			return new SqlConnection(ConnectionString);
		}
	}
}
