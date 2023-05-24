using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using SplashKitSDK;

namespace Cyberpunk77022
{
    class Star
    {
        float _velocity = (float) (new Random().Next(1,5));
        float _x = new Random().Next(-500, 2420);
        float _y = new Random().Next(0, 1080);
        float _sizeX = (float)(new Random().Next(5, 10));
        float _sizeY = (float)(new Random().Next(25,40));
        Color _color = Color.RandomRGB(100);
        float diff_dis = (float)(new Random().Next(1, 10));
        string _state;
        float _initX;
        float _initY;

        public Star()
        {
            _initX = _x;
            _initY = _y;
        }

        public void Update(string state, int width, int height)
        {

            if (_initY < -500)
            {
                _initY = (float)height;
                _y = (float)height;
            }
            else
            {
                _initY = _initY - _velocity;
            }
            if (state == "game")
            {
                //_x = _x - @win.game.player.camx / _speed_dec;
            }
            else if(state != "game")
            {
                _x += (-_x + (_initX - ((float)SplashKit.MousePosition().X - (float)width/2) / diff_dis)) * (float)0.05;
                _y += (-_y + (_initY - ((float)SplashKit.MousePosition().Y - (float)height/2) / diff_dis)) * (float)0.05;
            }
        }

        public void Draw()
        {
            SplashKit.FillRectangle(_color, _x - _sizeX/2, _y - _sizeY/2, _sizeX, _sizeY);
        }
    }

    class Something
    {
        Color _color = Color.RandomRGB(10);
        float _x = new Random().Next(0, 1920);
        float _y = new Random().Next(0, 1080);
        float _sizeX = (float)(new Random().Next(200, 1200));
        float _sizeY = (float)(new Random().Next(200, 1200));
        float diff_dis = (float)(new Random().Next(1, 10));
        string _state;
        float _initX;
        float _initY;

        public Something()
        {
            _initX = _x;
            _initY = _y;
        }
        public void Update(string state, int width, int height)
        {
            if (state == "game")
            {
                //_x = _x - @win.game.player.camx / _speed_dec;
            }
            else if (state != "game")
            {
                _x += (-_x + (_initX - ((float)SplashKit.MousePosition().X - (float)width / 2) / diff_dis)) * (float)0.05;
                _y += (-_y + (_initY - ((float)SplashKit.MousePosition().Y - (float)height / 2) / diff_dis)) * (float)0.05;
            }
        }
        public void Draw()
        {
            SplashKit.FillRectangle(_color, _x - _sizeX / 2, _y - _sizeY / 2, _sizeX, _sizeY);
        }
    }
    public class Program
    {
        static void Main()
        {
            int WIDTH = 1920;
            int HEIGHT = 1080;
            new Window("Cyberpunk 7702 | 2", WIDTH, HEIGHT);
            Star[] stars = new Star[30];
            Something[] sths = new Something[10];
            SplashKit.LoadBitmap("logo","logo.png");
            Bitmap logo = SplashKit.BitmapNamed("logo");
            DrawingOptions drawingOptions = new DrawingOptions();
            drawingOptions.Dest = logo;
            drawingOptions.ScaleX = (float)1.5;
            drawingOptions.ScaleY = (float)1.5;

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

                SplashKit.DrawBitmap(logo, WIDTH/2 - logo.Width/2, 100, drawingOptions);

                SplashKit.Delay(5);
                SplashKit.RefreshScreen();
                SplashKit.ClearScreen();
            } while (!SplashKit.WindowCloseRequested("Cyberpunk 7702 | 2") && !SplashKit.KeyDown(KeyCode.EscapeKey));

        }
    }
}
