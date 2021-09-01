using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WorkerServiceTest2.SocketService
{
    internal class SocketServer
    {
        private Log log = new Log();
        internal async Task start()
        {
            // Establish the local endpoint 
            // for the socket. Dns.GetHostName
            // returns the name of the host 
            // running the application.
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            // håber at AddressList[0] = localhost
            IPAddress ipAddr = ipHost.AddressList[0];
            Console.WriteLine(ipAddr);
            IPEndPoint LocalEndPoint = new IPEndPoint(ipAddr, 11111);

            // Creation TCP/IP Socket 
            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Using Bind() method we associate a
            // network address to the Server Socket
            // All client that will connect to this 
            // Server Socket must know this network
            // Address
            listener.Bind(LocalEndPoint);

            // Using Listen() method we create 
            // the Client list that will want
            // to connect to Server.
            // Number of concurrent clients
            listener.Listen(10);

            while (true)
            {
                try
                {
                    // Suspend while waiting for
                    // incoming connection Using 
                    // Accept() method the server 
                    // will accept connection of client
                    Console.WriteLine("Service is ready to recieve");
                    log.write("Service is ready to recieve");
                    Socket serverSocket = listener.Accept();

                    // Data recieving data buffer
                    byte[] recievedBytes = new byte[1024];
                    string data;

                    while (true)
                    {
                        // Resetting the data string for new data.
                        data = null;

                        // Getting the amount of bytes and the data.
                        int numByte = serverSocket.Receive(recievedBytes);

                        // Making a string of the data
                        data += Encoding.ASCII.GetString(recievedBytes, 0, numByte);
                        Console.WriteLine("Recieved message: " + data);
                        log.write("Recieved message: " + data);

                        // Hvis <EOM> eksisterer i den modtagne besked.
                        if (!(data.IndexOf("<EOM>") > -1))
                        {
                            sendMessage(serverSocket, "No <EOM> present in message for ending it");
                        }
                        else
                        {
                            sendMessage(serverSocket, data);
                        }

                        // If datavalidation fails, exit.
                        // Close client Socket. After closing,
                        // we can use the closed Socket
                        // for a new Client Connection
                        // serverSocket.Shutdown(SocketShutdown.Both);
                        // serverSocket.Close();

                        // <EOC> End Of Communication
                        if (data.IndexOf("<EOC>") > -1)
                        {
                            Console.WriteLine("Communication is ended with client. Wait for new connection.");
                            log.write("Communication is ended with client. Wait for new connection.");
                            // Disconnect and reuse the socket.
                            serverSocket.Disconnect(true);
                            serverSocket = listener.Accept();
                            //break;
                        }

                    }
                }
                catch (Exception se)
                {
                    Console.WriteLine("Client disconnected without sending <EOC>");
                    log.write("Client disconnected without sending <EOC>");
                }
                await Task.Delay(TimeSpan.FromMilliseconds(1));
            }
        }
        internal void sendMessage(Socket serverSocket, string messageString)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(messageString);
            serverSocket.SendBufferSize = messageBytes.Length;
            serverSocket.Send(messageBytes);
        }
    }
}
