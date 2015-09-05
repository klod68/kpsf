using System;
using System.Collections.Generic;
using System.Text;


namespace DALBuilder.DomainLayer
{
     /// <summary>
     /// Represents stored procedures stream.
     /// </summary>
     public abstract class StoreProcedureStream
     {
            protected string _selectCommand;
            protected string _selectAllCommand;
            protected string _insertCommand;
            protected string _updateCommand;
            protected string _deleteCommand;

            //TODO:concurrency support sprocs

            protected string _updateCheckLock;
            protected string _updateCheckLockUID;
            protected string _updateCheckTimestamp;

            protected string _deleteCheckLock;
            protected string _deleteCheckLockUID;
            protected string _deleteCheckTimestamp;
          //end TODO

            protected string _text;
            protected Table _table;
            protected MappingsXMLStream _mappings;

            public string SelectCommand
            {
               set { _selectCommand = value; }
               get { return _selectCommand; }
            }
            public string SelectAllCommand
            {
               set { _selectAllCommand = value; }
               get { return _selectAllCommand; }
            }

            public string InsertCommand
            {
               set { _insertCommand = value; }
               get { return _insertCommand; }
            }

            public string UpdateCommand
            {
               set { _updateCommand = value; }
               get { return _updateCommand; }
            }
            public string UpdateCheckLock
            {
               set { _updateCheckLock = value; }
               get { return _updateCheckLock; }
            }
            public string UpdateCheckLockUID
            {
               set { _updateCheckLockUID = value; }
               get { return _updateCheckLockUID; }
            }
            public string UpdateCheckTimestamp
            {
               set { _updateCheckTimestamp = value; }
               get { return _updateCheckTimestamp; }
            }

            public string DeleteCommand
            {
               set { _deleteCommand = value; }
               get { return _deleteCommand; }
            }
            public string DeleteCheckLock
            {
               set { _deleteCheckLock = value; }
               get { return _deleteCheckLock; }
            }
            public string DeleteCheckLockUID
            {
               set { _deleteCheckLockUID = value; }
               get { return _deleteCheckLockUID; }
            }
            public string DeleteCheckTimestamp
            {
               set { _deleteCheckTimestamp = value; }
               get { return _deleteCheckTimestamp; }
            }

            public string Text
            {
               set { _text = value; }
               get { return _text; }
            }
            public Table Table
            {
               set { _table = value; }
               get { return _table; }
            }
            public MappingsXMLStream Mappings
            {
               get { return _mappings; }
            }

            protected StoreProcedureStream() 
            { }

            
            /// <summary>
            /// Template contructor method
            /// </summary>
            /// <param name="name"></param>
            /// <param name="table"></param>
            public virtual void StoreFromTable(Table table)
            {
               _table = table;
                //template methods
               GenerateStream();
               MakeMappingsStream();
            }


            /// <summary>
            /// Method for generate the specific store procedure stream.
            /// </summary>
            /// <returns></returns>
            protected abstract void GenerateStream();
            protected abstract void MakeMappingsStream();
          


     }
}
