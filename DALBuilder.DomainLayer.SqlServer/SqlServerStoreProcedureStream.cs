using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace DALBuilder.DomainLayer.SqlServer
{
	 public class SqlServerStoreProcedureStream:StoreProcedureStream
	 {
		  private bool _hasPKIdentity = false;

		  public SqlServerStoreProcedureStream():base() { }

		  //public override void StoreFromTable(Table table)
		  //{
		  //    base.StoreFromTable(table);
		  //}
		  /// <summary>
		  /// Generate a sql command stream with SIUD scripts to set text property of the class.
		  /// </summary>
		  protected override void  GenerateStream()
		  {
			   StringBuilder _temp = new StringBuilder();
			//TODO: If pessimistic then lock records
			//when select. 

			_temp.Append(GetSelectOneScript());
			
			   _temp.Append(GetSelectAllScript());
			   _temp.Append(GetInsertScript());

			if(_table.Database.ConcurrencySupport == ConcurrencySupportEnum.None)
				   _temp.Append(GetUpdateScript());

			if (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.Optimistic)
				_temp.Append(GetUpdateWithOptimisticConcurrencySupportScript());

			if (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.PessimisticUserId)
				_temp.Append(GetUpdateWithPessimisticSupportScript("CheckingUserId"));

			if (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.PessimisticUserName)
				_temp.Append(GetUpdateWithPessimisticSupportScript("CheckingUserName"));

			if(_table.Database.ConcurrencySupport == ConcurrencySupportEnum.None)
				_temp.Append(GetDeleteScript());

			if(_table.Database.ConcurrencySupport== ConcurrencySupportEnum.Optimistic)
				_temp.Append(GetDeleteWithOptimisticConcurrencySupportScript());

			if (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.PessimisticUserId)
				_temp.Append(GetDeleteWithPessimisticSupportScript("CheckingUserId"));

			if (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.PessimisticUserName)
				_temp.Append(GetDeleteWithPessimisticSupportScript("CheckingUserName"));
			   _text = _temp.ToString();
		  }
		  protected string GetSelectOneScript()
		  {
			   StringBuilder _temp = new StringBuilder();
			   _temp.Append("USE " + _table.Database.Name + "\n\nGO\n\n");
			   _selectCommand = "[" + _table.Schema + "].[Retrieve" + _table.Name + "]";
			   _temp.Append("CREATE PROC " + _selectCommand + "\n");
			_temp.Append("\t(" + GetPrimaryKeyList(false) + ")\n");
				
			//TODO: if pessimistic lock the selected record
			//if(_table.Database.ConcurrencySupport== ConcurrencySupportEnum.PessimisticUserId)
			//     _temp.Append("\n\t, @inUse uniqueidentifier)\n");
			//else if(_table.Database.ConcurrencySupport== ConcurrencySupportEnum.PessimisticUserName)
			//     _temp.Append("\n\t, @inUse varchar(50))\n");

			   _temp.Append("AS\n");
			   _temp.Append("\tSELECT " + GetColumnList(true,false) + "\n"); 
			   _temp.Append("\tFROM [" + _table.Schema + "].[" + _table.Name + "]\n");
			   //check if there's a where clause condition content
			   _temp.Append("\tWHERE " + GetPrimaryKeyList(true) + "\n");

			//TODO: if pessimistic add update lock
			//if (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.PessimisticUserId
			//     || _table.Database.ConcurrencySupport == ConcurrencySupportEnum.PessimisticUserName)
			//{
			//     _temp.Append("\nGO\n\n");
			//     GetUpdateToLockScript();
			//}
			   _temp.Append("\nGO\n\n");
			   return _temp.ToString();
		  }

		
		protected string GetSelectAllScript()
		  {
			   StringBuilder _temp = new StringBuilder();
			   _temp.Append("USE " + _table.Database.Name + "\n\nGO\n\n");
			_selectAllCommand = "[" + _table.Schema + "].[RetrieveAll" + _table.Name + "]";
			   _temp.Append("CREATE PROC " +  _selectAllCommand + "\n");
			   _temp.Append("AS\n");
			   _temp.Append("\tSELECT " + GetColumnList(true,true) + "\n");
			   _temp.Append("\tFROM [" + _table.Schema + "].[" + _table.Name + "]\n");
			   _temp.Append("\nGO\n\n");
			   return _temp.ToString();
		  }
		protected string GetInsertScript()
		  {
			   StringBuilder _temp = new StringBuilder();
			   _temp.Append("USE " + _table.Database.Name + "\n\nGO\n\n");
			_insertCommand = "[" + _table.Schema + "].[Create" + _table.Name + "]";
			   _temp.Append("CREATE PROC " + _insertCommand + "\n");
			   _temp.Append("\t(" + GetParameterList(true, false,false,false) + ")\n");
			   _temp.Append("AS\n");
			   _temp.Append("\tINSERT INTO [" + _table.Schema + "].[" + _table.Name + "]\n");
			   _temp.Append("\t( " + GetColumnList(false,true) + ")\n");
			   _temp.Append("\tVALUES\n");
			   _temp.Append("\t( " + GetParameterList(false, false,false,false) + " )\n");
			   if (_hasPKIdentity)
					_temp.Append("\n\tRETURN SCOPE_IDENTITY()\n");
			   _temp.Append("\nGO\n\n");
			   return _temp.ToString();
		  }
		protected string GetUpdateScript()
		  {
			   StringBuilder _temp = new StringBuilder();
			   _temp.Append("USE " + _table.Database.Name + "\n\nGO\n\n");
			_updateCommand = "[" + _table.Schema + "].[Update" + _table.Name + "]";
			   _temp.Append("CREATE PROC " + _updateCommand + "\n");
			   _temp.Append("\t(" + GetParameterList(true,true,true,false) + ")\n");
			   _temp.Append("AS\n");
			   _temp.Append("\tUPDATE [" + _table.Schema + "].[" + _table.Name + "]\n");
			   _temp.Append("\tSET " + GetColumnsParameterList() + "\n");
			   _temp.Append("\tWHERE " + GetPrimaryKeyList(true) + "\n");
			   _temp.Append("\nGO\n\n");
			   return _temp.ToString();
		  }
		protected string GetUpdateWithOptimisticConcurrencySupportScript()
		{
			StringBuilder _temp = new StringBuilder();
			_temp.Append("USE " + _table.Database.Name + "\n\nGO\n\n");
			_updateCommand = "[" + _table.Schema + "].[Update" + _table.Name + "CheckingVersion]";
			_temp.Append("CREATE PROC " + _updateCommand + "\n");
			_temp.Append("\t(" + GetParameterList(true, true, true,true) + ")\n");
			_temp.Append("AS\n");
			_temp.Append("\tUPDATE [" + _table.Schema + "].[" + _table.Name + "]\n");
			_temp.Append("\tSET " + GetColumnsParameterList() + "\n");
			_temp.Append("\tWHERE " + GetPrimaryKeyList(true) + "\n");
			   _temp.Append("\tAND [" + AppSettings.Default.OptimisticConcurrencySupportColumn + "]  = @version\n");
			_temp.Append("\nGO\n\n");
			return _temp.ToString();
		}

		protected string GetUpdateWithPessimisticSupportScript(string updateName)
		{
			StringBuilder _temp = new StringBuilder();
			_temp.Append("USE " + _table.Database.Name + "\n\nGO\n\n");
			_updateCommand = "[" + _table.Schema + "].[Update" + _table.Name + updateName + "]";
			_temp.Append("CREATE PROC " + _updateCommand + "\n");
			_temp.Append("\t(" + GetParameterList(true, true, true,true) + ")\n");
			_temp.Append("AS\n");
			_temp.Append("\tUPDATE [" + _table.Schema + "].[" + _table.Name + "]\n");
			_temp.Append("\tSET " + GetColumnsParameterList() + "\n");
			_temp.Append("\tWHERE " + GetPrimaryKeyList(true) + "\n");
			   _temp.Append("\tAND [" + AppSettings.Default.PessimisticConcurrencySupportColum + "]  = @inUse\n");
			_temp.Append("\nGO\n\n");
			return _temp.ToString();
		}

		protected string GetDeleteScript()
		  {
			   StringBuilder _temp = new StringBuilder();
			   _temp.Append("USE " + _table.Database.Name + "\n\nGO\n\n");
			_deleteCommand = "[" + _table.Schema + "].[Delete" + _table.Name + "]";
			   _temp.Append("CREATE PROC " + _deleteCommand + "\n");
			   _temp.Append("\t(" + GetPrimaryKeyList(false) + ")\n");
			   _temp.Append("AS\n");
			   _temp.Append("\tDELETE [" + _table.Schema + "].[" + _table.Name + "]\n");
			   _temp.Append("\tWHERE " + GetPrimaryKeyList(true) + "\n");
			   _temp.Append("\nGO\n\n");
			   return _temp.ToString();
		  }

		protected string GetDeleteWithOptimisticConcurrencySupportScript()
		{
			StringBuilder _temp = new StringBuilder();
			_temp.Append("USE " + _table.Database.Name + "\n\nGO\n\n");
			_deleteCommand = "[" + _table.Schema + "].[Delete" + _table.Name + "]";
			_temp.Append("CREATE PROC " + _deleteCommand + "\n");
			_temp.Append("\t(" + GetPrimaryKeyList(false));
			_temp.Append("\n\t, @version timestamp)\n");
			_temp.Append("AS\n");
			_temp.Append("\tDELETE [" + _table.Schema + "].[" + _table.Name + "]\n");
			_temp.Append("\tWHERE " + GetPrimaryKeyList(true) + "\n");
			_temp.Append("\tAND [" + AppSettings.Default.OptimisticConcurrencySupportColumn + "] = @version\n");
			_temp.Append("\nGO\n\n");
			return _temp.ToString();
		}

		protected string GetDeleteWithPessimisticSupportScript(string deleteName)
		{
			StringBuilder _temp = new StringBuilder();
			_temp.Append("USE " + _table.Database.Name + "\n\nGO\n\n");
			_deleteCommand = "[" + _table.Schema + "].[Delete" + _table.Name + deleteName + "]";
			_temp.Append("CREATE PROC " + _deleteCommand + "\n");
			_temp.Append("\t(" + GetPrimaryKeyList(false));
			
			if(_table.Database.ConcurrencySupport== ConcurrencySupportEnum.PessimisticUserId)
				_temp.Append("\n\t, @inUse uniqueidentifier)\n");
			else if(_table.Database.ConcurrencySupport== ConcurrencySupportEnum.PessimisticUserName)
				_temp.Append("\n\t, @inUse varchar(50))\n");

			_temp.Append("AS\n");
			_temp.Append("\tDELETE [" + _table.Schema + "].[" + _table.Name + "]\n");
			_temp.Append("\tWHERE " + GetPrimaryKeyList(true) + "\n");
			_temp.Append("\tAND [" + AppSettings.Default.PessimisticConcurrencySupportColum + "] = @inUse\n");
			_temp.Append("\nGO\n\n");
			return _temp.ToString();
		}
		  //TODO: set these public methods to private
		  /// <summary>
		  /// Get primary key for parameter or where clause section (filter=true).
		  /// </summary>
		  /// <param name="filter"></param>
		  /// <returns></returns>
		protected string GetPrimaryKeyList(bool filter)
		  {
			   //check pk or identity cols

			   StringBuilder _temp = new StringBuilder(); 
			   string _cont1 = string.Empty;
			   string _cont2 = string.Empty;

			   foreach (Column _col in _table.Columns)
			   {
					if (!_col.IsPrimaryKey && !_col.IsIdentity)
						 continue;

					if (filter)
					{
						 _temp.Append(_cont1 + "[" + _col.Name + "] = " + _col.ParameterName);
						 _cont1 = "\n\t\t AND ";
					}
					else
					{
						 _temp.Append(_cont2 + _col.ParameterName + " " + _col.DataType);
						 _cont2 = "\n\t, ";
					}
			   }

			
			return _temp.ToString();
		  }
		  /// <summary>
		  /// Get body column list with or without auto generate columns.
		  /// </summary>
		  /// <param name="withAutoCol"></param>
		  /// <returns></returns>
		protected string GetColumnList(bool withAutoCol, bool withPK)
		  {
			   StringBuilder _temp = new StringBuilder();
			   string _cont = string.Empty;

			   foreach (Column _col in _table.Columns)
			   {
					bool _auto = _col.IsIdentity || _col.IsRowGuid || _col.IsComputed || _col.IsRowversion;

					if ((!withAutoCol && _auto) || (!withPK && _col.IsPrimaryKey))
						 continue;

				
					_temp.Append(_cont + "[" + _col.Name + "]");
					_cont = "\n\t, ";
			   }
			   if (_temp.ToString() == "")
					_temp.Append("*");
			   return _temp.ToString();
		  }
		  /// <summary>
		  /// Get a parameter list for all circunstances.
		  /// </summary>
		  /// <param name="withType"></param>
		  /// <param name="withAutoCols"></param>
		  /// <returns></returns>
		protected string GetParameterList(bool withType, bool withIdentity, bool withRowGuid, bool withRowversion)
		  {
			   StringBuilder _temp = new StringBuilder();
			   //bool _one = false;
			   string _cont = string.Empty;

			   foreach (Column _col in _table.Columns)
			   {                    
					
					//Check if it's identity to return scope_identity
					if (_col.IsIdentity && _col.IsPrimaryKey && !_hasPKIdentity )
						 _hasPKIdentity = true;

					//TODO: how to return a new guid from database

					if (_col.IsComputed)
						 continue;

				if (!withRowversion && _col.IsRowversion)
					continue;

					if ((!withIdentity && _col.IsIdentity)|| (!withRowGuid && _col.IsRowGuid))
						 continue;

					_temp.Append(_cont + _col.ParameterName);
					if (withType)
						 _temp.Append(" " + _col.DataType);

					_cont = "\n\t, ";
			   }
			   return _temp.ToString();
		  }
		  /// <summary>
		  /// Get a column=parameter list for update procedures.
		  /// </summary>
		  /// <returns></returns>
		  protected string GetColumnsParameterList()
		  {
			   StringBuilder _temp = new StringBuilder();
			   string _cont = string.Empty;

			   foreach (Column _col in _table.Columns)
			   {
					bool _auto = _col.IsIdentity || _col.IsRowGuid || _col.IsComputed || _col.IsRowversion;

					//TODO: should include no auto generate primary keys for update?
					//if yes, I need to include a newKey and oldKey parameters in update
					if (_auto || _col.IsPrimaryKey)
						 continue;

					_temp.Append(_cont + "[" + _col.Name + "] = " + _col.ParameterName);
					_cont = "\n\t\t, ";
				   
			   }
			   return _temp.ToString();
		  }

		  protected override void  MakeMappingsStream()
		  {
			  _mappings = new SqlServerMappingsXMLStream();
			  _mappings.SetStreamFromProcedures(this);
		  }
	 }
}
