using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cyberpunk77022
{
    public class Player : Object
    {
        float _a = (float)1;
        bool _jumped = false;
        Manager _manager;
        Gun _PlayerGun;
        GameStage _game;
        Camera _camera;
        float _health;
        float _maxHealth;
        float _minusHealth = 0;
        string _skill;

        Gun TakeGun(string name)
        {
            switch(name)
            {
                case "Gun 1":
                    return new Sniper1(_game, this, 99);
                case "Gun 2":
                    return new Pistol2(_game, this, 50);
                case "Gun 3":
                    return new Rifle1(_game, this, 20);
                case "Gun 4":
                    return new Rifle2(_game, this, 25);
                case "Gun 5":
                    return new Sniper2(_game, this, 100);
                case "Gun 6":
                    return new Shotgun1(_game, this, 30);
                default:
                    return new Pistol1(_game, this, 40);
            }
        }

        Color TakeSkin(string name)
        {
            switch (name)
            {
                case "Blue":
                    return Color.Blue;
                case "Green":
                    return Color.Green;
                case "Red":
                    return Color.Red;
                case "Yellow":
                    return Color.Yellow;
                case "Gray":
                    return Color.Gray;
                case "Pink":
                    return Color.Pink;
                default:
                    return Color.Blue;
            }
        }
        public Player(GameStage game, Camera camera, Point2D pos, float sizeX, float sizeY, string weapon, string skin, string skill) : base(camera, pos, sizeX,sizeY,Color.White,true,0,0) { 
            _manager = game.Manager;
            _game = game;
            _PlayerGun = TakeGun(weapon);
            this.Color = TakeSkin(skin);
            _camera = camera;
            _maxHealth = 100;
            if(skill == "Health")
            {
                _maxHealth = 200;
            }
            _health = _maxHealth;
            _skill = skill;
        }

        public void Update(List<Ground> grounds, List<Bullet> bullets)
        {
            _jumped = true;
            string collide = "no";
            for (int i = 0; i < grounds.Count; i++)
            {
                string isCollide = this.IsCollideAt(grounds[i]);
                if(isCollide != "no") collide = isCollide;
                if (isCollide == "bottom")
                {
                    this.Pos = new Point2D() { X = this.Pos.X, Y = grounds[i].Top - (this.Bottom - this.Pos.Y) + 1 };
                    if(this.VelY > 0) this.VelY = 0;
                    _jumped = false;
                } else
                if (isCollide == "top")
                {
                    this.Pos = new Point2D() { X = this.Pos.X, Y = grounds[i].Bottom + (-this.Top + this.Pos.Y - 1) };
                    if (this.VelY < 0) this.VelY = 0;
                } else
                if (isCollide == "right")
                {
                    _jumped = false;
                    this.Pos = new Point2D() { X = grounds[i].Left + (-this.Right + this.Pos.X), Y = this.Pos.Y - 1 };
                    if(this.VelX > 0) this.VelX = 0;
                } else
                if (isCollide == "left")
                {
                    _jumped = false;
                    this.Pos = new Point2D() { X = grounds[i].Right - (this.Left - this.Pos.X), Y = this.Pos.Y + 1 };
                    if (this.VelX < 0) this.VelX = 0;
                }
            }
            base.Gravity();
            if(SplashKit.KeyDown(KeyCode.AKey))
            {
                this.VelX -= _a;
            } else if (SplashKit.KeyDown(KeyCode.DKey))
            {
                this.VelX += _a;
            }
            if (SplashKit.KeyDown(KeyCode.WKey) && !_jumped)
            {
                this.VelY = -10;
                for(int i = 0; i < 3; i++)
                {
                    switch(collide)
                    {
                        case "bottom":
                            _game.AddExplosion(new Explosion(_game, _camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                                Y = (double)new Random().Next((int)this.Bottom - 10, (int)this.Bottom + 10),
                            }, Color.White));
                            break;

                        case "left":
                            _game.AddExplosion(new Explosion(_game, _camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Left - 20, (int)this.Left + 20),
                                Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                            }, Color.White));
                            break;
                            
                        case "right":
                            _game.AddExplosion(new Explosion(_game, _camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Right - 20, (int)this.Right + 20),
                                Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                            }, Color.White));
                            break;

                    }
                }
            }
            VelX = VelX / 1.06f;
            this.Pos = new Point2D() { X = this.Pos.X + this.VelX, Y = this.Pos.Y + this.VelY };
            _PlayerGun.Update((new Point2D() { X = SplashKit.MousePosition().X + _camera.Pos.X, Y = SplashKit.MousePosition().Y + _camera.Pos.Y }));
            if (SplashKit.MouseDown(MouseButton.LeftButton))
            {
                _PlayerGun.Shoot();
            }
        }

        public void GetHit(Bullet bullet)
        {
            _game.Camera.Shock(bullet.VelX, bullet.VelY);
            _minusHealth = _health;
            if (_skill == "Defense")
            {
                _health -= bullet.Damage * 0.6f;
            } else
            {
                _health -= bullet.Damage;
            }
            _minusHealth -= _health;
        }

        public void DrawGun() 
        {
            _PlayerGun.Draw();
        }

        public void DrawHealth()
        {
            _minusHealth *= 0.98f;
            SplashKit.FillRectangle(Color.Gray, this.Left - _camera.Pos.X, this.Top - 20 - _camera.Pos.Y, this.Right - this.Left, 10);
            SplashKit.FillRectangle(Color.Green, this.Left - _camera.Pos.X, this.Top - 20 - _camera.Pos.Y, (this.Right - this.Left) * _health / _maxHealth, 10);
            SplashKit.FillRectangle(Color.RGBAColor(255, 0, 0, 90), this.Left + (this.Right - this.Left) * _health / _maxHealth - _camera.Pos.X, this.Top - 20 - _camera.Pos.Y, (this.Right - this.Left) * _minusHealth / _maxHealth, 10);
            DrawSkill(_skill);
        }

        public void DrawSkill(string skill)
        {
            switch (skill)
            {
                case "Health":
                    SplashKit.FillRectangle(Color.Green, this.Right - _camera.Pos.X + 15 - 5, this.Top - 15 - _camera.Pos.Y - 10, 10, 20);
                    SplashKit.FillRectangle(Color.Green, this.Right - _camera.Pos.X + 15 - 10, this.Top - 15 - _camera.Pos.Y - 5, 20, 10);
                    break;
                case "Defense":
                    SplashKit.FillRectangle(Color.DarkGray, this.Right - _camera.Pos.X + 10, this.Top - 20 - _camera.Pos.Y, 10, 10);
                    break;
            }
        }

        public Gun Gun
        {
            get { return _PlayerGun; }
            set { _PlayerGun = value; }
        }
        public float Health
        {
            get
            {
                return _health;
            }
        }

        public GameStage Game
        {
            get { return _game; }
        }
    }
}
