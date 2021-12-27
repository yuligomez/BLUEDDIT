using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Domain
{
    public class TcpClientUsername
    {
        public TcpClient TcpClient { get; set; }
        public string Username { get; set; }
        public DateTime Date { get; set; }

    }
}
