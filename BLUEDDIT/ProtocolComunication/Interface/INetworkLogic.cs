using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ProtocolComunication.Interface
{
    public interface INetworkLogic
    {

        Task SendAsync(byte[] data, TcpClient client);

        Task CompleteSendAsync(string response, TcpClient client, short command);

        Task<byte[]> ReceiveDataAsync(int size, TcpClient client);

        Task<string> CompleteRecivedAsync(TcpClient client);

        int CountSpecialCharacter(string text);

        bool IsLetterWithDiacritics(char c);
    }
}
