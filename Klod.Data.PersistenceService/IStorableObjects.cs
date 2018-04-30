using System;

namespace Klod.Data.PersistenceService
{
    public interface IStorableObjects
    {
        object[] Bundle { get; }
        Type MapType { get; set; }

        void Add(string key, object value);
        void Remove(string key);
    }
}
