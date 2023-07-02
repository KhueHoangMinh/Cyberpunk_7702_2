using Cyberpunk77022.skills;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        Stage currentStage;
        Gun _gun;
        Skin _skin;
        Skill _skill = null;

        List<string> _userdata = new List<string>();

        List<ShopItem> _guns;
        List<ShopItem> _skins;
        List<ShopItem> _skills;

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
            SplashKit.LoadBitmap("rpg", "guns/rpg.png");

            SplashKit.LoadSoundEffect("singleshot", "sounds/singleshot.mp3");
            SplashKit.LoadSoundEffect("click", "click.mp3");

            SplashKit.LoadFont("font", "Roboto-Bold.ttf");


            _window = window;

            _guns = new List<ShopItem>()
            {
                new Pistol1(_window, 40),
                new Sniper1(_window, 99),
                new Pistol2(_window, 50),
                new Rifle1(_window, 20),
                new Rifle2(_window, 25),
                new Sniper2(_window, 100),
                new Shotgun1(_window, 30),
                new RPG(_window, 300)
            };

            _skins = new List<ShopItem>()
            {
                new Skin("Blue", "blue", "a skin", 0, Color.Blue),
                new Skin("Green", "green", "a skin", 0, Color.Green),
                new Skin("Red", "red", "a skin", 0, Color.Red),
                new Skin("Yellow", "yellow", "a skin", 0, Color.Yellow),
                new Skin("Gray", "gray", "a skin", 0, Color.Gray),
                new Skin("Pink", "pink", "a skin", 0, Color.Pink)
            };

            _skills = new List<ShopItem>()
            {
                new Health(),
                new Defense()
            };



            stars = new List<Star>();
            sths = new List<Something>();

            this.Load("../../../userdata.txt");

            _gun = _guns[0] as Gun;
            Console.WriteLine(_gun);
            _skin = _skins[0] as Skin;

            home = new HomeStage(this);
            game = new GameStage(this);
            end = new EndStage(this);
            shop = new ShopStage(this, "home");

            currentStage = home;

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
                    for (int i = 0; i <= 15; i++)
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
                for (int i = 1; i <= 8; i++)
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
                for (int i = 0; i <= 15; i++)
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

            for (int i = 0; i <= 7; i++)
            {
                writer.WriteLine(_userdata[i]);
            }

            for (int i = 8; i <= 13; i++)
            {
                writer.WriteLine(_userdata[i]);
            }

            for (int i = 14; i <= 15; i++)
            {
                writer.WriteLine(_userdata[i]);
            }

            writer.Close();
        }

        public void Update()
        {

            for (int i = 0; i < stars.Count; i++)
            {
                if (currentStage is GameStage)
                {
                    stars[i].Update("test", _window.Width, _window.Height, (currentStage as GameStage).GetPlayer.Pos);
                }
                else
                {
                    stars[i].Update("test", _window.Width, _window.Height, SplashKit.MousePosition());
                }
            }
            for (int i = 0; i < sths.Count; i++)
            {
                if (currentStage is GameStage)
                {
                    sths[i].Update("test", _window.Width, _window.Height, (currentStage as GameStage).GetPlayer.Pos);
                }
                else
                {
                    sths[i].Update("test", _window.Width, _window.Height, SplashKit.MousePosition());
                }
            }
            currentStage.Update();

        }

        public void Draw()
        {
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].Draw();
            }
            for (int i = 0; i < sths.Count; i++)
            {
                sths[i].Draw();
            }
            currentStage.Draw();
        }

        public void NewHome()
        {
            home = new HomeStage(this);
            currentStage = home;
            state = "home";
        }
        public void NewGame()
        {
            game = new GameStage(this);
            currentStage = game;
            state = "game";
        }
        public void NewEnd()
        {
            end = new EndStage(this);
            currentStage = end;
            state = "end";
        }

        public void NewShop(string prevStage)
        {
            shop = new ShopStage(this, prevStage);
            currentStage = shop;
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

        public List<ShopItem> Guns
        {
            get { return _guns; }
        }
        public List<ShopItem> Skins
        {
            get { return _skins; }
        }
        public List<ShopItem> Skills
        {
            get { return _skills; }
        }

        public Gun Gun
        {
            get { return _gun; }
            set { _gun = value; }
        }
        public Skin Skin
        {
            get { return _skin; }
            set { _skin = value; }
        }
        public Skill Skill
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
