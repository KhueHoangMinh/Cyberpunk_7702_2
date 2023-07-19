using System;
using System.ComponentModel;
using System.Dynamic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
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
            long TIME_BETWEEN_FRAMES = 0;
            long CURRENT_UPDATE_TICK = DateTime.UtcNow.Ticks;
            long CURRENT_FRAME_TICK = DateTime.UtcNow.Ticks;

            Window window = new Window("Cyberpunk 7702 | 2", WIDTH, HEIGHT);
            Manager manager = new Manager(window);

            string _textInput = SplashKit.TextInput(window);

            SplashKit.CurrentWindowToggleFullscreen();

            // Game loop

            do
            {
                //if (DateTime.UtcNow.Ticks - CURRENT_UPDATE_TICK >= TIME_BETWEEN_UPDATES)
                //{

                //    SplashKit.ProcessEvents();
                //    manager.Update();

                //    CURRENT_UPDATE_TICK = DateTime.UtcNow.Ticks;
                //}

                while(CURRENT_UPDATE_TICK <= DateTime.UtcNow.Ticks)
                {
                    SplashKit.ProcessEvents();
                    manager.Update();
                    CURRENT_UPDATE_TICK += TIME_BETWEEN_UPDATES;
                }

                SplashKit.ClearScreen(Color.Black);
                manager.Draw();
                SplashKit.RefreshScreen(120);

                //if (DateTime.UtcNow.Ticks - CURRENT_FRAME_TICK >= TIME_BETWEEN_FRAMES)
                //{
                //    SplashKit.ClearScreen(Color.Black);
                //    manager.Draw();
                //    SplashKit.RefreshScreen();

                //    CURRENT_FRAME_TICK = DateTime.UtcNow.Ticks;
                //}

                //Thread.Sleep(1000 / 75);
            } while (
                !SplashKit.WindowCloseRequested(window) && !SplashKit.KeyDown(KeyCode.EscapeKey)
            );

        }
    }
}
