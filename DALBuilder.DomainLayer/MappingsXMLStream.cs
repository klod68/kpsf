using System;
using System.Collections.Generic;
using System.Text;

namespace DALBuilder.DomainLayer
{
     /// <summary>
     /// Represents an xml stream.
     /// </summary>
     public abstract class MappingsXMLStream
     {
          protected string _text;
          protected StoreProcedureStream _sprocs;
          
          public string Text { 
               set { _text = value; }
               get { return _text; }
          }
          public MappingsXMLStream() { }
          /// <summary>
          /// Implementation of the mapping stream using the stored procedures stream.
          /// Use a template method for generate the code by derivated classes.
          /// </summary>
          /// <param name="sprocs"></param>
          public void SetStreamFromProcedures(StoreProcedureStream sprocs)
          {
               _sprocs=sprocs;
                //template method
               GenerateMappingsStream();
          }
          /// <summary>
          /// Sets the Text property with a Mappings xml string.
          /// </summary>
          protected abstract void GenerateMappingsStream();

     }
}
