namespace Klod.Data.PersistenceService
{
    /// <summary>
    ///Base class to represent the set of instructions and details for a command object.
    /// </summary>
    public abstract class CommandMap
	{
		#region attributes (private variables)
		private string _name = string.Empty;
		private string _type = string.Empty;

        private int _parametersCount;

		private string[] _propertiesSet;
		private string[] _parametersSet;

		private string[] _classNamesSet;//to store objects info, when available, for transaction commands
		
        //crr 2012.05.15
		//REFACTORING: Classes can be used more than once in the same map. Use the token property for a unique identifier for them.
		private string[] _classTokensSet; //to store a unique name when two or more class of the same type are used in the map


		private string[] _parametersDataTypesSet;
		private string[] _parametersDirectionsSet;
		private string[] _parametersUsesSet;
		#endregion

		#region constructor
		protected CommandMap()
		{

		}
		#endregion

		#region properties
		public string Name
		{
			set { _name = value; }
			get { return _name; }
		}
		public string Type
		{
			set { _type = value; }
			get { return _type; }
		}

        public int ParametersCount
		{
			set { _parametersCount = value; }
			get { return _parametersCount; }
		}
		public string[] ParametersSet
		{
			set { _parametersSet = value; }
			get { return _parametersSet; }
		}
		public string[] PropertiesSet
		{
			set { _propertiesSet = value; }
			get { return _propertiesSet; }
		}
		public string[] ClassNamesSet
		{
			set { _classNamesSet = value; }
			get { return _classNamesSet; }
		}
		public string[] ClassTokensSet
		{
			set { _classTokensSet = value; }
			get { return _classTokensSet; }
		}
		public string[] ParametersDataTypesSet
		{
			set { _parametersDataTypesSet = value; }
			get { return _parametersDataTypesSet; }
		}
		public string[] ParametersDirectionsSet
		{
			set { _parametersDirectionsSet = value; }
			get { return _parametersDirectionsSet; }
		}
		public string[] ParametersUsesSet
		{
			set { _parametersUsesSet = value; }
			get { return _parametersUsesSet; }
		}
		#endregion

		/// <summary>
		/// Comment
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public virtual string GetProperty(int index)
		{
			try
			{
				return _propertiesSet[index];
			}
			catch
			{
				return string.Empty;
			}
		}
		public virtual string GetParameter(int index)
		{
			return _parametersSet[index];
		}

		#region methods for parameter attributes
		public virtual string GetParameterDataType(int index)
		{
			return _parametersDataTypesSet[index];
		}
		//Return an enumeration? No. Let the implementations to decide which value to use.
		public virtual string GetParameterDirection(int index)
		{
			return _parametersDirectionsSet[index];
		}
		//Use an enumeration? No. Let the implementations to decide.
		public virtual string GetParameterUse(int index)
		{
			return _parametersUsesSet[index];
		}

		/// <summary>
		/// The data type of the identifier for the persistentObject
		/// </summary>
		/// <returns></returns>
		public abstract string GetIDDataType();

        /// <summary>
        /// Get the provider's specific command type
        /// </summary>
        /// <returns></returns>
        public abstract string GetProviderCommandType();


		//The class name of the type that will be receive values
		//from a multiple object retrive operation (transactions,...)
		#endregion

		#region methods for get retrieved table or resultsets metadata
		public abstract string GetClassName(int index);

		//2012.05.15 crr
		//REFACTORING: token property for OID classes in the map.
		public abstract string GetClassToken(int index);

		//CHECK: What the hell is that method?
		public abstract string GetRetrieveColumn(int index);
		public abstract int GetRetrievedColumnsCount();
		public abstract string GetRetrieveProperty(int index);
		public abstract string GetReturnTableName();
		public abstract bool IsRetrievedColumObjectIdentifier(int index);
		#endregion
	}

}
