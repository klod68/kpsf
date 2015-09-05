using System;
namespace DALBuilder.DomainLayer
{
    /// <summary>
    /// Interface representing a directory path/address.
    /// </summary>
    public interface IDirectoryPath
    {
        void SaveConcurrencySupportCode(ConcurrencySupportCodeString[] codeStrings, string path, string filename);
        void SaveMappingsToFile(StoreProcedureStream[] sprocs, string path, string filename);
        void SaveSProcsToFile(StoreProcedureStream[] sprocs, string path, string filename);
    }
}
