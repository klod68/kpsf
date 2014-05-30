using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;
using Klod.Data.PersistenceService.Settings;

namespace Klod.Data.PersistenceService.XML
{
	/// <summary>
	/// A set of instructions to map object properties to parameters or elements
	/// of the persistent object.
	/// </summary>
	public class XmlMap : Map
	{
		protected XmlNode _text; //content
		protected string _className;
		protected string _type;
		protected string _storage;
		protected string _storageType;

		#region constructor
		public XmlMap(XmlNode text)
		{
			_text = text;
			_className = _text.Attributes[PersistenceServiceConfiguration.Default.MapClassAttribute].Value;
			_type = _text.Attributes[PersistenceServiceConfiguration.Default.MapTypeAttribute].Value;
			_storage = _text.Attributes[PersistenceServiceConfiguration.Default.MapStorageAttribute].Value;
			_storageType = _text.Attributes[PersistenceServiceConfiguration.Default.MapStorageTypeAttribute].Value;
		}
		#endregion

		#region properties
		public override string ClassName
		{
			get { return _className; }
		}
		public override string Type
		{
			get { return _type; }
		}
		public override string Storage
		{
			get { return _storage; }
		}
		public override string StorageType
		{
			get { return _storageType; }
		}
		#endregion

		#region methods

		public override CommandMap GetCommandMap(string cmdType)
		{
			try
			{
				return new XmlCommandMap(_text.SelectSingleNode(PersistenceServiceConfiguration.Default.CommandsElement + "/" + PersistenceServiceConfiguration.Default.CommandElement + "[@" + PersistenceServiceConfiguration.Default.CommandTypeAttribute + "=\"" + cmdType + "\"]"), _className);
			}
			catch 
			{
				return null;
			}
		}
		#endregion

	}
}
