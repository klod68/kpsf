using System.Collections.Generic;
using Klod.Data.PersistenceService.Identifiers;

namespace Klod.Data.PersistenceService.Persistent
{
    //CHECK: Why it doesn't have behavior?
    /// <summary>
    /// Wrap class for add persistence capabilities to common domain objects
    /// </summary>
    public class PersistentObject : IPersistable
	{
		private ObjectIdentifier _oid;
		private object _me;
		private object _version;      //optimistic support: store timestamp info.
		private object _inUse; //pessimistic: if there's a guid or a username then it's in use   
		private Dictionary<string, ObjectIdentifier> _references;

		public PersistentObject()
		{
			_me = null;
			_oid = new OID();
			_version = null;
			_inUse = null;
			_references = new Dictionary<string, ObjectIdentifier>();
		}

		public PersistentObject(object persistableObj, ObjectIdentifier oid)
		{
			_me = persistableObj;
			_oid = oid;
			_version = null;
			_inUse = null;
			_references = new Dictionary<string, ObjectIdentifier>();
		}
		public PersistentObject(object persistableObj, ObjectIdentifier oid, object version, object inUse)
		{
			_me = persistableObj;
			_oid = oid;
			_version = version;
			_inUse = InUse;
			_references = new Dictionary<string, ObjectIdentifier>();
		}
		public ObjectIdentifier OID
		{
			set { _oid = value; }
			get { return _oid; }
		}

		public object StoredObject
		{
			set { _me = value; }
			get { return _me; }
		}
		public object Version
		{
			set { _version = value; }
			get { return _version; }
		}
		public object InUse
		{
			set { _inUse = value; }
			get { return _inUse; }
		}
		public ObjectIdentifier[] References
		{
			set
			{
				_references.Clear();
				for (int i = 0; i < value.GetLength(0); i++)
				{
					_references.Add(value[i].FieldName, value[i]);
				}
			}
			get
			{
				ObjectIdentifier[] returnOids = new ObjectIdentifier[_references.Count];
				ObjectIdentifier[] oids = new ObjectIdentifier[_references.Count];
				//string[] ids = new string[_references.Count];

				//_references.Keys.CopyTo(ids, 0);
				_references.Values.CopyTo(oids, 0);

				for (int i = 0; i < _references.Count; i++)
				{

					returnOids[i] = oids[i];
				}

				return returnOids;
			}
		}

		public bool AddReference(ObjectIdentifier reference)
		{
			try
			{
				_references.Add(reference.FieldName, reference);
				return true;
			}
			catch
			{
				return false;
			}
		}
		public bool RemoveReference(ObjectIdentifier reference)
		{
			try
			{
				if (_references.ContainsKey(reference.FieldName))
				{
					_references.Remove(reference.FieldName);
				}
				return true;

			}
			catch
			{
				return false;
			}
		}
		public ObjectIdentifier GetReference(string fieldName)
		{
			if (_references.ContainsKey(fieldName))
				return _references[fieldName];
			return null;
		}
		public bool HasReference(string fieldName)
		{
			if (_references.ContainsKey(fieldName))
				return true;
			return false;
		}
	}
}
