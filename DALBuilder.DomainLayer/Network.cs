using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace DALBuilder.DomainLayer
{
     /// <summary>
     /// Represents a network.
     /// </summary>
     public abstract class Network
     {
          /// <summary>
          /// Search for sql servers in the network
          /// </summary>
          /// <returns></returns>
          public abstract DataTable GetDatabaseServers();
     }
}
