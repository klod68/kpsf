using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	public abstract class SqlServerConcurrencySupport
	{
		private ConcurrencySupportType _concurrency;

		public SqlServerConcurrencySupport() 
		{
			_concurrency=ConcurrencySupportType.None;
		}
		public SqlServerConcurrencySupport(ConcurrencySupportType concurrency)
		{
			_concurrency = concurrency;
		}

		public PersistenceCommand MakeCommand()
		{
			return null;
		}
		


	}
}
