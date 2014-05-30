using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	//ok crr 2009.11.05
	/// <summary>
	/// Implementatin of a delete command for destroying records.
	/// </summary>
	public class SqlServerDeleteCommand : SqlServerPersistenceCommand
	{

		public SqlServerDeleteCommand(Map map, ObjectIdentifier oid, SqlCommand command)
			: base(map, oid, command, CRUD.Delete) { }
		public SqlServerDeleteCommand(Map map, ObjectIdentifier oid, SqlCommand command, CRUD cmdType)
			: base(map, oid, command, cmdType) { }
		public SqlServerDeleteCommand(Map map, ObjectIdentifier oid, SqlCommand command, string cmdType, ConcurrencySupportType concurrency)
			: base(map, oid, command, cmdType, concurrency) { }

		protected override void SetParametersValues()
		{
			try
			{
				ParametersSet[0].Value = ObjectToPersist.OID.ID;

			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}
	}
}
