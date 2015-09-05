using System;
using System.Collections.Generic;
using System.Text;
using DALBuilder.DomainLayer;
using System.Diagnostics;
using System.Data;
using System.Reflection;

namespace DALBuilder.ApplicationLayer
{
	 
	//TODO: Late-bind the Factory object to allow runtime changes of the implementation.
	/// <summary>
	 /// Singleton Controller class for application logic.
	 /// Receives all UI messages and maps them to the Domain Layer
	 /// </summary>
	 public class MakeScriptFilesController
	 {
		//PRINCIPLE:Declare all class variables as abstract/interface and use late-binding (code injection) for instantiation.
		private Connection _cn = null;
		private Database _db = null;
		private IDirectoryPath _dp = null;
		private DALFactory _df = null;


		public MakeScriptFilesController() 
		{
			string[] args = app.Default.DALFactoryClass.Split(',');
			//Avoid a design-time reference to the concrete SQL Server das constructor component. Use code-injection or plugin pattern.
			_df = MakeDALFactory(app.Default.DALFactoryClassLocation, args[0]);
		}
	  
		 /// <summary>
		 /// Create dynamically the DASFactory
		 /// </summary>
		 /// <param name="assembly"></param>
		 /// <param name="objectType"></param>
		 /// <returns></returns>
		 private DALFactory MakeDALFactory(string assembly,string objectType)
		 {
			try
			{
				if (_df!=null)
					return _df;

				object obj;
				//Assembly a = Assembly.Load(assembly);
				//Assembly acts as an assembler for code injection.
				Assembly a = Assembly.LoadFrom(assembly);
				Type t = a.GetType(objectType, true, true);
				if (t == null)
					throw new Exception("Cannot find the type in this assembly.");

				obj = (object)Activator.CreateInstance(t);
				return (DALFactory)obj;
			}
			catch (Exception ex)
			{
				throw new Exception("Cannot create an object.", ex);
			}
		 }

		  /// <summary>
		  /// Get the list of Database Servers in the Network
		  /// </summary>
		  /// <returns></returns>
		  public DataTable GetServersInNetwork()
		  {
			   try
			   {
				   Network nw=_df.MakeNetwork();
					return nw.GetDatabaseServers();
			   }
			   catch { return null; }
		  }

		  /// <summary>
		  /// Get the database in the selected SQL Server
		  /// </summary>
		  /// <param name="serverName"></param>
		  /// <returns></returns>
		  public DataTable GetDatabases(string serverName)
		  {
			   try
			   {
				   Server server = _df.MakeServer();
				   return server.GetDatabases(serverName);
			   }
			   catch (Exception ex)
			   {
					Debug.Write(ex.Message);
					return null; }
		  }


		  /// <summary>
		  /// 1. Make set the proper connection. Pass all parameters from UI.
		  /// REMEMBER: The User doesn't have to know how to format a proper connection string.
		  /// </summary>
		  /// <param name="settings"></param>
		  public void SetConnection(string server,string database,bool isIntegratedSecurity,string login,string password)
		  {
			   //create a connection object
			   _cn = _df.MakeConnection();

			   if (_cn == null)
					throw new Exception("Cannot connect to your data source.");

			   string _settings = _cn.ParseSettings(server, database, isIntegratedSecurity.ToString(), login, password);
			   try
			   {
					//set connection settings and connect
					if (!_cn.Connect(_settings))
						 throw new Exception("Cannot connect to your data source.");
			   }
			   catch { }
		  }
		  /// <summary>
		  /// 2. Then get all tables names from the database.
		  /// The UI use this info to show them to the user.
		  /// </summary>
		  /// <returns></returns>
		  public string[] GetTableNames()
		  {
			   try
			   {
					if (_cn == null) //if no connection can't call MakeDB
						 return null;
					//Create a db object with the connection setted
					//The database object is tha main (root) object of the domain layer
					_db = _df.MakeDB(_cn);
					if (_db == null)
						 return null; //if no db then no tables

					return _db.GetTableNames();
			   }
			   catch (Exception ex)
			{
				Debug.Write(ex.Message);
				return null; 
			}
		  }

		  /// <summary>
		  /// 3. Submit the selected tables for scripting.
		  /// </summary>
		  /// <param name="tables"></param>
		  public void SubmitTablesSelected(string[] tables)
		  {
			   if (_db == null)
					throw new Exception("No Database object settled");
			   _db.SubmitTablesSelected(tables);
		  }

		  /// <summary>
		  /// 4. If concurrent support then add code for new columns.
		  /// </summary>
		  public void AddConcurrencySupport(bool optimistic, bool pessimisticUserName,bool pessimisticUniqueIdentifier)
		  {
			   ConcurrencySupportEnum concurrency = ConcurrencySupportEnum.None;

			   if (optimistic)
					concurrency = ConcurrencySupportEnum.Optimistic;
			   else if (pessimisticUniqueIdentifier)
					concurrency = ConcurrencySupportEnum.PessimisticUserId;
			   else if (pessimisticUserName)
					concurrency = ConcurrencySupportEnum.PessimisticUserName;

			   if (_db == null)
					throw new Exception("No Database object settled");
			   _db.ConcurrencySupport = concurrency;
		  }

		  
		  /// <summary>
		  /// 5. Generate the stored procedures and 
		  /// save the scripts and mappings files in the selected paths location.
		  /// </summary>
		  /// <param name="path"></param>
		  public void SaveScripts(string dasPath)
		  {
			   try
			   {
					_dp = _df.MakeDirectoryPath();

					if(_dp == null)
						 throw new Exception("No Directory Path settled.");

					//generate concurrency code
					ConcurrencySupportCodeString[] _concurrencyCode = _db.GenerateConcurrencySupportCode();

					//Generate SPROCS
					StoreProcedureStream[] _sprocs = _db.GenerateSProcs();

					if (_concurrencyCode != null)
					{     //save support code for concurrency
						 _dp.SaveConcurrencySupportCode(_concurrencyCode,dasPath,"ConcurrencySupport.sql");
					}
					if (_sprocs != null)
					{                         //save sprocs in the saved path
						 _dp.SaveSProcsToFile(_sprocs,dasPath,"DALScript.sql");

						 //save mappings file. Who is responsible? DirectoryPath
					_dp.SaveMappingsToFile(_sprocs, dasPath, "mappings.xml");
					}
			   }
			   catch (Exception ex) {Debug.Write(ex.Message); }
		  }
	 }
}
