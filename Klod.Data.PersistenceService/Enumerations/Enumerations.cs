using System;
using System.Collections.Generic;
using System.Text;

namespace Klod.Data.PersistenceService
{
	//CHECK: These enumerations are the only types that the UI needs to access from this
	//namespace. Is this requirement a violation of the strict Layers Pattern?
	//Anyway the use of enumerations as parameters for calls of the UI
	//could be named the "Pass-enumerations-from-UI Pattern"
	//Without this provision, how a UI could send messages to the Application Layer (as a set of Controllers)
	//without the use of Domain Layer objects? The solution is proximate to the use
	//of the primitive types (integer, dates, text)...

	/// <summary>
	/// Indicate common CRUD operations: Create, Retrieve, Update, Delete and more.
	/// Used as a commands type declaration in map files.
	/// Are they better located in the application layer instead?
	/// </summary>
	public enum CRUD
	{
		Create,
		Retrieve,
		RetrieveAll,
		RetrieveAndLock,         //pessimistic
		RetrieveWithTimestamp,   //optimistic
		Update,
		UpdateCheckLock,         //pessimistic
		UpdateCheckTimestamp,     //optimistic
		Delete,
		DeleteCheckLock,         //pessimistic 
		DeleteCheckTimestamp,     //optimistic
		CompositeTransaction
	}
	/// <summary>
	/// Enumeration for establish the ID data type: Numeric, String or GUID.
	/// </summary>
	public enum IDDataType
	{
		NotSet,
		Number,
		String,
		Guid
	}
	/// <summary>
	/// Defines the semantic for an incoming parameter.
	/// </summary>
	public enum ParameterUse
	{
		NotSet,
		Regular, //in/out regular parameters
		RowVersion, //rowversion parameters
		UserName, //user name parameter
		UserId //user id parameter (UUID, GUID or SID)
	}

	/// <summary>
	/// Enumeration for the OID Types: Object, PersistentObject or Reference.
	/// </summary>
	public enum IDType
	{
		NotSet,
		Object,
		PersistentObject,
		Reference
	}
	/// <summary>
	/// Defines the persistent type and format.
	/// </summary>
	public enum StorageType
	{
		table,
		xmlFile,
		textFile,
		csvFile
	}
	/// <summary>
	/// Defines the concurrency options.
	/// </summary>
	public enum ConcurrencySupportType
	{
		None,
		Optimistic,
		PessimisticUniqueIdentifier,
		PessimisticUserName
	}
	/// <summary>
	/// Options for return types of a Command object.
	/// </summary>
	public enum CommandReturnType
	{
		Object,
		ObjectIdentifier,
		Bool,
		DataView
	}
}