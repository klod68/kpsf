using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DALBuilder.DomainLayer;

namespace DALBuilder.DomainLayer
{
     /// <summary>
     /// Singleton Abstract Factory object to create root and long-lived objects
     /// </summary>
     public abstract class DALFactory
     {
          /// <summary>
          /// Create a Database setted with a concrete connection
          /// </summary>
          /// <returns></returns>
          public abstract Database MakeDB(Connection connection);
         
          /// <summary>
          /// Create a DirectoryPath object
          /// </summary>
          /// <returns></returns>
          public  DirectoryPath MakeDirectoryPath()
          {
               return new DirectoryPath();
          }

          /// <summary>
          /// Create a Connection object
          /// </summary>
          /// <returns></returns>
          public abstract Connection MakeConnection();

         /// <summary>
         /// Create a Server object
         /// </summary>
         /// <returns></returns>
         public abstract Server MakeServer();

         /// <summary>
         /// Create a Network object
         /// </summary>
         /// <returns></returns>
         public abstract Network MakeNetwork();
         
     }
}
