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

            // Listener

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

            // Sender

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
        }
    }
}

