using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SEN
{
    class Client
    {
        private TcpClient tcpServer;
        private NetworkStream serverStream;

        public Client()
        {
            this.tcpServer = new TcpClient();
        }

        public void Connect(string ip, int port)
        {
            this.tcpServer.Connect(ip, port);
            this.serverStream = tcpServer.GetStream();
        }

        public void Disconenct()
        {
            this.tcpServer.Close();
        }

        public void Read()
        {
            if (this.serverStream.CanRead)
            {
                byte[] message = new byte[this.tcpServer.ReceiveBufferSize];
                // is blocking
                this.serverStream.Read(message, 0, (int)this.tcpServer.ReceiveBufferSize);
                Console.WriteLine(Encoding.UTF8.GetString(message));
            }
            else
            {

                this.serverStream.Close();
            }
        }
    }
}
