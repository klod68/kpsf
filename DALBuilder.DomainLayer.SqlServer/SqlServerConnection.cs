using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace DALBuilder.DomainLayer.SqlServer
{
     public class SqlServerConnection:Connection,IDisposable
     {
          private SqlConnection _sqlCn;

          public override Object ConnectionObject
          {
               set { _sqlCn = (SqlConnection)value; }
               get { return _sqlCn; }
          }
          /// <summary>
          /// Connect to the database using settings parameter
          /// or protected settings property value
          /// </summary>
          /// <param name="settings"></param>
          /// <returns></returns>
          public override bool Connect(string settings)
          {
               if(settings!=null && settings!=string.Empty)
                    _sqlCn = new SqlConnection(settings);
               else if (this._settings != null && this._settings != string.Empty)
                    _sqlCn = new SqlConnection(this._settings);
               else
                    return false;

               if (_sqlCn != null)
               {
                    return true;
               }
               return false;
          }
          /// <summary>
          /// Parse a parameter string array to create a connection string.
          /// Parameters: server, database, isIntegratedSecurity, login, password.
          /// </summary>
          /// <param name="settings"></param>
          /// <returns></returns>
          public override string ParseSettings(params string[] settings)
          {
               //TODO: validate the incoming parameter length
               if (settings.GetLength(0) < 5)
                    return string.Empty;

               StringBuilder _settings = new StringBuilder();
               _settings.Append("data source=" + settings[0] + ";");
               _settings.Append("initial catalog=" + settings[1] + ";");
               if (bool.Parse(settings[2]))
                    _settings.Append("integrated security=yes");
               else
                    _settings.Append("user id=" + settings[3] + ";password=" + settings[4]);
               return _settings.ToString();
          }


          public void Dispose()
          {
              Dispose(true);
              GC.SuppressFinalize(this);
          }
          protected virtual void Dispose(bool disposing)
          {
              if (disposing)
              {
                  if (_sqlCn != null)
                  {
                      _sqlCn.Dispose();
                      _sqlCn = null;
                  }
              }
          }
     }
}
