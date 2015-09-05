using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DALBuilder.DomainLayer.SqlServer
{
    public class SqlServer:Server
    {
        public SqlServer() { }
        public override DataTable GetDatabases(string serverName)
        {
            DataTable _databases = new DataTable();
            _databases.Columns.Add("database");
            Microsoft.SqlServer.Management.Smo.Server _server = new Microsoft.SqlServer.Management.Smo.Server(serverName);
            if (_server != null)
            {
                foreach (Microsoft.SqlServer.Management.Smo.Database _db in _server.Databases)
                {
                    if (!_db.IsSystemObject)
                        _databases.Rows.Add(_db.Name);
                }
                return _databases;
            }
            return null;
        }
    }
}
