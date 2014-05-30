using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	//ok crr 2009.11.05
	/// <summary>
	/// Implementation of a create command for inserts new records.
	/// </summary>
	public class SqlServerCreateCommand : SqlServerPersistenceCommand
	{

		public SqlServerCreateCommand(Map map, object appObject, SqlCommand command)
			: base(map, appObject, command)
		{}
		public SqlServerCreateCommand(Map map, ObjectIdentifier oid, object appObject, SqlCommand command)
			: base(map, oid, command, CRUD.Create) 
		{ }
		protected override void SetParametersValues()
		{
			try
			{
				for (byte i = 0; i < ParametersSet.GetLength(0); i++)
				{
					if (ParametersSet[i].Name == "@RETURN_VALUE")
					{
						ParametersSet[i].Value = 0;
						continue;
					}
					ParametersSet[i].Value = ObjectForProcess.GetType().GetProperty(CommandMap.GetProperty(i)).GetValue(ObjectForProcess, null);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Cannot set the values for the parameters array.", ex);
			}
		}
	}
}
