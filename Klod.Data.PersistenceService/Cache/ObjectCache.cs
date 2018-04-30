using System;
using System.Collections.Generic;

namespace Klod.Data.PersistenceService.Cache
{
    /// <summary>
    /// An object repository.
    /// </summary>
    internal class ObjectCache : IObjectCache
    {
        private Dictionary<ObjectIdentifier, object> _cache;

        public Dictionary<ObjectIdentifier, object> Cache
        {
            set { _cache = value; }
            get { return _cache; }
        }

        public ObjectCache()
        {
            _cache = new Dictionary<ObjectIdentifier, object>();
        }

        #region IObjectCache Members

        public void Add(ObjectIdentifier oid, object appObject)
        {
            _cache.Add(oid, appObject);
        }

        public Object Get(ObjectIdentifier oid)
        {
            if (_cache.ContainsKey(oid))
                return _cache[oid];
            return null;
        }

        public void Remove(ObjectIdentifier oid)
        {
            _cache.Remove(oid);
        }

        public int Count()
        {
            return _cache.Count;
        }

        public bool Contains(ObjectIdentifier oid)
        {
            return _cache.ContainsKey(oid);
        }

        public bool ContainsObject(Object appObject)
        {
            return _cache.ContainsValue(appObject);
        }

        public void Clear()
        {
            _cache.Clear();
        }

        #endregion IObjectCache Members
    }
}