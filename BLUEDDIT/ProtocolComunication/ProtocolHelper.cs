using System;
using System.Text;

namespace ProtocolComunication
{
    public class ProtocolHelper
    {
        public static int GetLength ()
        {
            return ProtocolConstants.FileNameLenght + ProtocolConstants.FileSizeLength;
        }

        public static byte [] CreateHeader (short command ,string fileName , long fileSize) 
        {
            var header = new byte [HeaderConstants.CommandLength + GetLength()];
            var fileNameData = BitConverter.GetBytes(Encoding.UTF8.GetBytes(fileName).Length);
            if (fileNameData.Length!= ProtocolConstants.FileNameLenght)
            {
               throw new Exception ("Error en la especificación");
            }
            byte[] fileSizeBytes = BitConverter.GetBytes(fileSize);

            //Copio filename hacia header y copio filesize hacia header 
            var commandByte = BitConverter.GetBytes(command);
            Array.Copy(commandByte, 0, header, 0, HeaderConstants.CommandLength);
            Array.Copy(fileNameData, 0,header, HeaderConstants.CommandLength, ProtocolConstants.FileNameLenght);
            Array.Copy(fileSizeBytes, 0,header,ProtocolConstants.FileNameLenght, ProtocolConstants.FileSizeLength);
            return header;
        }
    }
}