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
        DrawingOptions drawingOptions;
        Manager _manager;
        Button startBtn;
        Button shopBtn;
        Button aboutBtn;
        InEffect inEf;
        OutEffect outEf;
        bool _closing = false;
        string _nextState;
        public HomeStage(Manager manager)
        {
            _manager = manager;
            drawingOptions = new DrawingOptions()
            {
                Dest = manager.Window,
                ScaleX = (float)1.5,
                ScaleY = (float)1.5,

            };
            inEf = new InEffect(0, 0, _manager.Window.Width, _manager.Window.Height);
            outEf = new OutEffect(0,0,_manager.Window.Width, _manager.Window.Height);
            startBtn = new Button("START", Color.Red, _manager.Window.Width / 2, 400, 250, 150);
            shopBtn = new Button("SHOP", Color.Red, _manager.Window.Width / 2, 650, 250, 150);
            aboutBtn = new Button("ABOUT", Color.Red, _manager.Window.Width / 2, 900, 250, 150);

        }

        public void Update()
        {
            startBtn.Update();
            shopBtn.Update();
            aboutBtn.Update();
            if(SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if(startBtn.Hovering)
                {
                    _closing = true;
                    _nextState = "game";
                }
                if (shopBtn.Hovering)
                {
                    _closing = true;
                    _nextState = "shop";
                }
                if (aboutBtn.Hovering)
                {
                    _closing = true;
                    _nextState = "about";
                }
            }
            if(outEf._completed)
            {
                if(_nextState == "game")
                {
                    _manager.NewGame();
                }
                if(_nextState == "shop")
                {
                    _manager.NewShop("home");
                }
            }
        }

        public void Draw() 
        {

            SplashKit.DrawBitmap(SplashKit.BitmapNamed("logo"), _manager.Window.Width / 2 - SplashKit.BitmapNamed("logo").Width / 2, 100, drawingOptions);
            startBtn.Draw();
            shopBtn.Draw();
            aboutBtn.Draw();

            inEf.Draw();
            if(_closing)
            {
                outEf.Draw();
            }
        }
    }
}
