using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Domain;
using Newtonsoft.Json;

namespace Server_GPRC_MQ
{
    public class ServerHandler
    {
      
        public TcpListener TcpListener { get; set; }

        public TcpClient TcpClient { get; set; }

        public ServerHandler()
        {
            string path = Directory.GetCurrentDirectory() + "/appsetting.json";
            string json = System.IO.File.ReadAllText(path);
            var serverSettings = JsonConvert.DeserializeObject<ServerSetting>(json);
            TcpListener = new TcpListener(IPAddress.Parse(serverSettings.ServerIP), serverSettings.ServerPort);
            TcpListener.Start(1);
        }
    }
}
