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
            SplashKit.LoadBitmap("logo","logo.png");
            Bitmap logo = SplashKit.BitmapNamed("logo");
            Button startBtn = new Button("Start",Color.Red,WIDTH/2,HEIGHT/2,200,200);
            DrawingOptions drawingOptions = new DrawingOptions
            {
                Dest = window,
                ScaleX = (float)1.5,
                ScaleY = (float)1.5,

            };
            

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

                startBtn.Update();
                startBtn.Draw();

                SplashKit.DrawBitmap(logo, WIDTH/2 - logo.Width/2, 100, drawingOptions);

                SplashKit.Delay(5);
                SplashKit.RefreshScreen();
                SplashKit.ClearScreen();
            } while (!SplashKit.WindowCloseRequested("Cyberpunk 7702 | 2") && !SplashKit.KeyDown(KeyCode.EscapeKey));

        }
    }
}
