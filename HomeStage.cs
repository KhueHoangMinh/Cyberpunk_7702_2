using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Cyberpunk77022
{
    public class HomeStage
    {
        Bitmap logo;
        DrawingOptions drawingOptions;
        int _width;
        int _height;
        Button startBtn;
        Button shopBtn;
        Button aboutBtn;
        Action startAct;
        Action shopAct;
        Action aboutAct;
        public HomeStage(Window window, int width, int height, Action<string> ChangeState)
        {

            SplashKit.LoadBitmap("logo", "logo.png");
            logo = SplashKit.BitmapNamed("logo");
            drawingOptions = new DrawingOptions
            {
                Dest = window,
                ScaleX = (float)1.5,
                ScaleY = (float)1.5,

            };
            _width = width;
            _height = height;
            startBtn = new Button("START", Color.Red, width / 2, 400, 250, 150);
            shopBtn = new Button("SHOP", Color.Red, width / 2, 650, 250, 150);
            aboutBtn = new Button("ABOUT", Color.Red, width / 2, 900, 250, 150);
            startAct = () =>
            {
                ChangeState("start");
            };
            shopAct = () =>
            {
                ChangeState("shop");
            };
            aboutAct = () =>
            {
                ChangeState("about");
            };
        }

        public void Update()
        {
            startBtn.Update(startAct);
            shopBtn.Update(shopAct);
            aboutBtn.Update(aboutAct);
        }

        public void Draw() 
        {

            SplashKit.DrawBitmap(logo, _width / 2 - logo.Width / 2, 100, drawingOptions);
            startBtn.Draw();
            shopBtn.Draw();
            aboutBtn.Draw();
        }
    }
}
