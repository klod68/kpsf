using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DALBuilder.DomainLayer
{
     /// <summary>
     /// Represents the database server.
     /// </summary>
     public abstract class Server
     {
            /// <summary>
            /// Get the databases stored in the SQL Server.
            /// </summary>
            /// <param name="serverName"></param>
            /// <returns></returns>
        public abstract DataTable GetDatabases(string serverName);
     }
}
