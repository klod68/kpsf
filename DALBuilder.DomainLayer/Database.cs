using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace DALBuilder.DomainLayer
{
     /// <summary>
     /// Abstract class representing a database. Use a dataset object to represent
     /// an in memory selected datatables collection.
     /// </summary>
     public abstract class Database
     {
          protected DataSet _ds;			//a ds that represents the datasource metadata
          protected string[] _selectedTables;//represents the submitted tables for processing
          protected ConcurrencySupportEnum _concurrency; //store concurrency support to this datasource

          /// <summary>
          /// Default constructor.
          /// </summary>
		public Database() {}

          /// <summary>
          /// Template constructor method.
          /// </summary>
          public void SetConnection(Connection connection)
          {
			//_cn = connection;
            //Template method, child must override details
			LoadDataSetWithDataSourceMetadata(connection);
          }

          #region properties
		public string Name
		{
			set { _ds.DataSetName = value; }
			get { return _ds.DataSetName; }
		}
		public ConcurrencySupportEnum ConcurrencySupport
          {
               set { _concurrency = value; }
               get { return _concurrency; }
          }
          #endregion

          #region public methods
          /// <summary>
          /// Get al tables selected in the database.
          /// </summary>
          /// <returns></returns>
		public abstract string[] GetTableNames();
          
          /// <summary>
          /// Submit the selected tables an loads them in the dataset.
          /// Use a template method to encapsulate the loading and postpone implementation
          /// to derived.
          /// </summary>
          /// <param name="tables"></param>
          public void SubmitTablesSelected(string[] tables)
          {
               _selectedTables = tables;
               LoadDataSetWithSelectedTablesMetadata();
          }
          /// <summary>
          /// Generate concurrency support code.
          /// </summary>
          /// <param name="concurrencySupport"></param>
          /// <returns></returns>
          public abstract ConcurrencySupportCodeString[] GenerateConcurrencySupportCode();

          /// <summary>
          /// Generate Stored procedures to be implemented by data source provider code
          /// </summary>
          /// <returns></returns>
          public abstract StoreProcedureStream[] GenerateSProcs();

          #endregion

          #region protected methods

          //hook method to be implemented by derived
		protected abstract void LoadDataSetWithDataSourceMetadata(Connection connection);

          //All database must load the dataset with selected tables metadata
          protected abstract void LoadDataSetWithSelectedTablesMetadata();

          /// <summary>
          /// Get a table object from name.
          /// </summary>
          /// <param name="tableName"></param>
          /// <returns></returns>
          protected abstract Table GetTable(string tableName);


          #endregion
     }
}
