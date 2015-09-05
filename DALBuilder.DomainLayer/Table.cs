using System;
using System.Collections.Generic;
using System.Text;

namespace DALBuilder.DomainLayer
{
     /// <summary>
     /// Represent a selected table in the database.
     /// </summary>
     public abstract class Table
     {
          protected Column[] _columns = null;

          protected string _name;
          protected Database _db;
          protected string _schema;
          protected Column _pk;
          protected Column _ident;

          public Database Database
          {
               set { _db = value; }
               get { return _db; }
          }
          public string Schema
          {
               set { _schema = value; }
               get { return _schema; }
          }
          public string Name
          {
               set { _name = value; }
               get { return _name; }
          }
          public string FullName
          {
               get { return "[" + _schema + "].[" + _name + "]"; }
          }
          public Column[] Columns
          {
               set { _columns = value; }
               get { return _columns; }
          }
          public Column PrimaryKey
          {
               set { _pk = value; }
               get { return _pk; }
          }
          public Column Identity
          {
               set { _ident = value; }
               get { return _ident; }
          }

          /// <summary>
          /// Abstract method to return a StoreProcedureStream class.
          /// </summary>
          /// <returns></returns>
          public abstract StoreProcedureStream MakeStoreProcedureStream();

          /// <summary>
          /// Abstract method to return a ConcurrencySupportCodeString class.
          /// </summary>
          /// <returns></returns>
          public abstract ConcurrencySupportCodeString MakeConcurrencySupportCodeString();

     }
}
