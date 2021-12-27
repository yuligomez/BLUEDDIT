using System;
using System.IO;
using System.Threading.Tasks;
using ProtocolComunication.Interface;

namespace ProtocolComunication
{
    public class FileStremHandler : IFileStremHandler
    {


        
        //lee un segmento del archivo 
        public async Task <byte[]> ReadSegmentFileAsync(string path, long offSet , int length)
        {
            var buffer = new byte[length];
            var fs = new FileStream(path, FileMode.Open); //archivo en modo lectura
            fs.Position = offSet;
            var bytesRead = 0;
            while (bytesRead < length)
            {
                var read = await fs.ReadAsync(buffer, bytesRead, length - bytesRead);
                if (read == 0)
                {
                    throw new Exception("File cannot be read");
                }
                bytesRead += read;
            }
            return buffer;

        }

   
        public async Task WriteSegmentFileAsync(string path, byte[] dataBuffer)
        {
            FileHandler fh = new FileHandler();
            if (fh.FileExists(path))
            {
                var fs = new FileStream(path, FileMode.Append);
                await fs.WriteAsync(dataBuffer, 0, dataBuffer.Length);

            }
            else //sino existe el archivo lo creo 
            {
                var fs = new FileStream(path, FileMode.Create);
                await fs.WriteAsync(dataBuffer, 0, dataBuffer.Length);

            }
        }

    }
}