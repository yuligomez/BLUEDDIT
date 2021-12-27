using Domain;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ProtocolComunication.Interface
{
    public interface IFileExcutionHandler
    {
        Task<Response> RecivedFileForPostAsync(Tuple<short, int> header, TcpClient client);

        Task<Response> SendDataAsync(TcpClient client, string fileName, string postName);

        Task SendFileAsync(string postName, string username, string path, TcpClient tcpClient);
    }
}
