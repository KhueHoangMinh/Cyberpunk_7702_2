using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Manager
    {

        static int coin = 0;
        static int score = 0;
        static int bestScore = 0;
        List<Star> stars;
        List<Something> sths;
        string state = "home";
        Window _window;
        HomeStage home;
        GameStage game;
        EndStage end;

        public Manager(Window window)
        {
            SplashKit.LoadBitmap("logo", "logo.png");
            SplashKit.LoadBitmap("pistol", "guns/pistol.png");

            SplashKit.LoadSoundEffect("singleshot", "sounds/singleshot.mp3");
            SplashKit.LoadSoundEffect("click", "click.mp3");

            SplashKit.LoadFont("font", "Roboto-Bold.ttf");

            _window = window;

            stars = new List<Star>();
            sths = new List<Something>();

            home = new HomeStage(this);
            game = new GameStage(this);
            end = new EndStage(this);

            for (int i = 0; i < 30; i++)
            {
                stars.Add(new Star());
            }
            for (int i = 0; i < 10; i++)
            {
                sths.Add(new Something());
            }
        }


        public void Update()
        {
            for (int i = 0; i < stars.Count; i++)
            {
                if (state == "game")
                {
                    stars[i].Update("test", _window.Width, _window.Height, game.GetPlayer.Pos);
                }
                else
                {
                    stars[i].Update("test", _window.Width, _window.Height, SplashKit.MousePosition());
                }
                stars[i].Draw();
            }
            for (int i = 0; i < sths.Count; i++)
            {
                if (state == "game")
                {
                    sths[i].Update("test", _window.Width, _window.Height, game.GetPlayer.Pos);
                }
                else
                {
                    sths[i].Update("test", _window.Width, _window.Height, SplashKit.MousePosition());
                }
                sths[i].Draw();
            }
            switch (state)
            {
                case "home":
                    home.Update();
                    home.Draw();
                    break;
                case "game":
                    game.Update();
                    game.Draw();
                    break;
                case "end":
                    end.Update();
                    end.Draw();
                    break;
            }

        }

        public void NewHome()
        {
            home = new HomeStage(this);
            state = "home";
        }
        public void NewGame()
        {
            game = new GameStage(this);
            state = "game";
        }
        public void NewEnd()
        {
            end = new EndStage(this);
            state = "end";
        }
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        public Window Window
        {
            get { return _window; }
        }
        public HomeStage HomeStage { get { return home; } }

        public GameStage GameStage { get { return game; } }

        public EndStage EndStage { get { return end; } }
    }
}
