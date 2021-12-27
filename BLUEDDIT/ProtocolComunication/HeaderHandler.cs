using System;
using System.Text;
using ProtocolComunication.Interface;

namespace ProtocolComunication
{
    public class HeaderHandler : IHeaderHandler
    {
        // YYY ZZZZ LARGO
        // REQ/RES CMD LARGO
        public HeaderHandler()
        {
            
        }

        public Tuple<short, int> DecodeHeader(byte[] data)
        {
            try
            {
                short command = BitConverter.ToInt16(data, 0);
                int dataLength = BitConverter.ToInt32(data, HeaderConstants.CommandLength);
                return new Tuple<short, int>(command, dataLength);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error de decodificacion de data: " + e.Message);
                return null;
            }
        }

        public byte[] EncodeHeader(short command, int dataLength)
        {
            var header = new byte[HeaderConstants.CommandLength + HeaderConstants.DataLength];
            //  copio la data de command , apartir de la posicion o de command
            // hacia header y de la posicon 0 de header
            //copio 2 bytes (CommandLength)
            Array.Copy(BitConverter.GetBytes(command), 0, header, 0, HeaderConstants.CommandLength);


             // copio el largo dataLength
            // desde la posicion 0
            // hacia header apartir del indiice commandlegth para no pisar lo que ya tengo
            // copio datalength bytes 
            Array.Copy(BitConverter.GetBytes(dataLength), 0, header, HeaderConstants.CommandLength, HeaderConstants.DataLength);
            return header;
        }
    }
}