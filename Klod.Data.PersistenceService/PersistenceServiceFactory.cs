using System;
using System.Reflection;
using Klod.Data.PersistenceService.Settings;

namespace Klod.Data.PersistenceService
{
    //TODO: Use cache to store and reuse PersistenceServer instances?
	/// <summary>
	/// Let's the interface/abstract project do the factory of implementations. The abstract set of classes should be in an isolated
	/// assembly. Implementations use another assembly to support late binding.
	/// </summary>
	public sealed class PersistenceServiceFactory
	{
		#region singleton implementation
		private static readonly PersistenceServiceFactory _psf = new PersistenceServiceFactory();

		static PersistenceServiceFactory() { }

		private PersistenceServiceFactory() { }

		public static PersistenceServiceFactory GetInstance()
		{
			return _psf;
		}
		#endregion

		#region private helper methods
		/// <summary>
		/// Load from an assembly location and get the persistenceServer type.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="persistenceServerClass"></param>
		/// <param name="connectionString"></param>
		/// <param name="connectionName"></param>
		/// <param name="concurrencySupport"></param>
		/// <returns></returns>
		private PersistenceServer LoadFrom(string assembly,string persistenceServerClass,string connectionString,string connectionName,string concurrencySupport)
		{
			PersistenceServer server;
			Assembly a=null;

			a = Assembly.LoadFrom(assembly);
			server = (PersistenceServer)a.CreateInstance(persistenceServerClass);
			server.SetConnection(connectionString, connectionName);
			server.ConcurrencySupport = (ConcurrencySupportType)Enum.Parse(typeof(ConcurrencySupportType), concurrencySupport);
			return server;

		}
		#endregion

		#region MakePersistenceServer
		/// <summary>
		/// Factory method to fully late-binding the concrete implementation of the persistenceServer class.
		/// </summary>
		/// <returns></returns>
		public PersistenceServer MakePersistenceServer()
		{
			return LoadFrom(PersistenceServiceConfiguration.Default.PersistenceServiceAssemblyLocation, PersistenceServiceConfiguration.Default.PersistenceServerClass, PersistenceServiceConfiguration.Default.Connection, PersistenceServiceConfiguration.Default.ConnectionName, PersistenceServiceConfiguration.Default.ConcurrencySupport);
		}
		/// <summary>
		/// Factory method to fully late-binding the concrete implementation of the persistenceServer class.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="connectionName"></param>
		/// <returns></returns>
		public PersistenceServer MakePersistenceServer(string connectionString, string connectionName)
		{
			return LoadFrom(PersistenceServiceConfiguration.Default.PersistenceServiceAssemblyLocation, PersistenceServiceConfiguration.Default.PersistenceServerClass, connectionString, connectionName, PersistenceServiceConfiguration.Default.ConcurrencySupport);
		}
		#endregion
	}
}
