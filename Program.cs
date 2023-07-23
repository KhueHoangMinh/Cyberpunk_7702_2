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
    public sealed class Program
    {

        static void Main()
        {
            int WIDTH = 1920;
            int HEIGHT = 1080;
            long TIME_BETWEEN_UPDATES = 150000;
            long CURRENT_UPDATE_TICK = DateTime.UtcNow.Ticks;
            uint FPS = 120;

            Window window = new Window("Cyberpunk 7702 | 2", WIDTH, HEIGHT);

            Manager.Instance.Window = window;

            SplashKit.CurrentWindowToggleFullscreen();

            // Game loop

            do
            {

                while(CURRENT_UPDATE_TICK <= DateTime.UtcNow.Ticks)
                {
                    SplashKit.ProcessEvents();
                    Manager.Instance.Update();
                    CURRENT_UPDATE_TICK += TIME_BETWEEN_UPDATES;
                }

                SplashKit.ClearScreen(Color.Black);
                Manager.Instance.Draw();
                SplashKit.RefreshScreen(FPS);
            } while (
                !SplashKit.WindowCloseRequested(window) && !SplashKit.KeyDown(KeyCode.EscapeKey)
            );

        }
    }
}
