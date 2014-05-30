using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Klod.Data.PersistenceService.XML;

namespace Klod.Data.PersistenceService.Relational.SqlServer
{
	//ok crr 2009.11.05
	/// <summary>
	/// Implementation for the dematerializer object
	/// </summary>
	internal class SqlServerDematerializer : Dematerializer
	{
		private static readonly SqlServerDematerializer _me = new SqlServerDematerializer();
		private SqlServerDematerializer() : base() { }
		static SqlServerDematerializer() { }

		public static SqlServerDematerializer GetInstance()
		{
			return _me;
		}
		protected override Map MakeMap(XmlNode mapNode)
		{
			return new XmlMap(mapNode);
		}
	}
}
