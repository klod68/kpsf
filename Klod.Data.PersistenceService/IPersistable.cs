namespace Klod.Data.PersistenceService
{
    /// <summary>
    /// Interface to support persistable objects requiring an OID property.
    /// </summary>
    public interface IPersistable
	{
		ObjectIdentifier OID { set;get;}
		object Version { set;get;}
		object InUse { set;get;}
		object StoredObject { set;get;}
		ObjectIdentifier[] References { set;get;}
		bool AddReference(ObjectIdentifier reference);
		bool RemoveReference(ObjectIdentifier reference);
		ObjectIdentifier GetReference(string fieldName);
		bool HasReference(string fieldName);

	}
}
