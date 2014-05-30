using System;
using System.Collections.Generic;
using System.Text;

namespace Klod.Data.PersistenceService
{
	public abstract class PersistenceMapper : IMapper
	{
		private Dictionary<ObjectIdentifier, IPersistable> _cachedObjects;

		public Dictionary<ObjectIdentifier, IPersistable> CachedObjects
		{
			set { _cachedObjects = value; }
			get { return _cachedObjects; }
		}
		
		public PersistenceMapper()
		{
			_cachedObjects = new Dictionary<ObjectIdentifier, IPersistable>();
		}

		/// <summary>
		/// Store a new object in a persistent storage.
		/// </summary>
		/// <param name="oid"></param>
		/// <param name="appObject"></param>
		/// <returns></returns>
		public bool Put(ObjectIdentifier oid, Object appObject)
		{
			bool success = PutObjectInStorage(oid, appObject);
			IPersistable pObject = MakePersistable(appObject, oid);
			_cachedObjects.Add(oid, pObject);

			return success;
		}

		/// <summary>
		/// Store a new object in a persistent storage.
		/// </summary>
		/// <param name="appObject"></param>
		/// <returns></returns>
		public ObjectIdentifier Put(Object appObject)
		{
			ObjectIdentifier returnOID = PutObjectInStorage(appObject);
			//create an IPersistable instance
			IPersistable pObject = MakePersistable(appObject, returnOID);
			_cachedObjects.Add(returnOID, pObject);
			return returnOID;
		}
		/// <summary>
		/// Retrieve a persistent object from a storage.
		/// </summary>
		/// <param name="oid"></param>
		/// <returns></returns>
		public IPersistable Get(ObjectIdentifier oid)
		{
			IPersistable obj = _cachedObjects[oid];
			if (obj == null)
			{
				obj = GetObjectFromStorage(oid);
				_cachedObjects.Add(oid, obj);
			}
			return obj;
		}
		/// <summary>
		/// Change a persistent object in a storage.
		/// </summary>
		/// <param name="pObject"></param>
		/// <returns></returns>
		public bool Change(IPersistable pObject)
		{
			bool success = ChangeObjectInStorage(pObject);

			if (_cachedObjects.ContainsKey(pObject.OID))
				_cachedObjects[pObject.OID] = pObject;
			else
				_cachedObjects.Add(pObject.OID, pObject);

			return success;
		}
		/// <summary>
		/// Destroy a persistent object in a storage.
		/// </summary>
		/// <param name="oid"></param>
		/// <returns></returns>
		public bool Destroy(ObjectIdentifier oid)
		{
			bool success = DestroyObjectInStorage(oid);
			_cachedObjects.Remove(oid);
			return success;
		}

		//create
		protected abstract bool PutObjectInStorage(ObjectIdentifier oid, Object appObject);
		protected abstract ObjectIdentifier PutObjectInStorage(Object appObject);
		//retrieve
		protected abstract IPersistable GetObjectFromStorage(ObjectIdentifier oid);
		//update
		protected abstract bool ChangeObjectInStorage(IPersistable pObject);
		//delete
		protected abstract bool DestroyObjectInStorage(ObjectIdentifier oid);

		protected abstract IPersistable MakePersistable();
		protected abstract IPersistable MakePersistable(object appObject, ObjectIdentifier oid);

	}
}
