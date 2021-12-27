using System;
namespace ProtocolComunication.Interface
{
    public interface IFileHandler
    {

        bool FileExists(string path);

        public string GetFileName(string path);

        long GetFileSize(string path);

        long GetFileParts(long filesize);
    }
}
