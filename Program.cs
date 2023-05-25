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
            Window window = new Window("Cyberpunk 7702 | 2", WIDTH, HEIGHT);
            Star[] stars = new Star[30];
            Something[] sths = new Something[10];
            string state = "home";
            Action<string> ChangeState = (string NewState) =>
            {
                state = NewState;
            };
            HomeStage home = new HomeStage(window, WIDTH, HEIGHT, ChangeState);


            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new Star();
            }
            for (int i = 0; i < sths.Length; i++)
            {
                sths[i] = new Something();
            }
            SplashKit.CurrentWindowToggleFullscreen();
            do
            {
                SplashKit.ProcessEvents();

                SplashKit.FillRectangle(Color.Black, 0, 0, WIDTH, HEIGHT);

                for (int i = 0; i < stars.Length; i++)
                {
                    stars[i].Update("test",WIDTH, HEIGHT);
                    stars[i].Draw();
                }
                for (int i = 0; i < sths.Length; i++)
                {
                    sths[i].Update("test", WIDTH, HEIGHT);
                    sths[i].Draw();
                }
                switch(state)
                {
                    case "home":
                        home.Update();
                        home.Draw();
                        break;
                }

                SplashKit.RefreshScreen(80);
                SplashKit.ClearScreen();
            } while (!SplashKit.WindowCloseRequested(window) && !SplashKit.KeyDown(KeyCode.EscapeKey));

        }
    }
}
