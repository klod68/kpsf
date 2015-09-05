using System;
using System.Collections.Generic;
using System.Text;

namespace DALBuilder.DomainLayer
{
     public abstract class ConcurrencySupportCodeString
     {
          protected Table _table;
          protected string _codeString;

          protected ConcurrencySupportCodeString() { }

          public void CodeStringFromTable(Table table)
          {
               _table = table;

               if (_table.Database.ConcurrencySupport == ConcurrencySupportEnum.None)
                    return;
               //Template method, let's child implement details
               GenerateCodeString();
          }
          /// <summary>
          /// Generate the appropiate concurrency support code
          /// to be implemented by derived.
          /// </summary>
          protected abstract void GenerateCodeString();

          /// <summary>
          /// To set/get the code string
          /// </summary>
          public string Text
          {
               set { _codeString = value; }
               get { return _codeString;  }
          }
     }
}
