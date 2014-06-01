using System;
using System.Collections.Generic;
using System.Text;

using Klod.Data.PersistenceService.Persistent;

namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	/// <summary>
	/// Implementation of a Composite operations command for SQL Server relational databases.
	/// </summary>
	public class SqlServerCompositeCommand : CompositeCommand
	{
		public SqlServerCompositeCommand() : base() { }
		public override void Sort()
		{
			//TODO: implement a sorting algorithm
		}
		//TODO: prepares the set to map multiple objects to parameters
		public override IPersistable MakePersistable()
		{
			return new PersistentObject();
		}

		public override IPersistable MakePersistable(object persistableObj, ObjectIdentifier oid)
		{
			return new PersistentObject(persistableObj, oid);
		}
	}
}
