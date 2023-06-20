using System;
using System.ComponentModel;
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
            long TIME_BETWEEN_FRAME = 120000;
            long CURRENT_TICK = 0;
            Window window = new Window("Cyberpunk 7702 | 2", WIDTH, HEIGHT);
            Manager manager = new Manager(window);
            SplashKit.CurrentWindowToggleFullscreen();
            do
            {
                if (DateTime.UtcNow.Ticks - CURRENT_TICK >= TIME_BETWEEN_FRAME)
                {
                    SplashKit.ProcessEvents();
                    SplashKit.ClearScreen(Color.Black);
                    CURRENT_TICK = DateTime.UtcNow.Ticks;
                    manager.Draw();
                    manager.Update();
                    SplashKit.RefreshScreen();
                }
            } while (!SplashKit.WindowCloseRequested(window) && !SplashKit.KeyDown(KeyCode.EscapeKey));

        }
    }
}
