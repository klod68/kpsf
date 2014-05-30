using System;
using System.Collections.Generic;
using System.Text;

using Klod.Data.PersistenceService.Settings;

namespace Klod.Data.PersistenceService.Exceptions
{
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
