using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ConsoleApplication4
{
    class Program
    {
        static TcpListener tcpListener = new TcpListener(8100);
        static void Main(string[] args)
        {
            tcpListener.Start();
            Console.WriteLine("************This is Server program************");
            Console.WriteLine("HoW many clients are going to connect to this server?:");
            int numberOfClientsYouNeedToConnect = int.Parse(Console.ReadLine());
            for (int i = 0; i < numberOfClientsYouNeedToConnect; i++)
            {
                Thread newThread = new Thread(new ThreadStart(here));
                newThread.Start();
            }

        }

        static void here()
        {

            {
                try
                {
                    bool status = true;
                    string servermessage = "";
                    string clientmessage = "";
                    tcpListener.Start();
                    Console.WriteLine("Server Started");
                    Socket socketForClient = tcpListener.AcceptSocket();
                    Console.WriteLine("Client Connected");
                    NetworkStream networkStream = new NetworkStream(socketForClient);
                    StreamWriter streamwriter = new StreamWriter(networkStream);
                    StreamReader streamreader = new StreamReader(networkStream);

                    while (status)
                    {
                        if (socketForClient.Connected)
                        {
                            servermessage = streamreader.ReadLine();

                            try
                            {

                                Console.WriteLine("Client:" + servermessage);

                                if ((servermessage == "bye"))
                                {
                                    try
                                    {
                                        status = false;
                                        streamreader.Close();
                                        networkStream.Close();
                                        streamwriter.Close();
                                        return;
                                    }

                                    catch (SocketException)
                                    {

                                        Console.WriteLine("Client EXIT RECEIVED");
                                        status = false;
                                        streamreader.Close();
                                        networkStream.Close();
                                        streamwriter.Close();
                                        return;

                                    }

                                }

                            }

                            catch(SocketException) { Console.WriteLine("Client EXIT RECEIVED"); }


                            Console.Write("Server:");
                            //Console.WriteLine(servermessage);

                            clientmessage = reverse(servermessage);
                            //Console.WriteLine(clientmessage);
                            streamwriter.WriteLine(clientmessage);
                            streamwriter.Flush();
                        }
                    }
                    streamreader.Close();
                    networkStream.Close();
                    streamwriter.Close();
                    socketForClient.Close();
                    Console.WriteLine("Exiting");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        static string reverse(string text)
        {
            //Console.WriteLine("alpha report");
            string input = text;
            char[] inputarray = input.ToCharArray();
            Array.Reverse(inputarray);
            string output = new string(inputarray);


            return output;

        }


    }
}
