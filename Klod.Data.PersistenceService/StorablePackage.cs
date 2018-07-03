using System;
using System.Collections.Generic;

namespace Klod.Data.PersistenceService
{
    public class StorablePackage : IStorablePackage
    {
        private IDictionary<string, object> _bundle;
        private object _first;
        private Type _mapType;
        private string _persistenceCmd;

        public StorablePackage()
        {
            _mapType = null;
            _persistenceCmd = string.Empty;
            _first = null;
            _bundle = new Dictionary<string, object>();
        }
        public StorablePackage(string key, object first) {
            _mapType = null;
            _persistenceCmd = string.Empty;
            _first = first;
            _bundle = new Dictionary<string, object>();
            _bundle.Add(key, first);
        }
        

        public object[] GetObjects()
        {
                object[] _bundle = new object[this._bundle.Count];

                this._bundle.Values.CopyTo(_bundle, 0);
                return _bundle;
        }

        public Type MapType {
            get => _mapType;
            set => _mapType = value;
        }

        public string PersistenceCommand
        {
            get => _persistenceCmd;
            set => _persistenceCmd = value;
        }

        public void Add(string key, object value)
        {
            if (_bundle.Count == 0)
                _first = value;
            _bundle.Add(key, value);
        }

        public object GetFirst()
        {
            return _first;
        }

        public object GetByKey(string key)
        {
            if (_bundle.ContainsKey(key))
                return _bundle[key];
            return null;
        }
    }
}
