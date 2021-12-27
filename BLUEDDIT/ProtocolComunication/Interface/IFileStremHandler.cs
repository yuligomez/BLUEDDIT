using System;
using System.Threading.Tasks;

namespace ProtocolComunication.Interface
{
    public interface IFileStremHandler
    {
   
        Task <byte[]> ReadSegmentFileAsync(string path, long offSet, int length);

        Task WriteSegmentFileAsync(string path, byte[] dataBuffer);
    }
}
