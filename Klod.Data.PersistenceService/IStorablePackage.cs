using System;
using System.Collections;
using System.Collections.Generic;

namespace Klod.Data.PersistenceService
{
    /// <summary>
    /// Represents a set of object to be persistent with a command and a map instruction type
    /// </summary>
    public interface IStorablePackage
    {
        object[] GetObjects();
        object GetFirst();
        object GetByKey(string key);
        void Add(string key, object obj);
        Type MapType { get; set; }
        string PersistenceCommand { get; set; }
    }
}
