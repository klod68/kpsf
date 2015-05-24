using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	//ok crr 2009.11.08 move the objects for process to the base class.
	//ok crr 2009.11.05
	/// <summary>
	/// Implementation of a generic transaction command.
	/// </summary>
	public class SqlServerTransactionCommand : SqlServerPersistenceCommand
	{

		//Needed for concurrency support with multiple objects and custom command
		public SqlServerTransactionCommand(Map map, SqlCommand command, string cmdType, ConcurrencySupportType concurrency, bool returnCollection, IDictionary<string,object> objectsForProcess)
			: base(map, cmdType, command, concurrency, returnCollection, objectsForProcess) { }
		public SqlServerTransactionCommand(Map map, SqlCommand command, CRUD cmdType, IDictionary<string,object> objectsForProcess)
			: base(map, command, cmdType, objectsForProcess) { }

		protected override void SetParametersValues()
		{
			try
			{					
				//Get and set correct values from objects to all parameters
				for (byte i = 0; i < ParametersSet.GetLength(0); i++)
				{
					string classToken = CommandMap.GetClassToken(i); //avoid multiple calls

					if (ParametersSet[i].Direction == System.Data.ParameterDirection.ReturnValue 
                        ||ParametersSet[i].Direction==System.Data.ParameterDirection.Output)
					{
						ParametersSet[i].Value = 0;
						continue;
					}
					
					//REFACTORING: must be capable to use same class name with the token property as unique id.
					if(ObjectCollectionForProcess.ContainsKey(classToken))
					{
						ParametersSet[i].Value = ObjectCollectionForProcess[classToken].GetType().GetProperty(CommandMap.GetProperty(i)).GetValue(ObjectCollectionForProcess[classToken], null);
					}
				}
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}
	}
}
