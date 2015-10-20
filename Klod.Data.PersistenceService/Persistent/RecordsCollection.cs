using System.Collections.Generic;
using System.Data;

namespace Klod.Data.PersistenceService.Persistent
{
    /// <summary>
    /// Return object for multiple records.
    /// </summary>
    public class RecordCollection : DataView
	{
		private Dictionary<int, string> _columnNames;

		public Dictionary<int, string> ColumnNames
		{
			set { _columnNames = value; }
			get { return _columnNames; }
		}

		public RecordCollection()
			: base()
		{
			_columnNames = new Dictionary<int, string>();
		}
		public void Add(int index, string column)
		{
			_columnNames.Add(index, column);
		}
		public void Remove(int index)
		{
			_columnNames.Remove(index);
		}
		public bool ContainColumn(string column)
		{
			return _columnNames.ContainsValue(column);
		}
		public string GetColumnName(int index)
		{
			return _columnNames[index];
		}

	}
}
