using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using SplashKitSDK;

namespace Cyberpunk77022
{
    
    public class Program
    {
        public Bitmap[] Bitmaps
        {
            get
            {
                return new Bitmap[] {
                    SplashKit.BitmapNamed("logo")
                };
            }
        }

        public SoundEffect[] SoundEffects
        {
            get
            {
                return new SoundEffect[] {
                    SplashKit.SoundEffectNamed("click"),
                    SplashKit.SoundEffectNamed("singleshot"),
                };
            }
        }
        public Font[] Fonts
        {
            get
            {
                return new Font[] {
                    SplashKit.FontNamed("font"),
                };
            }
        }
        static void Main()
        {
            SplashKit.LoadBitmap("logo", "logo.png");
            SplashKit.LoadBitmap("pistol", "guns/pistol.png");

            SplashKit.LoadSoundEffect("singleshot", "sounds/singleshot.mp3");
            SplashKit.LoadSoundEffect("click", "click.mp3");

            SplashKit.LoadFont("font", "Roboto-Bold.ttf");



            int WIDTH = 1920;
            int HEIGHT = 1080;
            Window window = new Window("Cyberpunk 7702 | 2", WIDTH, HEIGHT);
            List<Star> stars = new List<Star>();
            List<Something> sths = new List<Something>();
            string state = "home";
            Action<string> ChangeState = (string NewState) =>
            {
                state = NewState;
            };
            DrawingOptions drawingOptions = new DrawingOptions();
            HomeStage home = new HomeStage(window, WIDTH, HEIGHT, ChangeState);
            GameStage game = new GameStage(window, WIDTH, HEIGHT, ChangeState);
            drawingOptions.ScaleX = (float)3;


            for (int i = 0; i < 30; i++)
            {
                stars.Add(new Star());
            }
            for (int i = 0; i < 10; i++)
            {
                sths.Add(new Something());
            }

            SplashKit.CurrentWindowToggleFullscreen();
            do
            {
                SplashKit.ProcessEvents();
                SplashKit.ClearScreen(Color.Black);

                for (int i = 0; i < stars.Count; i++)
                {
                    if(state == "game")
                    {
                        stars[i].Update("test", WIDTH, HEIGHT, game.GetPlayer.Pos);
                    }
                    else
                    {
                        stars[i].Update("test", WIDTH, HEIGHT, SplashKit.MousePosition());
                    }
                    stars[i].Draw();
                }
                for (int i = 0; i < sths.Count; i++)
                {
                    if (state == "game")
                    {
                        sths[i].Update("test", WIDTH, HEIGHT, game.GetPlayer.Pos);
                    }
                    else
                    {
                        sths[i].Update("test", WIDTH, HEIGHT, SplashKit.MousePosition());
                    }
                    sths[i].Draw();
                }
                switch(state)
                {
                    case "home":
                        home.Update();
                        home.Draw();
                        break;
                    case "game":
                        game.Update();
                        game.Draw();
                        break;
                }
                //SplashKit.Delay(8);
                SplashKit.RefreshScreen(75);
            } while (!SplashKit.WindowCloseRequested(window) && !SplashKit.KeyDown(KeyCode.EscapeKey));

        }
    }
}
