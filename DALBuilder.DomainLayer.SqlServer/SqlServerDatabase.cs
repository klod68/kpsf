using System;
using System.Collections.Generic;
using System.Text;
using DALBuilder.DomainLayer;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DALBuilder.DomainLayer.SqlServer
{
	 public class SqlServerDatabase:Database,IDisposable
	 {
		  protected SqlDataAdapter _da = null;
		  protected SqlCommand _cmd = null;
		  protected SqlConnection _sqlCn = null;
		  protected DataRow[] _currentTable = null;

	 #region table metadata selects statement
		  const string _tablesInDB = @"SELECT [name],[object_id],[type],schema_name(schema_id) [schema_name] FROM sys.tables WHERE [type]='U' AND [name] <> 'sysdiagrams'";

		  const string _getColumnsByTableText = @"SELECT distinct c.[object_id]
							 ,c.column_id
							 ,c.[name]
							 ,type_name(system_type_id) data_type
							 ,max_length
							 , [precision]
							 ,scale
							 ,is_rowguidcol
							 ,is_identity
							 ,is_computed
							 --,i.index_id
							 ,is_primary_key
							 --,i.[name] index_name 
						 FROM sys.columns c 
							 LEFT JOIN sys.index_columns ic 
								 ON c.column_id=ic.column_id 
									 AND c.[object_id]=ic.[object_id] 
							 RIGHT JOIN sys.indexes i 
								 ON ic.[object_id]=i.[object_id] 
									 AND ic.index_id=i.index_id 
						 WHERE c.[object_id]=@object_id
						 and is_primary_key=1
						 UNION
						 SELECT distinct [object_id]
							 ,column_id
							 ,[name]
							 ,type_name(system_type_id) data_type
							 ,max_length
							 ,[precision]
							 ,scale
							 ,is_rowguidcol
							 ,is_identity
							 ,is_computed
							 ,null 'is_primary_key'
						 FROM sys.columns
						 WHERE [object_id]=@object_id and [name] not in
						 (SELECT distinct 
							 c.[name]
						 FROM sys.columns c 
							 LEFT JOIN sys.index_columns ic 
								 ON c.column_id=ic.column_id 
									 AND c.[object_id]=ic.[object_id] 
							 RIGHT JOIN sys.indexes i 
								 ON ic.[object_id]=i.[object_id] 
									 AND ic.index_id=i.index_id 
						 WHERE c.[object_id]=@object_id
						 and is_primary_key=1)";
#endregion
		  
		  //public SqlServerDatabase(Connection connection) : base(connection) {}

		  /// <summary>
		  /// Specific initialization code. Hook method implemented.
		  /// Load all selected database tables with object_ids
		  /// </summary>
		  /// <param name="connection"></param>
		protected override void  LoadDataSetWithDataSourceMetadata(Connection connection)
		{
			if (connection == null)
				return;

			_sqlCn = (SqlConnection)connection.ConnectionObject;

			if (_ds == null)
				_ds = new DataSet();

			_ds.DataSetName = _sqlCn.Database;

			//get a table with all tables and its metadata in the database
			_da = new SqlDataAdapter(_tablesInDB, _sqlCn);
			if (_da != null)
			{
				_da.Fill(this._ds, "TablesInDb");
			}
			
		}
		public override string[] GetTableNames()
		{
			//get all table names
			string[] _tableNames = new string[_ds.Tables["TablesInDb"].Rows.Count];
			for (byte i = 0; i < _tableNames.GetLength(0); i++)
			{
				_tableNames[i] = _ds.Tables["TablesInDb"].Rows[i]["name"].ToString();
			}
			return _tableNames;
		}
		
		  /// <summary>
		  /// Generate concurrency support code.
		  /// </summary>
		  /// <param name="concurrencySupport"></param>
		  /// <returns></returns>
		  public override ConcurrencySupportCodeString[] GenerateConcurrencySupportCode()
		  {

			   if (_concurrency == ConcurrencySupportEnum.None)
					return null;

			   ConcurrencySupportCodeString[] csStrings = new SqlServerConcurrencySupportCodeString[_selectedTables.GetLength(0)];

			   for (byte i = 0; i < _selectedTables.GetLength(0); i++)
			   {
					csStrings[i] = GetTable(_selectedTables[i]).MakeConcurrencySupportCodeString();
			   }
			   return csStrings;
		  }
		  
		  /// <summary>
		  /// Generate Sprocs scripts using a SqlServerStoreProcedureStream class.
		  /// </summary>
		  /// <returns></returns>
		  public override StoreProcedureStream[] GenerateSProcs()
		  {
			   StoreProcedureStream[] sprocs = new SqlServerStoreProcedureStream[_selectedTables.GetLength(0)];               

			   //Create a table object with all columns metadata configured
			   for (byte i = 0; i < _selectedTables.GetLength(0); i++)
			   {
					//create a new sproc stream for each table
					sprocs[i] = GetTable(_selectedTables[i]).MakeStoreProcedureStream();
			   }
			  
			   return sprocs;
		  }
		 
		  /// <summary>
		  /// Load all selected tables metadata in the dataset.
		  /// </summary>
		  protected override void LoadDataSetWithSelectedTablesMetadata()
		  {
			   try
			   {
					//open connection for best performance
					_sqlCn.Open();

					for (byte i = 0; i < _selectedTables.GetLength(0); i++)
					{
						 //Fill the metadata of each table
						 _cmd = _sqlCn.CreateCommand();
						 _cmd.CommandText = _getColumnsByTableText;
						 SqlParameter _parm = _cmd.Parameters.Add("@object_id", SqlDbType.Int);

						 _currentTable = _ds.Tables["TablesInDb"].Select("name='" + _selectedTables[i] + "'", "name");
						 _parm.Value = (int)_currentTable[0]["object_id"];

						 _da = new SqlDataAdapter(_cmd);
						 _da.Fill(_ds, _selectedTables[i]);

					}
					_sqlCn.Close();
			   }
			   catch (Exception ex)
			   {
					Debug.Write(ex.Message);
			   }

		  }

		  protected override Table GetTable(string tableName)
		  {
			  //Create a table object with all columns metadata configured
			 
			   Table _table = new SqlServerTable();
			   _table.Name = tableName;
			   
			   //get record of table name, object_id, schema
			   _currentTable = _ds.Tables["TablesInDb"].Select("name='" + tableName + "'", "name");
			   //get schema of the table
			   _table.Schema = _currentTable[0]["schema_name"].ToString();
			   
			   //set database name
			   _table.Database = this;

			   Column[] _columns = new Column[_ds.Tables[tableName].Rows.Count];
			   
			   //configure each column metadata for the columns collection of the table object
			   for(byte i=0;i<_columns.GetLength(0);i++)
			   {
					//Read each column in table and set its value
					_columns[i] = new Column();
					_columns[i].Table = _table;
					_columns[i].DataType = _ds.Tables[tableName].Rows[i]["data_type"].ToString();
				_columns[i].Name = _ds.Tables[tableName].Rows[i]["name"].ToString(); ;
				_columns[i].IsRowversion = (_ds.Tables[tableName].Rows[i]["data_type"].ToString().ToUpper() == "TIMESTAMP");
				
				if(_table.Database.ConcurrencySupport == ConcurrencySupportEnum.Optimistic)
					_columns[i].IsOptimisticConcurrencySupport = _columns[i].Name.ToLower() == AppSettings.Default.OptimisticConcurrencySupportColumn.ToLower();
				if(_table.Database.ConcurrencySupport == ConcurrencySupportEnum.PessimisticUserId
					|| _table.Database.ConcurrencySupport== ConcurrencySupportEnum.PessimisticUserName)
					_columns[i].IsPessimisticConcurrencySupport = _columns[i].Name.ToLower() == AppSettings.Default.PessimisticConcurrencySupportColum.ToLower();
	
				_columns[i].ID = (int)_ds.Tables[tableName].Rows[i]["column_id"];
					_columns[i].IsComputed = (bool)_ds.Tables[tableName].Rows[i]["is_computed"];

					//_columns[i].IsIdentity = (bool)_ds.Tables[tableName].Rows[i]["is_identity"];
					if (!Convert.IsDBNull(_ds.Tables[tableName].Rows[i]["is_identity"]))
					{
						 _columns[i].IsIdentity = (bool)_ds.Tables[tableName].Rows[i]["is_identity"];
						 if (_columns[i].IsIdentity)
							  _table.Identity = _columns[i];
					}
					_columns[i].IsRowGuid = (bool)_ds.Tables[tableName].Rows[i]["is_rowguidcol"];
					_columns[i].MaxLength = (short)_ds.Tables[tableName].Rows[i]["max_length"];
					_columns[i].Precision = (byte)_ds.Tables[tableName].Rows[i]["precision"];
					_columns[i].Scale = (byte)_ds.Tables[tableName].Rows[i]["scale"];
					if (!Convert.IsDBNull(_ds.Tables[tableName].Rows[i]["is_primary_key"]))
					{
						 _columns[i].IsPrimaryKey = (bool)_ds.Tables[tableName].Rows[i]["is_primary_key"];
						 if (_columns[i].IsPrimaryKey)
							  _table.PrimaryKey = _columns[i];
					}
					else
						 _columns[i].IsPrimaryKey = false;

			   }
			   _table.Columns = _columns;
			   return _table;
		  }


		  public void Dispose()
		  {
			  Dispose(true);
			  GC.SuppressFinalize(this);
		  }

		  protected virtual void Dispose(bool disposing)
		  {
			  if (disposing)
			  {
				  if (_da != null)
				  {
					  _da.Dispose();
					  _da = null;
				  }
			  }
		  }
	 }
}
