using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DALBuilder.DomainLayer;
using Microsoft.SqlServer.Management.Smo;

namespace DALBuilder.DomainLayer.SqlServer
{
    public class SqlServerNetwork:Network
    {
        public override DataTable GetDatabaseServers()
        {
            try
            {
                DataTable _sqlServers = SmoApplication.EnumAvailableSqlServers();

                return _sqlServers;
            }
            catch
            {
                return null;
            }
        }
    }
}
