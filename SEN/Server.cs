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

        public Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 1337);
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
                try
                {
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    this.clientList.Add(client);
                }
                catch (Exception ex)
                {
                    // closed 
                }
            }
        }

        public string getIP()
        {
            return ((IPEndPoint)this.tcpListener.LocalEndpoint).Address.ToString();
        }

        public void sendString(string message)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(message);

            foreach (TcpClient client in this.clientList)
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

        public void close()
        {
            // TODO Close Server and stop all connections.
            // Test with http://sockettest.sourceforge.net/
            this.tcpListener.Stop();
            foreach (TcpClient client in this.clientList)
            {
                client.GetStream().Close();
            }

            this.clientList.Clear();
        }
    }
}
