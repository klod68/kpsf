using System;

namespace Klod.Data.PersistenceService.Identifiers
{
    /// <summary>
    /// DataType class for object identification. Object identifier pattern for a relational world.
    /// </summary>
    public class OID : ObjectIdentifier
	{
		#region variables

		const string ERR_NO_ID = "No identifier has been set.";   //TODO: set this arguments to resource files?
		#endregion

		#region constructors
		public OID() { }
		public OID(Int32 numericID)
			: base(numericID)
		{
			DataType = IDDataType.Number;
			Type = IDType.PersistentObject;
		}
		public OID(Int32 numericID, string fieldName)
			: base(numericID, fieldName)
		{
			DataType = IDDataType.Number;
			Type = IDType.PersistentObject;
		}
		public OID(Int32 numericID, string fieldName, IDType type)
			: base(numericID, fieldName, IDDataType.Number, type)
		{ }
		public OID(Int64 numericID):base(numericID)
		{
			DataType = IDDataType.Number;
			Type = IDType.PersistentObject;
		}
		public OID(Int64 numericID, string fieldName)
			: base(numericID, fieldName)
		{
			DataType = IDDataType.Number;
			Type = IDType.PersistentObject;
		}
		public OID(Int64 numericID, string fieldName, IDType type)
			: base(numericID, fieldName, IDDataType.Number, type)
		{ }

		public OID(decimal numericID)
			: base(numericID)
		{
			DataType = IDDataType.Number;
			Type = IDType.PersistentObject;
		}
		public OID(decimal numericID, string fieldName)
			: base(numericID, fieldName)
		{
			DataType = IDDataType.Number;
			Type = IDType.PersistentObject;
		}
		public OID(decimal numericID, string fieldName, IDType type)
			: base(numericID, fieldName, IDDataType.Number, type)
		{ }
		public OID(float numericID)
			: base(numericID)
		{
			DataType = IDDataType.Number;
			Type = IDType.PersistentObject;
		}
		public OID(float numericID, string fieldName)
			: base(numericID, fieldName)
		{
			DataType = IDDataType.Number;
			Type = IDType.PersistentObject;
		}
		public OID(float numericID, string fieldName, IDType type)
			: base(numericID, fieldName, IDDataType.Number, type)
		{ }
	
		
		public OID(string stringID)
			: base(stringID)
		{
			DataType = IDDataType.String;
			Type = IDType.PersistentObject;
		}
		public OID(string stringID, string fieldName)
			: base(stringID, fieldName)
		{
			DataType = IDDataType.String;
			Type = IDType.PersistentObject;
		}
		public OID(string stringID, string fieldName, IDType type)
			: base(stringID, fieldName, IDDataType.String, type)
		{ }

		public OID(Guid guid)
			: base(guid)
		{
			DataType = IDDataType.Guid;
			Type = IDType.Object;
		}
		public OID(Guid guid, string fieldName)
			: base(guid, fieldName)
		{
			DataType = IDDataType.Guid;
			Type = IDType.Object;
		}
		public OID(Guid guid, string fieldName, IDType type)
			: base(guid, fieldName, IDDataType.Guid, type) { }

		public OID(object id, string fieldName, IDDataType dataType, IDType type)
			: base(id, fieldName, dataType, type) { }

		#endregion
	}
}
