using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;


namespace SEN
{
    class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private List<TcpClient> clientList;

        public Server(string ip, int port)
        {
            this.tcpListener = new TcpListener(IPAddress.Any, port);
            this.clientList = new List<TcpClient>();
            try
            {
                this.listenThread = new Thread(new ThreadStart(ListenForClients));
                this.listenThread.Start();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                this.tcpListener.Stop();
            }
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while(true)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                this.clientList.Add(client);
            }
        }

        public void sendString(string message)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(message);

            foreach (TcpClient client in clientList)
            {
                NetworkStream clientStream = client.GetStream();
                try
                {
                    clientStream.Write(buffer, 0, buffer.Length);
                    clientStream.Flush();
                }
                catch(Exception e)
                {
                    clientStream.Close();
                    this.clientList.Remove(client);
                }
            }
            
        }
    }
}
