using System;
using System.Collections.Generic;
using System.Xml;
using Klod.Data.PersistenceService.Settings;
namespace Klod.Data.PersistenceService
{
    //CHECK: Assumes maps in xml. Change implementation to other map sources types. crr 20150917
    /// <summary>
    /// Class with the interface for a dematerialization process.
    /// Specific dematerialization must be implemented by subtypes.
    /// Behavior: Loads the maps of every objects in the mapping.xml map file
    /// </summary>
    public abstract class Dematerializer
	{
		private Dictionary<string, Map> _mapPool; //store the maps in a dictionary for caching
		protected const string ERR_LOADING_XML = "Error loading the mapping file.";

		public Dictionary<string, Map> MapPool
		{
			set { _mapPool = value; }
			get { return _mapPool; }
		}
		/// <summary>
		/// Constructor of dematerializer:
		/// Set the pool and load the maps
		/// </summary>
		public Dematerializer()
		{
			//make the pool for cache all objects instructions
			_mapPool = new Dictionary<string, Map>();
			//load and cache all maps for materialization/dematerialization instructions
			LoadMaps();
		}
		/// <summary>
		/// Extract map information of an object and constructs
		/// the PersistentCommand for specific storage.
		/// </summary>
		/// <param name="appObject"></param>
		/// <returns></returns>
		public Map GetMap(Type objType)
		{
			try
			{
				if (_mapPool.ContainsKey(objType.FullName))
					return _mapPool[objType.FullName];
				return null;
			}
			catch (Exception ex)
			{
				throw new Exception("Error getting the map for the object.", ex);
			}
		}
		public Map GetMap(string objType)
		{
			try
			{
				if (_mapPool.ContainsKey(objType))
					return _mapPool[objType];
				return null;
			}
			catch (Exception ex)
			{
				throw new Exception("Error getting the map for the object.", ex);
			}
		}
		/// <summary>
		/// Method for reading an xml file with mapping data.
		/// </summary>
		protected void LoadMaps()
		{
			try
			{
				XmlDocument _mappingsFile = new XmlDocument();
				_mappingsFile.Load(GetMappingsFileAddress());
				XmlNodeList _mapNodes = _mappingsFile.SelectNodes("//" + PersistenceServiceConfiguration.Default.MapElement);
				//Caching: put the maps of every object in a pool
				foreach (XmlNode _mapNode in _mapNodes)
				{
					string _mapKey = _mapNode.Attributes[PersistenceServiceConfiguration.Default.MapClassAttribute].Value;
					Map _map = MakeMap(_mapNode);
					_mapPool.Add(_mapKey, _map);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ERR_LOADING_XML + "\n" + ex.Message);
			}
		}

		protected abstract Map MakeMap(XmlNode mapNode);

		//protect the real address
		protected virtual string GetMappingsFileAddress()
		{
			return PersistenceServiceConfiguration.Default.MappingsFileAddress;
		}
	}
}
