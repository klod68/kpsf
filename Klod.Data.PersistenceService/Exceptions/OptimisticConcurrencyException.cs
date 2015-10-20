using System;

using Klod.Data.PersistenceService.Settings;

namespace Klod.Data.PersistenceService.Exceptions
{
    /// <summary>
    /// Represents a violation of the optimistic concurrency. Could be thow whenever a command tries
    /// to execute an operation when a record version or user isn't the original retrieved by clients.
    /// </summary>
    [Serializable]
    public class OptimisticConcurrencyException : Exception
	{
		private string _message = PersistenceServiceConfiguration.Default.OptimisticConcurrencyErrorMessage;

		public override string Message
		{
			get
			{
				return _message;
			}
		}
		public OptimisticConcurrencyException() : base() { }
		public OptimisticConcurrencyException(string message)
			: base(message)
		{
			_message = message;
		}
		public OptimisticConcurrencyException(string message, Exception innerException)
			: base(message, innerException)
		{
			_message = message;
		}
	}
}
