using System;
using System.Collections.Generic;
using System.Text;

namespace DALBuilder.DomainLayer.SqlServer
{
    /// <summary>
    /// Concrete implementatin of the DASFactory type. Create instance of SQL Server version objects.
    /// TODO: Move creation code to DASFactory?
    /// </summary>
     public class SqlServerDALFactory:DALFactory
     {
         public SqlServerDALFactory() { }

         /// <summary>
         /// Make a SQL Server connection.
         /// </summary>
         /// <returns></returns>
          public override Connection MakeConnection()
          {
               return (Connection)Activator.CreateInstance(Type.GetType(AppSettings.Default.connection));
          }
         /// <summary>
         /// Make a Database from a SQL Server connection.
         /// </summary>
         /// <param name="connection"></param>
         /// <returns></returns>
          public override  Database MakeDB(Connection connection)
          {
               if (connection == null)
                    throw new Exception("Connection is not configured.");

               Type type = Type.GetType(AppSettings.Default.database);

               Database _db = (Database)Activator.CreateInstance(type);
               _db.SetConnection(connection);

               return _db;

          }
         /// <summary>
         /// Make a SQL Server server representation object.
         /// </summary>
         /// <returns></returns>
         public override Server MakeServer()
         {
             return (Server)Activator.CreateInstance(Type.GetType(AppSettings.Default.server));
         }
         /// <summary>
         /// Make a network type.
         /// </summary>
         /// <returns></returns>
         public override Network MakeNetwork()
         {
             return (Network)Activator.CreateInstance(Type.GetType(AppSettings.Default.network));
         }
     }
}
