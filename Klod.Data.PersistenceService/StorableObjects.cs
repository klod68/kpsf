using System;
using System.Collections.Generic;

namespace Klod.Data.PersistenceService
{
    public class StorableObjects : IStorableObjects
    {
        private IDictionary<string, object> _objectsToStore;
        private Type _mapType;

        public StorableObjects() {
            _objectsToStore = new Dictionary<string, object>();
            _mapType = null;
        }
        

        public object[] Bundle {
            get => (object[]) _objectsToStore.Values;
        }

        public Type MapType {
            get => _mapType;
            set => throw new NotImplementedException(); }

        public void Add(string key, object value)
        {
            _objectsToStore.Add(key, value);
        }

        public void Remove(string key)
        {
            _objectsToStore.Remove(key);
        }
    }
}
