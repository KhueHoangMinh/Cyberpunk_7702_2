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
    public class EndStage : Stage
    {
        Button startBtn;
        Button shopBtn;
        Button homeBtn;
        string _nextState;
        public EndStage(Manager manager) : base(manager)
        {
            startBtn = new Button("START", Color.Red, this.Manager.Window.Width / 2, 400, 250, 150);
            shopBtn = new Button("SHOP", Color.Red, this.Manager.Window.Width / 2, 650, 250, 150);
            homeBtn = new Button("HOME", Color.Red, this.Manager.Window.Width / 2, 900, 250, 150);

        }

        public override void Update()
        {
            startBtn.Update();
            shopBtn.Update();
            homeBtn.Update();
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (startBtn.Hovering)
                {
                    this.Closing = true;
                    _nextState = "game";
                }
                if (shopBtn.Hovering)
                {
                    this.Closing = true;
                    _nextState = "shop";
                }
                if (homeBtn.Hovering)
                {
                    this.Closing = true;
                    _nextState = "home";
                }
            }
            if (this.OutEf._completed)
            {
                if (_nextState == "game")
                {
                    this.Manager.NewGame();
                }
                if (_nextState == "shop")
                {
                    this.Manager.NewShop("end");
                }
                if (_nextState == "home")
                {
                    this.Manager.NewHome();
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
            SplashKit.DrawText("Your score:", Color.White, "font", 30, this.Manager.Window.Width / 2 - SplashKit.TextWidth("Your score:", "font", 30) / 2, 70);
            SplashKit.DrawText(this.Manager.Score.ToString(), Color.White, "font", 120, this.Manager.Window.Width / 2 - SplashKit.TextWidth(this.Manager.Score.ToString(), "font", 120) / 2, 100);
            SplashKit.DrawText("Best score: " + this.Manager.BestScore, Color.Red, "font", 30, this.Manager.Window.Width / 2 - SplashKit.TextWidth("Best score: " + this.Manager.BestScore, "font", 30) / 2, 230);
            startBtn.Draw();
            shopBtn.Draw();
            homeBtn.Draw();

            this.InEf.Draw();
            this.OutEf.Draw();
        }
    }
}