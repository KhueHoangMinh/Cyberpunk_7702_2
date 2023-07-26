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
        public Player(GameStage game, Camera camera, Point2D pos, float sizeX, float sizeY) : base(camera, pos, sizeX,sizeY, Color.White, true,0,0) { 
            _manager = game.Manager;
            _game = game;
            _PlayerGun = this.Game.Manager.SelectedGun[0];
            _PlayerGun.Game = game;
            _PlayerGun.GunOf = this;

            this.Color = game.Manager.Skin.Color;
            _camera = camera;
            _maxHealth = 100;
            if(game.Manager.Skill != null && game.Manager.Skill.ID == "health")
            {
                _maxHealth = 200;
            }
            _health = _maxHealth;
        }

        public override void CollideTop(Object @object)
        {
            this.Pos = new Point2D() { X = this.Pos.X + this.VelX, Y = @object.Bottom + (this.Bottom - this.Top) / 2 + 1 };
            if (this.VelY < 0) this.VelY = 0;
            if (this.Collide == "no") this.Collide = "top";
        }
        public override void CollideBottom(Object @object)
        {
            this.Pos = new Point2D() { X = this.Pos.X + this.VelX, Y = @object.Top - (this.Bottom - this.Top) / 2 - 1 };
            this.VelX = this.VelX * 0.96f;
            if (this.VelY > 0) this.VelY = 0;
            _jumped = false;
            if (this.Collide == "no") this.Collide = "bottom";
        }
        public override void CollideRight(Object @object)
        {
            this.Pos = new Point2D() { X = @object.Left - (this.Right - this.Left) / 2 - 1, Y = this.Pos.Y + this.VelY };
            if (this.VelX > 0) this.VelX = 0;
            _jumped = false;
            if (this.Collide == "no") this.Collide = "right";
        }
        public override void CollideLeft(Object @object)
        {
            this.Pos = new Point2D() { X = @object.Right + (this.Right - this.Left) / 2 + 1, Y = this.Pos.Y + this.VelY };
            if (this.VelX < 0) this.VelX = 0;
            _jumped = false;
            if (this.Collide == "no") this.Collide = "left";
        }

        public override void Update()
        {
            if (this.Pos.Y > 3000)
            {
                _health = -1;
            }
            _jumped = true;


            this.VelX = this.VelX * 0.96f;

            if (SplashKit.KeyDown(KeyCode.Num1Key))
            {
                _PlayerGun = this.Game.Manager.SelectedGun[0];
                _PlayerGun.Game = this.Game;
                _PlayerGun.GunOf = this;
            }

            if (SplashKit.KeyDown(KeyCode.Num2Key))
            {
                _PlayerGun = this.Game.Manager.SelectedGun[1];
                _PlayerGun.Game = this.Game;
                _PlayerGun.GunOf = this;
            }

            this.VelY += G;
            this.MoveObject(_game.Grounds);
            _PlayerGun.Update(new Point2D() { X = SplashKit.MousePosition().X + _camera.Pos.X, Y = SplashKit.MousePosition().Y + _camera.Pos.Y });
            if (SplashKit.KeyDown(KeyCode.AKey))
            {
                this.VelX -= _a;
            }
            else if (SplashKit.KeyDown(KeyCode.DKey))
            {
                this.VelX += _a;
            }
            if (SplashKit.KeyDown(KeyCode.WKey) && !_jumped)
            {
                this.VelY = -20;
                for (int i = 0; i < 3; i++)
                {
                    switch (this.Collide)
                    {
                        case "bottom":
                            _game.AddExplosion(new AniExplosion(this.Game, this.Game.Camera, new Random().Next(150,200), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                                Y = (double)new Random().Next((int)this.Bottom - 10, (int)this.Bottom + 10),
                            }, SplashKit.BitmapNamed("smoke_animation")));
                            break;

                        case "left":
                            _game.AddExplosion(new AniExplosion(this.Game, this.Game.Camera, new Random().Next(150, 200), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Left - 20, (int)this.Left + 20),
                                Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                            }, SplashKit.BitmapNamed("smoke_animation")));
                            break;

                        case "right":
                            _game.AddExplosion(new AniExplosion(this.Game, this.Game.Camera, new Random().Next(150, 200), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Right - 20, (int)this.Right + 20),
                                Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                            }, SplashKit.BitmapNamed("smoke_animation")));
                            break;

                    }
                }
            }
            if (SplashKit.MouseDown(MouseButton.LeftButton))
            {
                _PlayerGun.Shoot();
            }
            this.Collide = "no";
        }

        public void GetHit(Bullet bullet)
        {
            _game.Camera.Shock(bullet.VelX, bullet.VelY, bullet.Speed);
            _minusHealth = _health;
            if (_game.Manager.Skill != null && _game.Manager.Skill.ID == "defense")
            {
                _health -= bullet.Damage * 0.6f;
            } else
            {
                _health -= bullet.Damage;
            }
            _minusHealth -= _health;
            _game.AddMinusHealth(new MinusHealth(_game, this, (int)_minusHealth));
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
            DrawSkill();
        }

        public void DrawSkill()
        {
            if(_game.Manager.Skill != null) _game.Manager.Skill.InGameGraphic((float)(this.Right - _camera.Pos.X + 15), (float)(this.Top - 15 - _camera.Pos.Y));
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
        public float MaxHealth
        {
            get
            {
                return _maxHealth;
            }
        }
        public float MinusHealth
        {
            get
            {
                return _minusHealth;
            }
        }

        public GameStage Game
        {
            get { return _game; }
        }
    }
}
