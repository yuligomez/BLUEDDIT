using System;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ProtocolComunication.Interface;

namespace ProtocolComunication
{
    public class NetworkLogic : INetworkLogic
    {
        private static NetworkStream networkStream;
        private static HeaderHandler headerHandler;



        public NetworkLogic()
        {
            headerHandler = new HeaderHandler();
        }

        public async Task SendAsync(byte[] data, TcpClient client)
        {
            networkStream = client.GetStream();
            //bytes que envio en una iteracion
            await networkStream.WriteAsync(data, 0, data.Length);
        }

        public async Task CompleteSendAsync(string response, TcpClient client, short command)
        {
            var specialCaracters = CountSpecialCharacter(response);
            var headerBytes = headerHandler.EncodeHeader(command, response.Length + specialCaracters);
            await SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(response);
            await SendAsync(dataByte, client);
        }

        public async Task<byte[]> ReceiveDataAsync(int size, TcpClient client)
        {
            int totalReceivedBytes = 0; //contador de cantidad  de bytes que envio en total
            var data = new byte[size];
            networkStream = client.GetStream();
            while (totalReceivedBytes < size)
            {
                int receivedBytes = await networkStream.ReadAsync(
                    data,
                    totalReceivedBytes,
                    size - totalReceivedBytes);
                if (receivedBytes == 0)
                {
                    throw new SocketException();
                }
                totalReceivedBytes += receivedBytes;
            }

            return data;
        }


        public async Task<string> CompleteRecivedAsync( TcpClient client)
        {
            var buffer = new byte[HeaderConstants.CommandLength + HeaderConstants.DataLength];
            var data = await ReceiveDataAsync(buffer.Length, client);
            Tuple<short, int> headerServer = headerHandler.DecodeHeader(data);
            var dataLength = headerServer.Item2;
            var serverData = new byte[dataLength];
            Array.Copy(await ReceiveDataAsync(dataLength, client), 0, serverData, 0, dataLength);
            var dataString = System.Text.Encoding.UTF8.GetString(serverData);
            return dataString;
        }

        public int CountSpecialCharacter(string text)
        {
            int count = 0;
            foreach (Char s in text)
            {
                if (s == 'ñ')
                {
                    count++;
                }
                else if (s == 'Ñ')
                {
                    count++;
                }
                else if (IsLetterWithDiacritics(s))
                {
                    count++;
                }
            }
            return count;
        }

        public bool IsLetterWithDiacritics(char c)
        {
            var s = c.ToString().Normalize(NormalizationForm.FormD);
            return (s.Length > 1) &&
                   char.IsLetter(s[0]) &&
                   s.Skip(1).All(c2 => CharUnicodeInfo.GetUnicodeCategory(c2) == UnicodeCategory.NonSpacingMark);
        }

    }

}
