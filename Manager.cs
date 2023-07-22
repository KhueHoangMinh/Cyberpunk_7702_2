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
        AboutStage about;
        Stage currentStage;
        Gun[] _selectedGun = new Gun[2];
        int _gun = 0;
        Skin _skin;
        Skill _skill = null;
        Music background;

        List<ShopItem> _guns;
        List<ShopItem> _skins;
        List<ShopItem> _skills;

        public Manager(Window window)
        {
            SplashKit.LoadBitmap("logo", "images/logo.png");
            SplashKit.LoadBitmap("default", "images/guns_img/default.png");
            SplashKit.LoadBitmap("gun1", "images/guns_img/gun1.png");
            SplashKit.LoadBitmap("gun2", "images/guns_img/gun2.png");
            SplashKit.LoadBitmap("gun3", "images/guns_img/gun3.png");
            SplashKit.LoadBitmap("gun4", "images/guns_img/gun4.png");
            SplashKit.LoadBitmap("gun5", "images/guns_img/gun5.png");
            SplashKit.LoadBitmap("gun6", "images/guns_img/gun6.png");
            SplashKit.LoadBitmap("rpg", "images/guns_img/rpg.png");

            SplashKit.LoadBitmap("coin_single", "images/coin_single.png");
            SplashKit.LoadBitmap("coin_animation", "images/coin.png");
            SplashKit.BitmapSetCellDetails(
                SplashKit.BitmapNamed("coin_animation"),
                 SplashKit.BitmapNamed("coin_animation").Width,
                 SplashKit.BitmapNamed("coin_animation").Width,
                 1,
                 7,
                 7
            );

            SplashKit.LoadSoundEffect("singleshot", "sounds/singleshot.mp3");
            SplashKit.LoadSoundEffect("singleshot2", "sounds/singleshot2.mp3");
            SplashKit.LoadSoundEffect("shotgun", "sounds/shotgun.mp3");
            SplashKit.LoadSoundEffect("rifle", "sounds/rifle.mp3");
            SplashKit.LoadSoundEffect("sniper", "sounds/sniper.mp3");
            SplashKit.LoadSoundEffect("hit", "sounds/hit.mp3");

            SplashKit.LoadSoundEffect("click", "sounds/click.mp3");
            SplashKit.LoadSoundEffect("denied", "sounds/denied.mp3");

            SplashKit.LoadSoundEffect("dying1", "sounds/dying1.mp3");
            SplashKit.LoadSoundEffect("dying2", "sounds/dying2.mp3");
            SplashKit.LoadSoundEffect("dying3", "sounds/dying3.mp3");
            SplashKit.LoadSoundEffect("coin", "sounds/coin.mp3");

            SplashKit.LoadMusic("background", "sounds/background.mp3");

            SplashKit.LoadFont("font", "fonts/Roboto-Bold.ttf");



            _window = window;

            background = SplashKit.MusicNamed("background");
            SplashKit.SetMusicVolume(0.6f);
            background.FadeIn(3000);

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
                new Skin("Green", "green", "a skin", 10, Color.Green),
                new Skin("Red", "red", "a skin", 20, Color.Red),
                new Skin("Yellow", "yellow", "a skin", 30, Color.Yellow),
                new Skin("Gray", "gray", "a skin", 40, Color.Gray),
                new Skin("Pink", "pink", "a skin", 50, Color.Pink)
            };

            _skills = new List<ShopItem>()
            {
                new Health(),
                new Defense()
            };

            _guns[0].Purchased = true;
            _skins[0].Purchased = true;

            _selectedGun = new Gun[2] { _guns[0] as Gun, null };
            _gun = 0;
            _skin = _skins[0] as Skin;


            stars = new List<Star>();
            sths = new List<Something>();

            this.Load("../../../userdata.txt");


            home = new HomeStage(this);
            game = new GameStage(this);
            end = new EndStage(this);
            shop = new ShopStage(this, "home");
            about = new AboutStage(this);

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

                    string loading = "";
                    string data = reader.ReadLine();
                    int i = 0;
                    while(data != "===END===" && data != null)
                    {
                        if (data == "Weapon" || data == "Skin" || data == "Skill")
                        {
                            i = 0;
                            loading = data;
                        } else 
                        if (data == "0" || data == "1" || data == "2")
                        {
                            switch(data)
                            {
                                case "0":
                                    break;
                                case "1":
                                    switch (loading)
                                    {
                                        case "Weapon":
                                            _guns[i].Purchased = true;
                                            break;
                                        case "Skin":
                                            _skins[i].Purchased = true;
                                            break;
                                        case "Skill":
                                            _skills[i].Purchased = true;
                                            break;
                                    }
                                    break;
                                case "2":
                                    switch (loading)
                                    {
                                        case "Weapon":
                                            _guns[i].Purchased = true;
                                            _selectedGun[1] = _selectedGun[0];
                                            _selectedGun[0] = _guns[i] as Gun;
                                            _gun = 0;
                                            break;
                                        case "Skin":
                                            _skins[i].Purchased = true;
                                            _skin = _skins[i] as Skin;
                                            break;
                                        case "Skill":
                                            _skills[i].Purchased = true;
                                            _skill = _skills[i] as Skill;
                                            break;
                                    }
                                    break;
                            }
                            i++;
                        }
                        data = reader.ReadLine();
                    }

                    reader.Close();
                }
                catch
                {
                    Console.WriteLine("Load Failed");
                }
            } else
            {
                coin = 1000;
                score = 0;
                bestScore = 0;
                _guns[0].Purchased = true;
                _skins[0].Purchased = true;
                _selectedGun[0] = _guns[0] as Gun;
                _skin = _skins[0] as Skin;
                Save(filename);
            }
        }

        public void Save(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine(coin.ToString());
            writer.WriteLine(score.ToString());
            writer.WriteLine(bestScore.ToString());

            writer.WriteLine("Weapon");
            for (int i = 0; i < _guns.Count; i++)
            {
                if (_guns[i].Purchased)
                {
                    if(_selectedGun.Contains(_guns[i]))
                    {
                        writer.WriteLine("2");
                    } else
                    {
                        writer.WriteLine("1");
                    }
                } else
                {
                    writer.WriteLine("0");
                }
            }

            writer.WriteLine("Skin");
            for (int i = 0; i < _skins.Count; i++)
            {
                if (_skins[i].Purchased)
                {
                    if (_skin == _skins[i])
                    {
                        writer.WriteLine("2");
                    }
                    else
                    {
                        writer.WriteLine("1");
                    }
                }
                else
                {
                    writer.WriteLine("0");
                }
            }

            writer.WriteLine("Skill");
            for (int i = 0; i < _skills.Count; i++)
            {
                if (_skills[i].Purchased)
                {
                    if (_skill == _skills[i])
                    {
                        writer.WriteLine("2");
                    }
                    else
                    {
                        writer.WriteLine("1");
                    }
                }
                else
                {
                    writer.WriteLine("0");
                }
            }

            writer.WriteLine("===END===");

            writer.Close();
        }

        public void Update()
        {
            if(!SplashKit.MusicPlaying())
            {
                background.Play();
            }
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
        public void NewAbout()
        {
            about = new AboutStage(this);
            currentStage = about;
            state = "about";
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

        public Gun[] SelectedGun
        {
            get { return _selectedGun; }
        }

        public Gun Gun
        {
            get { return _selectedGun[_gun]; }
            set
            {
                _selectedGun[1] = _selectedGun[0];
                _selectedGun[0] = value;
                _gun = 0;
            }
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
    }
}
