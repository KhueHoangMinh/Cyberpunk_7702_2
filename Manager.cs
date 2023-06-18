using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Manager
    {

        static int coin = 1000;
        static int score = 0;
        static int bestScore = 0;
        List<Star> stars;
        List<Something> sths;
        string state = "home";
        Window _window;
        HomeStage home;
        GameStage game;
        EndStage end;
        ShopStage shop;
        string _gun = "Default Gun";
        string _skin = "Default Skin";
        string _skill = null;

        List<string> _userdata = new List<string>();

        public Manager(Window window)
        {
            SplashKit.LoadBitmap("logo", "logo.png");
            SplashKit.LoadBitmap("default", "guns/default.png");
            SplashKit.LoadBitmap("gun1", "guns/gun1.png");
            SplashKit.LoadBitmap("gun2", "guns/gun2.png");
            SplashKit.LoadBitmap("gun3", "guns/gun3.png");
            SplashKit.LoadBitmap("gun4", "guns/gun4.png");
            SplashKit.LoadBitmap("gun5", "guns/gun5.png");
            SplashKit.LoadBitmap("gun6", "guns/gun6.png");

            SplashKit.LoadSoundEffect("singleshot", "sounds/singleshot.mp3");
            SplashKit.LoadSoundEffect("click", "click.mp3");

            SplashKit.LoadFont("font", "Roboto-Bold.ttf");

            _window = window;

            stars = new List<Star>();
            sths = new List<Something>();

            this.Load("../../../userdata.txt");

            home = new HomeStage(this);
            game = new GameStage(this);
            end = new EndStage(this);
            shop = new ShopStage(this, "home");

            for (int i = 0; i < 30; i++)
            {
                stars.Add(new Star());
            }
            for (int i = 0; i < 10; i++)
            {
                sths.Add(new Something());
            }

        }

        public void Load(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    StreamReader reader = new StreamReader(filename);

                    coin = int.Parse(reader.ReadLine());
                    score = int.Parse(reader.ReadLine());
                    bestScore = int.Parse(reader.ReadLine());

                    _userdata = new List<string>();
                    for (int i = 0; i <= 14; i++)
                    {
                        _userdata.Add(reader.ReadLine());
                    }

                    reader.Close();
                }
                catch
                {
                    Console.WriteLine("Load Failed");
                }
            } else
            {
                StreamWriter writer = new StreamWriter(filename);
                //writer.WriteLine("Info");
                for (int i = 1; i <= 3; i++)
                {
                    writer.WriteLine("1000");
                }
                //writer.WriteLine("Weapon");
                for (int i = 1; i <= 7; i++)
                {
                    writer.WriteLine("0");
                }
                //writer.WriteLine("Skin");
                for (int i = 1; i <= 6; i++)
                {
                    writer.WriteLine("0");
                }
                //writer.WriteLine("Skill");
                for (int i = 1; i <= 2; i++)
                {
                    writer.WriteLine("0");
                }
                writer.Close();
                _userdata = new List<string>();
                for (int i = 0; i <= 14; i++)
                {
                    _userdata.Add("0");
                }
                coin = 1000;
                score = 0;
                bestScore = 0;
            }
        }

        public void Save(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine(coin.ToString());
            writer.WriteLine(score.ToString());
            writer.WriteLine(bestScore.ToString());

            for (int i = 0; i <= 6; i++)
            {
                writer.WriteLine(_userdata[i]);
            }

            for (int i = 7; i <= 12; i++)
            {
                writer.WriteLine(_userdata[i]);
            }

            for (int i = 13; i <= 14; i++)
            {
                writer.WriteLine(_userdata[i]);
            }

            writer.Close();
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
                case "shop":
                    shop.Update();
                    shop.Draw();
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

        public void NewShop(string prevStage)
        {
            shop = new ShopStage(this, prevStage);
            state = "shop";
        }

        public int Score
        {
            get { return score; } 
            set 
            { 
                score = value;
                if(score >= BestScore)
                {
                    bestScore = score;
                }
            }
        }

        public int Coin
        {
            get { return  coin; } set { coin = value; }
        }

        public int BestScore
        {
            get { return bestScore; } set { bestScore = value; }
        }
        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public string Gun
        {
            get { return _gun; }
            set { Console.WriteLine(value); _gun = value; }
        }
        public string Skin
        {
            get { return _skin; }
            set { _skin = value; }
        }
        public string Skill
        {
            get { return _skill; }
            set { _skill = value; }
        }
        public Window Window
        {
            get { return _window; }
        }
        public HomeStage HomeStage { get { return home; } }

        public GameStage GameStage { get { return game; } }

        public EndStage EndStage { get { return end; } }

        public ShopStage ShopStage { get { return shop; } }

        public List<string> UserData
        {
            get { return _userdata; }
            set { _userdata = value; }
        }
    }
}
