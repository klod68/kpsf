using System;
using System.Collections.Generic;
using System.Text;

namespace DALBuilder.DomainLayer.SqlServer
{
	 public class SqlServerTable:Table
	 {
		  public SqlServerTable():base() { }

		  public override ConcurrencySupportCodeString MakeConcurrencySupportCodeString()
		  {
			  SqlServerConcurrencySupportCodeString _sscscs = new SqlServerConcurrencySupportCodeString();
			  _sscscs.CodeStringFromTable(this);
			  return _sscscs;
		  }
		  public override StoreProcedureStream MakeStoreProcedureStream()
		  {
			  SqlServerStoreProcedureStream _sssps = new SqlServerStoreProcedureStream();
			  _sssps.StoreFromTable(this);
			  return _sssps;
		  }
		
	 }
}
