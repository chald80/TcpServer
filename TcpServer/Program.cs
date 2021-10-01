using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Opgave1;
using System.Linq;
using System.Runtime.CompilerServices;

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

        public static List<FootBallPlayer> FootBallPlayers = new List<FootBallPlayer>()
        {
            new FootBallPlayer(1, "Anders", 2000, 42)
        };



        
        private static void DoClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            while (true)
            {
                string message1 = reader.ReadLine();
                string message2 = reader.ReadLine();
                Console.WriteLine("Server received: " + message1);

                switch (message1)
                {
                    case "HentAlle":

                        foreach (FootBallPlayer FBP  in FootBallPlayers)
                        {
                            Console.WriteLine($"List Of Players {FBP.Id} {FBP.Name} {FBP.ShirtNumber} {FBP.Price}");
                            writer.WriteLine($"List Of Players {FBP.Id} {FBP.Name} {FBP.ShirtNumber} {FBP.Price}");
                        }
                        
                        writer.WriteLine("HentAlle");
                        writer.WriteLine(message1); 
                        writer.WriteLine(message2);
                        writer.Flush();
                        break;

                    case "Hent":

                        FootBallPlayer player = FootBallPlayers.FirstOrDefault(p => p.Id.ToString() == message2);
                        writer.WriteLine($"{player.Id} {player.Name} {player.ShirtNumber} {player.Price}");
                        writer.WriteLine("Hent");
                        writer.WriteLine(message1);
                        writer.WriteLine(message2);
                        writer.Flush();
                        break;


                    case "Gem":
                        FootBallPlayer FromJson = JsonSerializer.Deserialize<FootBallPlayer>(message2);
                        FootBallPlayers.Add(FromJson);
                        Console.WriteLine("FootBallPlayer" );

                        writer.WriteLine("gem");
                        writer.WriteLine(message1);
                        writer.WriteLine(message2);
                        writer.Flush();
                        break;

                    case "Close":

                        writer.WriteLine("closing Socket");
                        writer.Flush();
                        socket.Close();
                        break;
                
                }


            }
        }


        //private static IPAddress GetIpAddressOfThisComputer()
        //    // https://www.delftstack.com/howto/csharp/get-local-ip-address-in-csharp/
        //{
        //    IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
        //    IPAddress ipAddress = ipEntry.AddressList[0];
        //    Console.WriteLine("Your IP address: " + ipAddress);
        //    return ipAddress;
        //}
    }
}