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
    public class EndStage
    {
        Manager _manager;
        Button startBtn;
        Button shopBtn;
        Button homeBtn;
        InEffect inEf;
        OutEffect outEf;
        bool _closing = false;
        string _nextState;
        public EndStage(Manager manager)
        {
            _manager = manager;
            inEf = new InEffect(0, 0, _manager.Window.Width, _manager.Window.Height);
            outEf = new OutEffect(0,0,_manager.Window.Width, _manager.Window.Height);
            startBtn = new Button("START", Color.Red, _manager.Window.Width / 2, 400, 250, 150);
            shopBtn = new Button("SHOP", Color.Red, _manager.Window.Width / 2, 650, 250, 150);
            homeBtn = new Button("HOME", Color.Red, _manager.Window.Width / 2, 900, 250, 150);

        }

        public void Update()
        {
            startBtn.Update();
            shopBtn.Update();
            homeBtn.Update();
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (startBtn.Hovering)
                {
                    _closing = true;
                    _nextState = "game";
                }
                if (shopBtn.Hovering)
                {
                    _closing = true;
                    _nextState = "shop";
                }
                if (homeBtn.Hovering)
                {
                    _closing = true;
                    _nextState = "home";
                }
            }
            if (outEf._completed)
            {
                if (_nextState == "game")
                {
                    _manager.NewGame();
                }
                if (_nextState == "shop")
                {
                    _manager.NewShop("end");
                }
                if (_nextState == "home")
                {
                    _manager.NewHome();
                }
            }
        }

        public void Draw()
        {
            SplashKit.DrawText("Your score:", Color.White, "font", 30, _manager.Window.Width / 2 - SplashKit.TextWidth("Your score:", "font", 30) / 2, 70);
            SplashKit.DrawText(_manager.Score.ToString(), Color.White, "font", 120, _manager.Window.Width / 2 - SplashKit.TextWidth(_manager.Score.ToString(), "font", 120) / 2, 100);
            SplashKit.DrawText("Best score: " + _manager.BestScore, Color.Red, "font", 30, _manager.Window.Width / 2 - SplashKit.TextWidth("Best score: " + _manager.BestScore, "font", 30) / 2, 230);
            startBtn.Draw();
            shopBtn.Draw();
            homeBtn.Draw();

            inEf.Draw();
            if (_closing)
            {
                outEf.Draw();
            }
        }
    }
}