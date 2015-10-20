using System;
using System.Data;

namespace Klod.Data.PersistenceService.Relational
{
    public abstract class RelationalDatabaseMapper : PersistenceMapper
	{
		private string _tableName;

		public string TableName
		{
			set { _tableName = value; }
			get { return _tableName; }
		}
		
		public RelationalDatabaseMapper(string tableName) : base() { _tableName = tableName; }


		protected override bool PutObjectInStorage(ObjectIdentifier oid, Object appObject)
		{
			return StoreObjectInDB(oid, appObject);
		}

		protected override ObjectIdentifier PutObjectInStorage(Object appObject)
		{
			return StoreObjectInDB(appObject);
		}

		protected override IPersistable GetObjectFromStorage(ObjectIdentifier oid)
		{
			DataView dvw = GetDBRecord(oid);
			return GetObjectFromRecord(oid, dvw);
		}

		protected override bool ChangeObjectInStorage(IPersistable pObject)
		{
			return UpdateObjectInDB(pObject);
		}

		protected override bool DestroyObjectInStorage(ObjectIdentifier oid)
		{
			return DeleteObjectInDB(oid);
		}

		public abstract DataView GetDBRecord(ObjectIdentifier oid);
		public abstract IPersistable GetObjectFromRecord(ObjectIdentifier oid, DataView record);
		public abstract ObjectIdentifier StoreObjectInDB(Object appObject);
		public abstract bool StoreObjectInDB(ObjectIdentifier oid, Object appObject);

		public abstract bool UpdateObjectInDB(IPersistable pObject);
		public abstract bool DeleteObjectInDB(ObjectIdentifier oid);


	}
}
