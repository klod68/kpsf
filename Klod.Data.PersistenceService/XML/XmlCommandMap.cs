using System;
using System.Xml;

using Klod.Data.PersistenceService.Settings;

namespace Klod.Data.PersistenceService.XML
{
	/// <summary>
	/// Representation of a command element in a map.
	/// </summary>
	internal class XmlCommandMap : CommandMap
	{
		#region variables
		protected XmlNode _cmdXML;
		protected XmlNode _paramXML;

		//protected string[] ClassNamesArray;//to store objects info, when available for transaction commands
		const string ERR_INSTANCE_CREATION = "Cannot create an instance of XmlCommandMap.";

		#endregion

		#region constructor
		protected XmlCommandMap() { }
		public XmlCommandMap(XmlNode commandXML, string objName)
		{
			try
			{
				_cmdXML = commandXML;
				Name = _cmdXML.Attributes[PersistenceServiceConfiguration.Default.CommandNameAttribute].Value;
				Type = _cmdXML.Attributes[PersistenceServiceConfiguration.Default.CommandTypeAttribute].Value;

				//set the parameters 
				_paramXML = _cmdXML.SelectSingleNode(PersistenceServiceConfiguration.Default.CommandParametersElement);
				if (_paramXML != null)
				{
					ParametersCount = _paramXML.ChildNodes.Count; //how many parameters we have?
					//array for parameters
					ParametersSet = new string[ParametersCount];
					//array for properties to map
					PropertiesSet = new string[ParametersCount];

					//array for objects where the property values are stored
					ClassNamesSet = new string[ParametersCount];

					//REFACTORING: 2012.05.15 crr
					ClassTokensSet = new string[ParametersCount];

					ParametersDataTypesSet = new string[ParametersCount];
					ParametersDirectionsSet = new string[ParametersCount];
					ParametersUsesSet = new string[ParametersCount];


					for (byte i = 0; i < this.ParametersCount; i++)
					{
						ParametersSet[i] = _paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterNameAttribute].Value;
						PropertiesSet[i] = _paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterPropertyAttribute].Value;
						if(_paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterClassAttribute] == null)
							ClassNamesSet[i] = objName;
						else
							ClassNamesSet[i] = _paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterClassAttribute].Value;

						//REFACTORING: get the token if any
						//2012.05.18
						if(_paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterTokenAttribute] == null)
							ClassTokensSet[i] = ClassNamesSet[i];
						else
							ClassTokensSet[i] = _paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterTokenAttribute].Value;

						if (_paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterTypeAttribute] != null)
							ParametersDataTypesSet[i] = _paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterTypeAttribute].Value;
						else
							ParametersDataTypesSet[i] = string.Empty;


						if (_paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterDirectionAttribute] != null)
							ParametersDirectionsSet[i] = _paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterDirectionAttribute].Value;
						else

							ParametersDirectionsSet[i] = string.Empty;

						if (_paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterUseAttribute] != null)
							ParametersUsesSet[i] = _paramXML.ChildNodes[i].Attributes[PersistenceServiceConfiguration.Default.ParameterUseAttribute].Value;
						else
							ParametersUsesSet[i] = string.Empty;

					}
				}
			}
			catch 
			{
				throw new Exception(ERR_INSTANCE_CREATION);
			}
		}
		#endregion


		#region default methods to get property values

		public override string GetIDDataType()
		{
			try
			{
				return _cmdXML.Attributes[PersistenceServiceConfiguration.Default.CommandIdDataTypeAttribute].Value;
			}
			catch
			{
				return string.Empty;
			}
		}

		public override string GetProviderCommandType()
		{
			try
			{
				return _cmdXML.Attributes[PersistenceServiceConfiguration.Default.CommandProviderTypeAttribute].Value;
			}
			catch { return string.Empty; }

		}
		public override string GetReturnTableName()
		{
			try
			{
				return _cmdXML.SelectSingleNode(PersistenceServiceConfiguration.Default.CommandReturnTableElement).Attributes[PersistenceServiceConfiguration.Default.ReturnTableNameAttribute].Value;
			}
			catch { return string.Empty; }
		}
		public override string GetRetrieveColumn(int index)
		{
			try
			{
				return _cmdXML.SelectSingleNode(PersistenceServiceConfiguration.Default.CommandReturnTableElement).ChildNodes[index].Attributes[PersistenceServiceConfiguration.Default.ReturnTableColumnNameAttribute].Value;
			}
			catch { return string.Empty; }
		}
		public override int GetRetrievedColumnsCount()
		{
			try
			{
				return _cmdXML.SelectSingleNode(PersistenceServiceConfiguration.Default.CommandReturnTableElement).ChildNodes.Count;
			}
			catch { return 0; }
		}
		public override string GetRetrieveProperty(int index)
		{
			try
			{
				return _cmdXML.SelectSingleNode(PersistenceServiceConfiguration.Default.CommandReturnTableElement).ChildNodes[index].Attributes[PersistenceServiceConfiguration.Default.ReturnTableColumnPropertyAttribute].Value;
			}
			catch { return string.Empty; }
		}
		public override bool IsRetrievedColumObjectIdentifier(int index)
		{
			try
			{
				return bool.Parse(_cmdXML.SelectSingleNode(PersistenceServiceConfiguration.Default.CommandReturnTableElement).ChildNodes[index].Attributes[PersistenceServiceConfiguration.Default.ReturnTableColumnPrimaryKeyAttribute].Value);
			}
			catch { return false; }
		}
		public override string GetClassName(int index)
		{
			try
			{
				return ClassNamesSet[index];
			}
			catch
			{
				return string.Empty;
			}
		}
		//REFACTORING: get the token name for same classes
		//2012.05.18 crr
		public override string  GetClassToken(int index)
		{
			try
			{
				return ClassTokensSet[index];
			}
			catch { return string.Empty; }
		}
		#endregion
	}
}
