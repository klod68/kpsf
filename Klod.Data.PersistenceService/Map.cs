namespace Klod.Data.PersistenceService
{
    /// <summary>
    /// Abstract class that represent the mapping document with a set of command maps or instruction to materialize/dematerialize objects.
    /// </summary>
    public abstract class Map
	{
		//change return types to enumerations, interfaces and abstract class
		public abstract CommandMap GetCommandMap(string cmdType);
		public abstract string ClassName { get; }
		public abstract string Storage { get; }//the name of the persistent storage entity
		public abstract string StorageType { get; }// must be a enumeration -- {table,xml,flat file}
		public abstract string Type { get; } //must be an enumeration {relational,...}
	}
}
