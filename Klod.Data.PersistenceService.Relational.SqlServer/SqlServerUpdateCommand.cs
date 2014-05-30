using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Security.Principal;

namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	//ok crr 2009.11.05
	/// <summary>
	/// Implementation of Update command to change an existing record.
	/// </summary>
	public class SqlServerUpdateCommand : SqlServerPersistenceCommand
	{
		public SqlServerUpdateCommand(Map map, IPersistable persistent, SqlCommand command)
			: base(map, persistent, command, CRUD.Update) { }

		//Overloaded the constructor to support concurrency (CRUD.UpdateWithTimestamp,...)
		public SqlServerUpdateCommand(Map map, IPersistable persistent, SqlCommand command, CRUD cmdType)
			: base(map, persistent, command, cmdType) { }

		protected override void SetParametersValues()
		{
			ParametersSet[0].Value = ObjectToPersist.OID.ID;

			for (byte i = 1; i < ParametersSet.GetLength(0); i++)
			{
				if (ParametersSet[i].Use == ParameterUse.Regular)
					ParametersSet[i].Value = ObjectToPersist.StoredObject.GetType().GetProperty(CommandMap.GetProperty(i)).GetValue(ObjectToPersist.StoredObject, null);
				else if (ParametersSet[i].Use == ParameterUse.RowVersion)
					ParametersSet[i].Value = ObjectToPersist.Version;
				else if (ParametersSet[i].Use == ParameterUse.UserName)
					ParametersSet[i].Value = WindowsIdentity.GetCurrent().Name;
				else if (ParametersSet[i].Use == ParameterUse.UserId)
					ParametersSet[i].Value = WindowsIdentity.GetCurrent().User.Value; //TODO: TO be implemented. Who must check concurrency conflicts.

			}
		}
	}
}
