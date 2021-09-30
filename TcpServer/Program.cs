using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Opgave5
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("This is the server");
            TcpListener listener = new TcpListener(IPAddress.Loopback, 2121);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Server ready");
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("Incoming client");
                DoClient(socket);
            }
        }

        private static void DoClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            string message = reader.ReadLine();
            Console.WriteLine("Server received: " + message);
            writer.Write(message);
            writer.Flush();
            socket.Close();
        }

        private static IPAddress GetIpAddressOfThisComputer()
            // https://www.delftstack.com/howto/csharp/get-local-ip-address-in-csharp/
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipEntry.AddressList[0];
            Console.WriteLine("Your IP address: " + ipAddress);
            return ipAddress;
        }
    }
}