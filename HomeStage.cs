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

        float x = 200;
        float y = 200;
        float w = 50;
        float h = 200;
        float angle = 0;


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
        Action<string> setClosing;
        InEffect inEf;
        OutEffect outEf;
        bool _closing = false;
        string _nextState;
        Action<string> _ChangeState;
        public HomeStage(Window window, int width, int height, Action<string> ChangeState)
        {

            SplashKit.LoadBitmap("logo", "logo.png");
            logo = SplashKit.BitmapNamed("logo");
            drawingOptions = new DrawingOptions()
            {
                Dest = window,
                ScaleX = (float)1.5,
                ScaleY = (float)1.5,
                //AnchorOffsetX = -logo.Width / 2,
                //AnchorOffsetY = -logo.Height / 2,

            };
            //drawingOptions.Angle = 80;
            _width = width;
            _height = height;
            inEf = new InEffect(width,height);
            outEf = new OutEffect(width, height);
            startBtn = new Button("START", Color.Red, width / 2, 400, 250, 150);
            shopBtn = new Button("SHOP", Color.Red, width / 2, 650, 250, 150);
            aboutBtn = new Button("ABOUT", Color.Red, width / 2, 900, 250, 150);
            startAct = () =>
            {
                _closing = true;
                _nextState = "game";
            };
            shopAct = () =>
            {
                _closing = true;
                _nextState = "shop";
            };
            aboutAct = () =>
            {
                _closing = true;
                _nextState = "about";
            };
            _ChangeState = ChangeState;

        }

        public void Update()
        {
            startBtn.Update(startAct);
            shopBtn.Update(shopAct);
            aboutBtn.Update(aboutAct);
            if(outEf._completed)
            {
                _ChangeState(_nextState);
            }
        }

        public void Draw() 
        {

            SplashKit.DrawBitmap(logo, _width / 2 - logo.Width / 2, 100, drawingOptions);
            startBtn.Draw();
            shopBtn.Draw();
            aboutBtn.Draw();

            angle = (float)(Math.Atan((SplashKit.MousePosition().X - 200) / (SplashKit.MousePosition().Y - 200)));

            SplashKit.FillQuad(Color.White, new Quad() { Points = new Point2D[4] {
                new Point2D() { X = x, Y = y },
                new Point2D() { X = x + h * Math.Sin(90-angle), Y = y + h * Math.Cos(90 - angle) },
                new Point2D() { X =  x + w * Math.Sin(angle), Y = y + w * Math.Cos(angle) },
                new Point2D() { X = x + w * Math.Sin(angle) + h * Math.Sin(90-angle), Y = y + w * Math.Cos(angle) + h * Math.Cos(90 - angle)}}});

            inEf.Draw();
            if(_closing)
            {
                outEf.Draw();
            }
        }
    }
}
