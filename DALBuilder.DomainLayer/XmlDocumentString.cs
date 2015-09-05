using System;
using System.Collections.Generic;
using System.Text;

namespace DALBuilder.DomainLayer
{
     /// <summary>
     /// Represents an XML document.
     /// </summary>
     public class XmlDocumentString
     {
       
          private StringBuilder _text=null;

          public XmlDocumentString(string rootName, string innerXML)
          {
               _text = new StringBuilder();
               _text.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n");
               _text.Append("<" + rootName + ">\n");
               _text.Append(innerXML);
               _text.Append("</" + rootName + ">");
          }

          public string Text
          {
               get { return _text.ToString(); }
          }

     }
}
