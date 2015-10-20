using System.Collections.Generic;

namespace Klod.Data.PersistenceService
{
    /// <summary>
    /// Composite class to store multiple PersistenCommands in a transaction.
    /// </summary>
    public abstract class CompositeCommand : PersistenceCommand
	{
		private Dictionary<int, PersistenceCommand> _cmds;

		public Dictionary<int, PersistenceCommand> Commands
		{
			set { _cmds = value; }
			get { return _cmds; }
		}

		public CompositeCommand()
			: base()
		{
			_cmds = new Dictionary<int, PersistenceCommand>();
		}
		public void Add(int index, PersistenceCommand cmd)
		{
			_cmds.Add(index, cmd);
		}
		public void Remove(int index)
		{
			_cmds.Remove(index);
		}
		public abstract void Sort();
		public override object Run()
		{
			try
			{
				for (int i = 0; i < _cmds.Count; i++)
				{
					_cmds[i].Run();
				}
				return true;
			}
			catch { return false; }
		}

	}
}
