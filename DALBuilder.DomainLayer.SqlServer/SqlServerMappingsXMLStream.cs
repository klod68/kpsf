using System;
using System.Collections.Generic;
using System.Text;

namespace DALBuilder.DomainLayer.SqlServer
{
	 public class SqlServerMappingsXMLStream:MappingsXMLStream
	 {
		  private StringBuilder _temp=null;

		  
		  protected override void GenerateMappingsStream()
		  {
			   
			   _temp = new StringBuilder();
			   //return a mappings xml fragment string
			   
			   //1. create a <relationalMap> for each table
			   SetBeginRelationalMap(); 
			   //2. create a <columns> element with children <column> elements
			   SetTableMap();
			   
			   //3. create a <commands> element with children <command> elements
			   SetCommandsMap();
			   //4. close the <mappings>
			   SetEndRelationalMap();
			  
			   _text= _temp.ToString();
		  }

		  private void SetBeginRelationalMap()
		  {
			   _temp.Append("\t<map type=\"relational\" class=\"\" storage=\"" + _sprocs.Table.FullName + "\" storageType=\"table\">\n");
		  }
		  private void SetTableMap()
		  {
			   _temp.Append("\t\t<table>\n");
			   foreach (Column col in _sprocs.Table.Columns)
			   {
					_temp.Append("\t\t\t<column name=\"" + col.Name + "\" property=\"\" ");
					if (col.IsPrimaryKey)
						 _temp.Append("primaryKey=\"true\"");
					_temp.Append("/>\n");
			   }
			   _temp.Append("\t\t</table>\n");
		  }
		  private void SetCommandsMap()
		  {
				_temp.Append("\t\t<commands>\n");

				_temp.Append("\t\t\t<command name=\"" + _sprocs.SelectAllCommand + "\" type=\"RetrieveAll\" providerType=\"\">\n");
				SetReturnedTable(true);
				_temp.Append("\t\t\t</command>\n");
				_temp.Append("\t\t\t<command name=\"" + _sprocs.SelectCommand + "\" type=\"Retrieve\" providerType=\"\">\n");
				SetPrimaryKeysParameters(true);
				SetReturnedTable(false);
				_temp.Append("\t\t\t</command>\n");
				//add attribute of idDataType
				_temp.Append("\t\t\t<command name=\"" + _sprocs.InsertCommand + "\" type=\"Create\" providerType=\"\" idDataType=\"");
			 if (_sprocs.Table.PrimaryKey != null)
				 _temp.Append(_sprocs.Table.PrimaryKey.DataType + "\">\n");
			 else if (_sprocs.Table.Identity != null)
				 _temp.Append(_sprocs.Table.Identity.DataType + "\">\n");
			 else _temp.Append("\">\n");
 
			SetInsertParameters();

				_temp.Append("\t\t\t</command>\n");
				_temp.Append("\t\t\t<command name=\"" + _sprocs.UpdateCommand + "\" type=\"Update\" providerType=\"\">\n");
				SetUpdateParameters();
				_temp.Append("\t\t\t</command>\n"); 
				_temp.Append("\t\t\t<command name=\"" + _sprocs.DeleteCommand + "\" type=\"Delete\" providerType=\"\">\n");
				SetPrimaryKeysParameters(false);
				_temp.Append("\t\t\t</command>\n");
				_temp.Append("\t\t</commands>\n");

		  }
		  private void SetEndRelationalMap()
		  {
			   _temp.Append("\t</map>\n");
		  }
		  private void SetPrimaryKeysParameters(bool insertCommand)
		  {
			   _temp.Append("\t\t\t\t<parameters>\n");
			   foreach (Column _col in _sprocs.Table.Columns)
			   {
					if (_col.IsPrimaryKey || _col.IsIdentity)
						 _temp.Append("\t\t\t\t\t<parameter name=\""
							  + _col.ParameterName + "\" property=\"\" type=\"" + _col.DataType + "\" direction=\"Input\"/>\n");
				
				if (!insertCommand 
					&& (_col.IsOptimisticConcurrencySupport || _col.IsPessimisticConcurrencySupport))
				{
					_temp.Append("\t\t\t\t\t<parameter name=\""
							+ _col.ParameterName + "\" property=\"\" type=\"" + _col.DataType + "\" direction=\"Input\"/>\n");
				}
			}
			   _temp.Append("\t\t\t\t</parameters>\n");
		  }
		  private void SetReturnedTable(bool includePk)
		  {
			   _temp.Append("\t\t\t\t<returnTable name=\"" + _sprocs.Table.Name + "\">\n");
			   foreach (Column _col in _sprocs.Table.Columns)
			   {
					if (_col.IsPrimaryKey && !includePk)
						 continue;

					_temp.Append("\t\t\t\t\t<column name=\"" + _col.Name + "\" property=\"\"/>\n");
			   }
			   _temp.Append("\t\t\t\t</returnTable>\n");
		  }
		  private void SetInsertParameters()
		  {
			   _temp.Append("\t\t\t\t<parameters>\n");
			   foreach (Column _col in _sprocs.Table.Columns)
			   {
					if (!_col.IsIdentity && !_col.IsComputed && !_col.IsRowGuid && !_col.IsRowversion)
					{
					_temp.Append("\t\t\t\t\t<parameter name=\""
								   + _col.ParameterName + "\" property=\"\" type=\"" + _col.DataType + "\" direction=\"Input\"/>\n");
						 continue;
					}
					if (_col.IsIdentity)
					{
						 _temp.Append("\t\t\t\t\t<parameter name=\"@RETURN_VALUE" +
							   "\" property=\"\" type=\"" + _col.DataType + "\" direction=\"ReturnValue\"/>\n");
					}
			   }
			   _temp.Append("\t\t\t\t</parameters>\n");
		  }
		  private void SetUpdateParameters()
		  {
			   _temp.Append("\t\t\t\t<parameters>\n");
			   foreach (Column _col in _sprocs.Table.Columns)
			   {
					if (!_col.IsIdentity && !_col.IsComputed && !_col.IsRowversion && !_col.IsRowGuid || _col.IsPrimaryKey)
					{
						 _temp.Append("\t\t\t\t\t<parameter name=\""
							 + _col.ParameterName + "\" property=\"\" type=\"" + _col.DataType + "\" direction=\"Input\"/>\n");
						 continue;
					}
				if (_col.IsOptimisticConcurrencySupport && _col.IsRowversion)
				{
					_temp.Append("\t\t\t\t\t<parameter name=\""
							+ _col.ParameterName + "\" property=\"\" type=\"" + _col.DataType + "\" direction=\"Input\"/>\n");
				}

			   }
			   _temp.Append("\t\t\t\t</parameters>\n");
		  }
	 }
}
