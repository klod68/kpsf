using System;

namespace Klod.Data.PersistenceService
{
    public interface IMapper
	{
		IPersistable Get(ObjectIdentifier oid);//retrieve an object
		ObjectIdentifier Put(Object appObject);//store new objects
		bool Put(ObjectIdentifier oid, Object appObject);//store new objects
		bool Change(IPersistable pObject);//store persistent object
		bool Destroy(ObjectIdentifier oid);//destroy a persistent object
	}
}
