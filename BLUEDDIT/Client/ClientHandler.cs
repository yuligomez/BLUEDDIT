using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;

namespace Client
{
    public class ClientHandler
    {

        public TcpClient TcpClient { get; set; }
        private ClientSetting clientSetting { get; set; }

   


        public ClientHandler()
        {
            string json = "";
            string path = Directory.GetCurrentDirectory() + "../../../../appsettings.json";
            json = System.IO.File.ReadAllText(path);
            var clientSetting = JsonConvert.DeserializeObject<ClientSetting>(json);
            this.clientSetting = clientSetting;
            TcpClient = new TcpClient(new IPEndPoint(IPAddress.Parse(clientSetting.ClientIP), 0));
        }


        public async Task ConnectAsync()
        {
            await TcpClient.ConnectAsync(IPAddress.Parse(clientSetting.ServerIP), clientSetting.ServerPort);
           
        }
 
        public void Desconnect()
        {
            TcpClient.Close();
        } 
    }
}
