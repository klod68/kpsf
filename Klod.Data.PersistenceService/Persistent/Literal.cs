using System;
using System.Collections.Generic;
using System.Text;

namespace Klod.Data.PersistenceService.Persistent
{
	//TODO: complete conversion methods
	/// <summary>
	/// Represents a literal value for o/r mapping purpose.
	/// </summary>
	public class Literal : IConvertible
	{
		private object _value;

		public object Value
		{
			set { _value = value; }
			get { return _value; }
		}

		public Literal() { }
		public Literal(object value) { _value = value; }
		public Literal(int intValue) { _value = intValue; }
		public Literal(short shortValue) { _value = shortValue; }
		public Literal(byte byteValue) { _value = byteValue; }
		public Literal(decimal decValue) { _value = decValue; }
		public Literal(string stringValue) { _value = stringValue; }
		public Literal(Guid guidValue) { _value = guidValue; }
		public Literal(DateTime dateTimeValue) { _value = dateTimeValue; }

		#region IConvertible Members

		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}

		public bool ToBoolean(IFormatProvider provider)
		{
			bool b = false;
			if (bool.TryParse(_value.ToString(), out b))
				return b;
			return b;
		}

		public byte ToByte(IFormatProvider provider)
		{
			Byte b = 0;
			if (Byte.TryParse(_value.ToString(), out b))
				return b;
			return b;
		}

		public char ToChar(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public DateTime ToDateTime(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public decimal ToDecimal(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public double ToDouble(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public short ToInt16(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int ToInt32(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public long ToInt64(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public sbyte ToSByte(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public float ToSingle(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string ToString(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public object ToType(Type conversionType, IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public ushort ToUInt16(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public uint ToUInt32(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public ulong ToUInt64(IFormatProvider provider)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
