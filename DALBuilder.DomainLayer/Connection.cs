using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace DALBuilder.DomainLayer
{
     /// <summary>
     /// Wrap type (abstract class) that represents a connection object to any data source.
     /// </summary>
     public abstract class Connection
     {
          protected string _settings;
          /// <summary>
          /// Settings information to connect to the database server.
          /// </summary>
          public string Settings
          {
               set { _settings = value; }
               get { return _settings; }
          }

          public Connection() { }
          /// <summary>
          /// Establish a connection with a data source
          /// implemented in the concrete derived classes
          /// </summary>
          public abstract bool Connect(string settings);
          /// <summary>
          /// Parse a parameter string array and return the connection
          /// specific connection string.
          /// </summary>
          /// <param name="setting"></param>
          /// <returns></returns>
          public abstract string ParseSettings(params string[] setting);
          
          /// <summary>
          /// Encapsulate the specific connection object configured for connection.
          /// </summary>
          public abstract object ConnectionObject {set;get;}

          
     }
}
