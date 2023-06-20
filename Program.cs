using System;
using System.ComponentModel;
using System.Reflection;
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
            long TIME_BETWEEN_UPDATES = 100000;
            long TIME_BETWEEN_FRAMES = 0;
            long CURRENT_UPDATE_TICK = 0;
            long CURRENT_FRAME_TICK = 0;

            Window window = new Window("Cyberpunk 7702 | 2", WIDTH, HEIGHT);
            Manager manager = new Manager(window);

            SplashKit.CurrentWindowToggleFullscreen();
            do
            {
                CURRENT_UPDATE_TICK = DateTime.UtcNow.Ticks;
                manager.Update();
                SplashKit.ProcessEvents();
                SplashKit.Delay((uint)TIME_BETWEEN_UPDATES / 10000);
                if (DateTime.UtcNow.Ticks - CURRENT_FRAME_TICK >= TIME_BETWEEN_FRAMES)
                {
                    CURRENT_FRAME_TICK = DateTime.UtcNow.Ticks;
                    SplashKit.ClearScreen(Color.Black);
                    manager.Draw();
                    SplashKit.RefreshScreen();
                }
            } while (!SplashKit.WindowCloseRequested(window) && !SplashKit.KeyDown(KeyCode.EscapeKey));

        }
    }
}
