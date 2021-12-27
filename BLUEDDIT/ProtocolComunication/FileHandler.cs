using System;
using System.IO;
using ProtocolComunication.Interface;

namespace ProtocolComunication
{
    public class FileHandler : IFileHandler
    {
        public bool FileExists (string path)
        {
            return File.Exists(path);
        }
        
        public string GetFileName (string path)
        {
            if (FileExists(path))
            {
                return new FileInfo(path).Name;
            }
            throw new Exception("No existe ese archivo en la ruta espcificada");
        }

        public long GetFileSize (string path )
        {
            if (FileExists(path))
            {
                return new FileInfo(path).Length;
            }
            throw new Exception("No existe ese archivo en la ruta espcificada");
        }

        public long GetFileParts(long filesize)
        {
            var parts = filesize / ProtocolConstants.MaxPacketSize;
            return parts * ProtocolConstants.MaxPacketSize == filesize ? parts : parts + 1;
        }
    }
}