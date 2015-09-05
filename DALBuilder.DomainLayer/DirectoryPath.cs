using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace DALBuilder.DomainLayer
{
    //TODO: Derive an interface from it?
     /// <summary>
     /// Represents a OS file system directory path
     /// </summary>
     public class DirectoryPath : IDirectoryPath
     {

		public DirectoryPath(){}
         /// <summary>
         /// Save concurrency support code in a script.
         /// </summary>
         /// <param name="codeStrings"></param>
         /// <param name="path"></param>
         /// <param name="filename"></param>
		public void SaveConcurrencySupportCode(ConcurrencySupportCodeString[] codeStrings, string path,string filename)
		{
			StringBuilder temp = new StringBuilder();
			foreach (ConcurrencySupportCodeString code in codeStrings)
			{
				temp.Append(code.Text);
			}

			SaveToFile(path, filename, temp.ToString());

		}
          
		/// <summary>
		/// Save CRUD stored procedures in a script.
		/// </summary>
		/// <param name="sprocs"></param>
		/// <param name="path"></param>
		/// <param name="filename"></param>
		public void SaveSProcsToFile(StoreProcedureStream[] sprocs,string path,string filename)
		{
			StringBuilder temp = new StringBuilder();
			foreach (StoreProcedureStream _stream in sprocs)
			{
				temp.Append(_stream.Text);
			}

			SaveToFile(path, filename, temp.ToString());

		}
          
		/// <summary>
		/// Save mapping O/R data in a XML file.
		/// </summary>
		/// <param name="sprocs"></param>
		/// <param name="path"></param>
		/// <param name="filename"></param>
		public void SaveMappingsToFile(StoreProcedureStream[] sprocs,string path,string filename)
		{
			//TODO: set header and footer xml doc
			StringBuilder temp = new StringBuilder();
			foreach (StoreProcedureStream _stream in sprocs)
			{
				temp.Append(_stream.Mappings.Text);
			}
			XmlDocumentString mappingsXML = new XmlDocumentString("mappings", temp.ToString());
			SaveToFile(path, filename, mappingsXML.Text);

		}

          private void SaveToFile(string path, string fileName, string data)
          {
               StreamWriter w = new StreamWriter(path + @"\" + fileName, false, Encoding.UTF8);
               w.Write(data);
               w.Flush();
               w.Close();
               w = null;

          }
     }
}
