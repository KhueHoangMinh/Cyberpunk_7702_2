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
            long TIME_BETWEEN_UPDATES = 150000;
            long TIME_BETWEEN_FRAMES = 12000;
            long CURRENT_UPDATE_TICK = 0;
            long CURRENT_FRAME_TICK = 0;

            Window window = new Window("Cyberpunk 7702 | 2", WIDTH, HEIGHT);
            Manager manager = new Manager(window);

            string _textInput = SplashKit.TextInput(window);



            // Game loop

                do
                {
                    if (DateTime.UtcNow.Ticks - CURRENT_UPDATE_TICK >= TIME_BETWEEN_UPDATES)
                    {
                        CURRENT_UPDATE_TICK = DateTime.UtcNow.Ticks;
                        SplashKit.ProcessEvents();
                        manager.Update();
                    }
                    if (DateTime.UtcNow.Ticks - CURRENT_FRAME_TICK >= TIME_BETWEEN_FRAMES)
                    {
                        CURRENT_FRAME_TICK = DateTime.UtcNow.Ticks;
                        SplashKit.ClearScreen(Color.Black);
                        manager.Draw();
                        //SplashKit.DrawText(_textInput, Color.White, "font", 50, 100, 100);
                        SplashKit.RefreshScreen();
                    }
            } while (
                //false 
                !SplashKit.WindowCloseRequested(window) && !SplashKit.KeyDown(KeyCode.EscapeKey)
            );
        }
    }
}

