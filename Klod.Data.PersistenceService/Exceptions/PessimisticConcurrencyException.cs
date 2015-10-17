using System;
using System.Collections.Generic;
using System.Text;
using Klod.Data.PersistenceService.Settings;

namespace Klod.Data.PersistenceService.Exceptions
{
    [Serializable]
	public class PessimisticConcurrencyException : Exception
	{
		private string _message = PersistenceServiceConfiguration.Default.PessimisticConcurrencyErrorMessage;
		public override string Message
		{
			get {return _message;}
		}
		public PessimisticConcurrencyException()
		{

		}
		public PessimisticConcurrencyException(string message)
			: base(message)
		{
			_message = message;
		}
		public PessimisticConcurrencyException(string message, Exception innerException)
			: base(message, innerException)
		{
			_message = message;
		}

	}
}
