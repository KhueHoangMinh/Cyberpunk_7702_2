using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Cyberpunk77022
{
    public class HomeStage : Stage
    {
        DrawingOptions drawingOptions;
        Button startBtn;
        Button shopBtn;
        Button aboutBtn;
        string _nextState;
        public HomeStage(Manager manager) : base(manager)
        {
            drawingOptions = new DrawingOptions()
            {
                Dest = manager.Window,
                ScaleX = (float)1.5,
                ScaleY = (float)1.5,

            };
            startBtn = new Button("START", Color.Red, this.Manager.Window.Width / 2, 400, 250, 150);
            shopBtn = new Button("SHOP", Color.Red, this.Manager.Window.Width / 2, 650, 250, 150);
            aboutBtn = new Button("ABOUT", Color.Red, this.Manager.Window.Width / 2, 900, 250, 150);

        }

        public override void Update()
        {
            startBtn.Update();
            shopBtn.Update();
            aboutBtn.Update();
            if(SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if(startBtn.Hovering)
                {
                    this.Closing = true;
                    _nextState = "game";
                }
                if (shopBtn.Hovering)
                {
                    this.Closing = true;
                    _nextState = "shop";
                }
                if (aboutBtn.Hovering)
                {
                    this.Closing = true;
                    _nextState = "about";
                }
            }
            if(this.OutEf._completed)
            {
                if(_nextState == "game")
                {
                    this.Manager.NewGame();
                }
                if(_nextState == "shop")
                {
                    this.Manager.NewShop("home");
                }
            }

            this.InEf.Update();
            if (this.Closing)
            {
                this.OutEf.Update();
            }
        }

        public override void Draw() 
        {

            SplashKit.DrawBitmap(SplashKit.BitmapNamed("logo"), this.Manager.Window.Width / 2 - SplashKit.BitmapNamed("logo").Width / 2, 100, drawingOptions);
            startBtn.Draw();
            shopBtn.Draw();
            aboutBtn.Draw();

            this.InEf.Draw();
            this.OutEf.Draw();
        }
    }
}
