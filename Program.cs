
// server code ===================================================================================================


using System;
using System.ComponentModel;
using System.Dynamic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using SplashKitSDK;

namespace Cyberpunk77022
{
    public class Program
    {

        public static void StartServer()
        {
            // Get Host IP Address that is used to establish a connection
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses
            IPHostEntry host = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAddress = IPAddress.Parse("192.168.1.12");
            Console.WriteLine(ipAddress.ToString());
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        }

        public static void Listen(Socket handler)
        {
            try
            {


                // Incoming data from the client.
                string data = null;
                byte[] bytes = null;

                //while (true)
                //{
                //    bytes = new byte[1024];
                //    int bytesRec = handler.Receive(bytes);
                //    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                //    if (data.IndexOf("<EOF>") > -1)
                //    {
                //        break;
                //    }
                //}

                //bytes = new byte[1024];
                //int bytesRec = handler.Receive(bytes);
                //data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                //Console.WriteLine("Ticks received : {0}", data);

                byte[] msg = Encoding.ASCII.GetBytes("Current server Ticks: " + DateTime.UtcNow.Ticks.ToString());
                handler.Send(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // Client app is the one sending messages to a Server/listener.
        // Both listener and client can send messages back and forth once a
        // communication is established.

        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses
                IPHostEntry host = Dns.GetHostEntry("127.0.0.1");
                IPAddress ipAddress = host.AddressList[0];
                Console.WriteLine(ipAddress.ToString());
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    // Connect to Remote EndPoint
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        static void Main()
        {
            int WIDTH = 1920;
            int HEIGHT = 1080;
            long TIME_BETWEEN_UPDATES = 120000;
            long TIME_BETWEEN_FRAMES = 0;
            long CURRENT_UPDATE_TICK = 0;
            long CURRENT_FRAME_TICK = 0;

            Window window = new Window("Cyberpunk 7702 | 2", WIDTH, HEIGHT);
            Manager manager = new Manager(window);

            string _textInput = SplashKit.TextInput(window);

            //SplashKit.CurrentWindowToggleFullscreen();

            // Get device ip address

            //var host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (var ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        Console.WriteLine(ip.ToString());
            //    }
            //}


            // Test

            //IPHostEntry host = Dns.GetHostEntry("127.0.0.1");
            //IPAddress ipAddress = IPAddress.Parse("192.168.1.12");
            //Console.WriteLine(ipAddress.ToString());
            //IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);


            //// Create a Socket that will use Tcp protocol
            //Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Udp);
            //// A Socket must be associated with an endpoint using the Bind method
            //listener.Bind(localEndPoint);
            //// Specify how many requests a Socket can listen before it gives Server busy response.
            //// We will listen 10 requests at a time
            //listener.Listen(10);

            //Console.WriteLine("Waiting for a connection...");
            //Socket handler = listener.Accept();





            new Thread(() =>
            {
                const int listenPort = 11000;

                UdpClient listener = new UdpClient(listenPort);
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

                try
                {
                    while (true)
                    {
                        Console.WriteLine("Waiting for broadcast");
                        byte[] bytes = listener.Receive(ref groupEP);

                        Console.WriteLine($"Received broadcast from {groupEP} :");

                        string msg = $" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}";
                        Console.WriteLine(msg);

                        if(msg == "Current ticks: " + DateTime.UtcNow.Ticks.ToString())
                        {
                            Console.WriteLine("EQ");

                        } else
                        {
                            Console.WriteLine(msg);
                            Console.WriteLine("Current ticks: " + DateTime.UtcNow.Ticks.ToString());
                        }
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    listener.Close();
                }

            }).Start();


            new Thread(() =>
            {

                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress broadcast = IPAddress.Parse("192.168.1.255");

                IPEndPoint ep = new IPEndPoint(broadcast, 11000);
                while (true)
                {
                    byte[] sendbuf = Encoding.ASCII.GetBytes("Current ticks: " + DateTime.UtcNow.Ticks.ToString());
                    s.SendTo(sendbuf, ep);
                    Thread.Sleep((int)TIME_BETWEEN_UPDATES / 10000);
                }

            }).Start();

            // Game loop

            do
            {
                if (DateTime.UtcNow.Ticks - CURRENT_UPDATE_TICK >= TIME_BETWEEN_UPDATES)
                {
                    CURRENT_UPDATE_TICK = DateTime.UtcNow.Ticks;
                }

                manager.Update();
                SplashKit.ProcessEvents();
                //if(SplashKit.MouseClicked(MouseButton.LeftButton))
                //{
                //    SplashKit.StartReadingText(new Rectangle() { X = 100, Y = 100, Width = 100, Height = 100 });
                //}
                //if(SplashKit.ReadingText())
                //{
                //    _textInput = SplashKit.TextInput(window);
                //}

                //Listen(handler);

                if (DateTime.UtcNow.Ticks - CURRENT_FRAME_TICK >= TIME_BETWEEN_FRAMES)
                {
                    CURRENT_FRAME_TICK = DateTime.UtcNow.Ticks;
                    SplashKit.ClearScreen(Color.Black);
                    manager.Draw();
                    //SplashKit.DrawText(_textInput, Color.White, "font", 50, 100, 100);
                    SplashKit.RefreshScreen();
                }
                SplashKit.Delay((uint)TIME_BETWEEN_UPDATES / 10000);
            } while (
                //false 
                !SplashKit.WindowCloseRequested(window) && !SplashKit.KeyDown(KeyCode.EscapeKey)
            );
            //handler.Shutdown(SocketShutdown.Both);
            //handler.Close();
        }
    }
}




/// client code ===================================================================================================

//using System;
//using System.ComponentModel;
//using System.Dynamic;
//using System.Net;
//using System.Net.Sockets;
//using System.Reflection;
//using System.Text;
//using System.Threading;
//using System.Windows;
//using SplashKitSDK;

//namespace Cyberpunk77022
//{
//    public class Program
//    {

//        public static void StartServer()
//        {
//            // Get Host IP Address that is used to establish a connection
//            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
//            // If a host has multiple addresses, you will get a list of addresses
//            IPHostEntry host = Dns.GetHostEntry("localhost");
//            IPAddress ipAddress = host.AddressList[0];
//            Console.WriteLine(ipAddress.ToString());
//            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

//            try
//            {

//                // Create a Socket that will use Tcp protocol
//                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
//                // A Socket must be associated with an endpoint using the Bind method
//                listener.Bind(localEndPoint);
//                // Specify how many requests a Socket can listen before it gives Server busy response.
//                // We will listen 10 requests at a time
//                listener.Listen(10);

//                Console.WriteLine("Waiting for a connection...");
//                Socket handler = listener.Accept();

//                // Incoming data from the client.
//                string data = null;
//                byte[] bytes = null;

//                while (true)
//                {
//                    bytes = new byte[1024];
//                    int bytesRec = handler.Receive(bytes);
//                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
//                    if (data.IndexOf("<EOF>") > -1)
//                    {
//                        break;
//                    }
//                }

//                Console.WriteLine("Text received : {0}", data);

//                byte[] msg = Encoding.ASCII.GetBytes(data);
//                handler.Send(msg);
//                handler.Shutdown(SocketShutdown.Both);
//                handler.Close();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }

//            Console.WriteLine("\n Press any key to continue...");
//            Console.ReadKey();
//        }

//        // Client app is the one sending messages to a Server/listener.
//        // Both listener and client can send messages back and forth once a
//        // communication is established.

//        public static void StartClient()
//        {

//            try
//            {
//                // Connect to a Remote server
//                // Get Host IP Address that is used to establish a connection
//                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
//                // If a host has multiple addresses, you will get a list of addresses
//                IPHostEntry host = Dns.GetHostEntry("127.0.0.1");
//                IPAddress ipAddress = IPAddress.Parse("192.168.1.12");
//                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

//                // Create a TCP/IP  socket.
//                Socket sender = new Socket(ipAddress.AddressFamily,
//                SocketType.Stream, ProtocolType.Tcp);

//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }

//        public static void SendMsg(Socket sender, string data)
//        {

//            byte[] bytes = new byte[1024];
//            // Connect the socket to the remote endpoint. Catch any errors.
//            try
//            {


//                // Encode the data string into a byte array.
//                byte[] msg = Encoding.ASCII.GetBytes("Ticks: " + data);

//                //// Send the data through the socket.
//                //int bytesSent = sender.Send(msg);

//                //Receive the response from the remote device.
//                int bytesRec = sender.Receive(bytes);
//                Console.WriteLine("Echoed test = {0}",
//                    Encoding.ASCII.GetString(bytes, 0, bytesRec));


//            }
//            catch (ArgumentNullException ane)
//            {
//                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
//            }
//            catch (SocketException se)
//            {
//                Console.WriteLine("SocketException : {0}", se.ToString());
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Unexpected exception : {0}", e.ToString());
//            }
//        }


//        static void Main()
//        {
//            int WIDTH = 1920;
//            int HEIGHT = 1080;
//            long TIME_BETWEEN_UPDATES = 120000;
//            long TIME_BETWEEN_FRAMES = 0;
//            long CURRENT_UPDATE_TICK = 0;
//            long CURRENT_FRAME_TICK = 0;

//            Window window = new Window("Cyberpunk 7702 | 2", WIDTH, HEIGHT);
//            Manager manager = new Manager(window);

//            string _textInput = SplashKit.TextInput(window);

//            //SplashKit.CurrentWindowToggleFullscreen();

//            //var host = Dns.GetHostEntry(Dns.GetHostName());
//            //foreach (var ip in host.AddressList)
//            //{
//            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
//            //    {
//            //        Console.WriteLine(ip.ToString());
//            //    }
//            //}


//            // Test

//            // Connect to a Remote server
//            // Get Host IP Address that is used to establish a connection
//            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
//            // If a host has multiple addresses, you will get a list of addresses
//            //IPHostEntry host = Dns.GetHostEntry("127.0.0.1");
//            //IPAddress ipAddress = IPAddress.Parse("192.168.1.12");
//            //IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

//            //// Create a TCP/IP  socket.
//            //Socket sender = new Socket(ipAddress.AddressFamily,
//            //SocketType.Stream, ProtocolType.Udp);


//            //// Connect to Remote EndPoint
//            //sender.Connect(remoteEP);

//            //Console.WriteLine("Socket connected to {0}",
//            //    sender.RemoteEndPoint.ToString());




//            new Thread(() =>
//            {

//                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

//                IPAddress broadcast = IPAddress.Parse("192.168.1.255");

//                IPEndPoint ep = new IPEndPoint(broadcast, 11000);
//                while (true)
//                {
//                    byte[] sendbuf = Encoding.ASCII.GetBytes("Current ticks: " + DateTime.UtcNow.Ticks.ToString());
//                    s.SendTo(sendbuf, ep);
//                    Thread.Sleep((int)TIME_BETWEEN_UPDATES / 10000);
//                }

//            }).Start();

//            // Game loop

//            do
//            {
//                CURRENT_UPDATE_TICK = DateTime.UtcNow.Ticks;




//                manager.Update();

//                //if(SplashKit.MouseClicked(MouseButton.LeftButton))
//                //{
//                //    SplashKit.StartReadingText(new Rectangle() { X = 100, Y = 100, Width = 100, Height = 100 });
//                //}
//                //if(SplashKit.ReadingText())
//                //{
//                //    _textInput = SplashKit.TextInput(window);
//                //}

//                SplashKit.ProcessEvents();
//                //SendMsg(sender, DateTime.UtcNow.Ticks.ToString());

//                Console.WriteLine("Message sent to the broadcast address");

//                if (DateTime.UtcNow.Ticks - CURRENT_FRAME_TICK >= TIME_BETWEEN_FRAMES)
//                {
//                    CURRENT_FRAME_TICK = DateTime.UtcNow.Ticks;
//                    SplashKit.ClearScreen(Color.Black);
//                    manager.Draw();
//                    //SplashKit.DrawText(_textInput, Color.White, "font", 50, 100, 100);
//                    SplashKit.RefreshScreen();
//                }
//                SplashKit.Delay((uint)TIME_BETWEEN_UPDATES / 10000);
//            } while (
//                //false 
//                !SplashKit.WindowCloseRequested(window) && !SplashKit.KeyDown(KeyCode.EscapeKey)
//            );

//            // Release the socket.
//            //sender.Shutdown(SocketShutdown.Both);
//            //sender.Close();

//        }
//    }
//}
