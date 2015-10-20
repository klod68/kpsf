namespace Klod.Data.PersistenceService
{
    public interface IObjectCache
	{
		void Add(ObjectIdentifier oid, object appObject);
		object Get(ObjectIdentifier oid);
		void Remove(ObjectIdentifier oid);
	}
}
