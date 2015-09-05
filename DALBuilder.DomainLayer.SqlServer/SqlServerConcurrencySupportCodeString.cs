using System;
using System.Collections.Generic;
using System.Text;


namespace DALBuilder.DomainLayer.SqlServer
{
	 public class SqlServerConcurrencySupportCodeString:ConcurrencySupportCodeString
	 {
		  private StringBuilder _code = null;
		  public SqlServerConcurrencySupportCodeString() : base() { }
		 // public SqlServerConcurrencySupportCodeString(Table table) : base(table) { }

		  protected override void GenerateCodeString()
		  {
			   _code = new StringBuilder();
			   _code.Append("USE " + _table.Database.Name + "\n\n");
			   _code.Append("GO\n\n");
			   _code.Append("ALTER TABLE " + _table.FullName + "\n");

			if (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.Optimistic)
			   {
					AddVersionColumnCode();
			   }
			else if ((_table.Database.ConcurrencySupport == ConcurrencySupportEnum.PessimisticUserId) || (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.PessimisticUserName))
			   {
					AddInUseColumnCode();
			   }
			   AddErrorMessage(true);
			   AddErrorMessage(false);

			   _code.Append("\nGO\n\n");
			   _codeString = _code.ToString();

		  }

		  private string AddVersionColumnCode()
		  {
			   if (_code == null)
					return string.Empty;

			   _code.Append("\tADD [" +  AppSettings.Default.OptimisticConcurrencySupportColumn + "] timestamp\n");
			   return _code.ToString();
		  }
		  private string AddInUseColumnCode()
		  {
			   if (_code == null)
					return string.Empty;

			string type = (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.PessimisticUserId) ? "uniqueidentifier" : "varchar(50)";
			   _code.Append("\tADD [" + AppSettings.Default.PessimisticConcurrencySupportColum + "] " + type + " NULL\n");
			   return _code.ToString();
		  }

		private string AddErrorMessage(bool optimistic)
		{
			if (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.Optimistic)
			{
				_code.Append("GO\nUSE master\nGO\n");
					if (optimistic)
					{
						 _code.Append("EXEC sys.addmessage @msgnum=50001,@severity=16,");
						 _code.Append("@msgtext='The current record has been modified by another process.'");
					}
					else
					{
						 _code.Append("EXEC sys.addmessage @msgnum=50002,@severity=16,");
						 _code.Append("@msgtext='The current record has been locked by another user.'");
					}
			}
			return _code.ToString();
		}
	 }
}
