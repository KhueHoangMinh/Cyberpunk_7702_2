using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cyberpunk77022
{
    public abstract class Enemy : Object
    {
        float _speed;
        bool _alive = true;
        bool _jumped = false;
        long _jumpedAt;
        long _firedAt;
        long _jumpTime;
        long _fireRate;
        Manager _manager;
        Gun _EnemyGun;
        GameStage _game;
        Camera _camera;
        Point2D dest;
        float _health;
        float _maxHealth;
        float _minusHealth = 0;
        string collide = "no";
        bool _gravity = true;

        float aX = 0;
        float aY = 0;
        public Enemy(GameStage game, Camera camera, Point2D pos, float sizeX, float sizeY, Color color) : base(camera, pos, sizeX, sizeY, color, true, 0, 0)
        {
            _manager = game.Manager;
            _game = game;
            _EnemyGun = new Pistol1(_game, this, 5);
            _EnemyGun.SmokeDensity = 5;
            _camera = camera;
            _maxHealth = 100;
            _health = _maxHealth;
            dest = new Point2D() { X = new Random().Next(200, 300), Y = new Random().Next(200, 300) };
            _speed = (float)(new Random().NextDouble() + 0.5);
            _jumpTime = new Random().Next(30000000, 50000000);
            _fireRate = new Random().Next(30000000, 50000000);
            _jumpedAt = -_jumpTime;
            _firedAt = -_fireRate;
        }
        public Enemy(GameStage game, Camera camera, Point2D pos, float sizeX, float sizeY, Color color, float MaxHealth) : base(camera, pos, sizeX, sizeY, color, true, 0, 0)
        {
            _manager = game.Manager;
            _game = game;
            _EnemyGun = new Pistol1(_game, this, 5);
            _EnemyGun.SmokeDensity = 5;
            _camera = camera;
            _maxHealth = MaxHealth;
            _health = _maxHealth;
            dest = new Point2D() { X = new Random().Next(200, 300), Y = new Random().Next(200, 300) };
            _speed = (float)(new Random().NextDouble() + 0.5);
            _jumpTime = new Random().Next(30000000, 50000000);
            _fireRate = new Random().Next(30000000, 50000000);
            _jumpedAt = -_jumpTime;
            _firedAt = -_fireRate;
        }
        public virtual void Update(List<Ground> grounds, List<Bullet> bullets)
        {
            if(_health < 0 && _alive)
            {
                _alive = false;
                this.Color = Color.DarkGray;
                //_game.RemoveEnemy(this);
            }
            if(!_alive)
            {
                Color pale = this.Color;
                pale.A -= 0.01f;
                this.Color = pale;
                if (this.Color.A <= 0.3)
                {
                    if (new Random().Next(0, 15) < 1)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            _game.AddSmoke(new Smoke(_game, _camera, new Random().Next(8, 15), new Random().Next(20, 50), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Left, (int)this.Right),
                                Y = (double)new Random().Next((int)this.Top, (int)this.Bottom),
                            }, Color.White, 0, 0));
                        }
                    }
                }
            }
            if(this.Color.A <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    _game.AddSmoke(new Smoke(_game, _camera, new Random().Next(8, 15), new Random().Next(20, 50), new Point2D()
                    {
                        X = (double)new Random().Next((int)this.Left, (int)this.Right),
                        Y = (double)new Random().Next((int)this.Top, (int)this.Bottom),
                    }, Color.White, 0, 0));
                }
                _game.RemoveEnemy(this);
            }
            _jumped = true;
            collide = "no";
            for (int i = 0; i < grounds.Count; i++)
            {
                string isCollide = this.IsCollideAt(grounds[i]);
                if (isCollide != "no") collide = isCollide;
                if (isCollide == "bottom")
                {
                    this.Pos = new Point2D() { X = this.Pos.X, Y = grounds[i].Top - (this.Bottom - this.Pos.Y) + 1 };
                    //if (this.VelY > 0 && this.VelY < 3)
                    //{
                    //    this.VelY = 0;
                    //}
                    //else if (this.VelY > 3)
                    //{
                    //    this.VelY = -this.VelY * 0.9f;
                    //}
                    if(this.VelY > 0) this.VelY = 0;
                    _jumped = false;
                }
                else
                if (isCollide == "top")
                {
                    this.Pos = new Point2D() { X = this.Pos.X, Y = grounds[i].Bottom + (-this.Top + this.Pos.Y - 1) };
                    if (this.VelY < 0) this.VelY = 0;
                }
                else
                if (isCollide == "right")
                {
                    _jumped = false;
                    this.Pos = new Point2D() { X = grounds[i].Left + (-this.Right + this.Pos.X), Y = this.Pos.Y - 1 };
                    if (this.VelX > 0) this.VelX = 0;
                }
                else
                if (isCollide == "left")
                {
                    _jumped = false;
                    this.Pos = new Point2D() { X = grounds[i].Right - (this.Left - this.Pos.X), Y = this.Pos.Y + 1 };
                    if (this.VelX < 0) this.VelX = 0;
                }
            }
            if(_gravity) base.Gravity();
            aX /= 1.06f;
            aY /= 1.06f;
            VelX = 0;

            if (_alive)
            {
                this.Behaviour();

                _EnemyGun.Update(_game.GetPlayer.Pos);
                if (DateTime.UtcNow.Ticks - _firedAt >= _fireRate && new Random().Next(0, 19) < 5)
                {
                    _firedAt = DateTime.UtcNow.Ticks;
                    _EnemyGun.Shoot();
                }
                else
                {
                    //_firedAt = DateTime.UtcNow.Ticks;
                }
            }
            this.Pos = new Point2D() { X = this.Pos.X + this.VelX + aX, Y = this.Pos.Y + this.VelY + aY };
        }

        public abstract void Behaviour();

        public void DrawGun()
        {
            if(_alive) _EnemyGun.Draw();
        }

        public void DrawHealth()
        {
            _minusHealth *= 0.98f;
            if (_alive)
            {
                SplashKit.FillRectangle(Color.Gray, this.Left - _camera.Pos.X, this.Top - 20 - _camera.Pos.Y, this.Right - this.Left, 10);
                SplashKit.FillRectangle(Color.Red, this.Left - _camera.Pos.X, this.Top - 20 - _camera.Pos.Y, (this.Right - this.Left) * _health / _maxHealth, 10);
                SplashKit.FillRectangle(Color.RGBAColor(255, 0, 0, 90), this.Left + (this.Right - this.Left) * _health / _maxHealth - _camera.Pos.X, this.Top - 20 - _camera.Pos.Y, (this.Right - this.Left) * _minusHealth / _maxHealth, 10);
            }
        }
        public void GetHit(Bullet bullet)
        {
            _minusHealth = _health;
            if (bullet is NormalBullet)
            {
                if (_health >= 0)
                {
                    aX = 10 * bullet.VelX / bullet.Speed;
                    aY = 10 * bullet.VelY / bullet.Speed;
                    aY -= 10;
                }
                else
                {
                    aX = 20 * bullet.VelX / bullet.Speed;
                    aY = 20 * bullet.VelY / bullet.Speed;
                    aY -= 20;
                }
                _health -= bullet.Damage;
            } else if (bullet is RPGBullet)
            {
                float scale = ((bullet as RPGBullet).ExplodeRange - (float)Math.Sqrt((this.Pos.X - bullet.Pos.X) * (this.Pos.X - bullet.Pos.X) + (this.Pos.Y - bullet.Pos.Y) * (this.Pos.Y - bullet.Pos.Y)))/ (bullet as RPGBullet).ExplodeRange;
                if (_health >= 0)
                {
                    aX = scale * 50 * bullet.VelX / bullet.Speed;
                    aY = scale * 50 * bullet.VelY / bullet.Speed;
                    aY -= scale * 50;
                }
                else
                {
                    aX = scale * 70 * bullet.VelX / bullet.Speed;
                    aY = scale * 70 * bullet.VelY / bullet.Speed;
                    aY -= scale * 70;
                }
                _health -= (int) (scale * bullet.Damage);
            }
            _minusHealth -= _health;
            if (_alive) _game.AddMinusHealth(new MinusHealth(_game, this, _minusHealth));
            //this.Pos = new Point2D() { X = this.Pos.X + bullet.VelX, Y = this.Pos.Y + bullet.VelY };
        }

        public Gun Gun
        {
            get { return _EnemyGun; }
            set { _EnemyGun = value; }
        }

        public bool Alive
        {
            get { return _alive; }
        }
        public float Health
        {
            get
            { return _health;}
            set { _health = value; }
        }

        public bool Gravity
        {
            get { return _gravity; }
            set { _gravity = value; }
        }

        public GameStage Game
        {
            get { return _game; }
        }

        public float Speed
        {
            get { return _speed; }
        }

        public Point2D Dest
        {
            get { return dest; }
        }

        public string Collide
        {
            get { return collide; }
            set { collide = value; }
        }

        public bool Jumped
        {
            get { return _jumped; }
            set { _jumped = value; }
        }

        public long JumpTime
        {
            get { return _jumpTime; }
            set{ _jumpTime = value; }
        }

        public long JumpedAt
        {
            get { return _jumpedAt; }
            set { _jumpedAt = value; }
        }
    }

    public class NormalEnemy : Enemy
    {
        public NormalEnemy(GameStage game, Camera camera, Point2D pos) : base(game, camera, pos, 60, 60, Color.Red) { 
        
        }

        public override void Behaviour()
        {
            if (this.Health < 0 && this.Alive)
            {
                this.Gravity = false;
            }
            if (this.Game.GetPlayer.Pos.X < this.Pos.X)
            {
                if (-this.Game.GetPlayer.Pos.X + this.Pos.X > this.Dest.X)
                {
                    this.VelX = -this.Speed;
                }
                else if (-this.Game.GetPlayer.Pos.X + this.Pos.X < this.Dest.X)
                {
                    this.VelX = this.Speed;
                }
            }
            else if (this.Game.GetPlayer.Pos.X > this.Pos.X)
            {
                if (this.Game.GetPlayer.Pos.X - this.Pos.X > this.Dest.X)
                {
                    this.VelX = this.Speed;
                }
                else if (this.Game.GetPlayer.Pos.X - this.Pos.X < this.Dest.X)
                {
                    this.VelX = -this.Speed;
                }
            }
            if (DateTime.UtcNow.Ticks - this.JumpedAt >= this.JumpTime && !this.Jumped && new Random().Next(0, 19) < 5)
            {
                this.JumpedAt = DateTime.UtcNow.Ticks;
                this.VelY = -10;
                for (int i = 0; i < 3; i++)
                {
                    switch (this.Collide)
                    {
                        case "bottom":
                            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                                Y = (double)new Random().Next((int)this.Bottom - 10, (int)this.Bottom + 10),
                            }, Color.White));
                            break;

                        case "left":
                            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Left - 20, (int)this.Left + 20),
                                Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                            }, Color.White));
                            break;

                        case "right":
                            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Right - 20, (int)this.Right + 20),
                                Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                            }, Color.White));
                            break;

                    }
                }
            }
            else
            {
                //_jumpedAt = DateTime.UtcNow.Ticks;
            }
        }
    }

    public class FlyEnemy : Enemy
    {
        public FlyEnemy(GameStage game, Camera camera, Point2D pos) : base(game, camera, pos, 50, 50, Color.Orange, 60)
        {
            this.Gun = new Rifle1(this.Game, this, 8);
        }
        public override void Behaviour()
        {
            if (this.Game.GetPlayer.Pos.X < this.Pos.X)
            {
                if (-this.Game.GetPlayer.Pos.X + this.Pos.X > this.Dest.X)
                {
                    this.VelX = -this.Speed;
                }
                else if (-this.Game.GetPlayer.Pos.X + this.Pos.X < this.Dest.X)
                {
                    this.VelX = this.Speed;
                }
            }
            else if (this.Game.GetPlayer.Pos.X > this.Pos.X)
            {
                if (this.Game.GetPlayer.Pos.X - this.Pos.X > this.Dest.X)
                {
                    this.VelX = this.Speed;
                }
                else if (this.Game.GetPlayer.Pos.X - this.Pos.X < this.Dest.X)
                {
                    this.VelX = -this.Speed;
                }
            }
            if (this.Game.GetPlayer.Pos.Y < this.Pos.Y)
            {
                if (-this.Game.GetPlayer.Pos.Y + this.Pos.Y > this.Dest.Y)
                {
                    this.VelY = -this.Speed;
                }
                else if (-this.Game.GetPlayer.Pos.Y + this.Pos.Y < this.Dest.Y)
                {
                    this.VelY = this.Speed;
                }
            }
            else if (this.Game.GetPlayer.Pos.Y > this.Pos.Y)
            {
                if (this.Game.GetPlayer.Pos.Y - this.Pos.Y > this.Dest.Y)
                {
                    this.VelY = this.Speed;
                }
                else if (this.Game.GetPlayer.Pos.Y - this.Pos.Y < this.Dest.Y)
                {
                    this.VelY = -this.Speed;
                }
            }
        }
    }
    public class BigEnemy : Enemy
    {
        public BigEnemy(GameStage game, Camera camera, Point2D pos) : base(game, camera, pos, 100, 100, Color.LightBlue, 300)
        {
            this.Gun = new Shotgun1(this.Game, this, 3);
        }

        public override void Behaviour()
        {
            if (this.Health < 0 && this.Alive)
            {
                this.Gravity = false;
            }
            if (this.Game.GetPlayer.Pos.X < this.Pos.X)
            {
                if (-this.Game.GetPlayer.Pos.X + this.Pos.X > this.Dest.X)
                {
                    this.VelX = -this.Speed;
                }
                else if (-this.Game.GetPlayer.Pos.X + this.Pos.X < this.Dest.X)
                {
                    this.VelX = this.Speed;
                }
            }
            else if (this.Game.GetPlayer.Pos.X > this.Pos.X)
            {
                if (this.Game.GetPlayer.Pos.X - this.Pos.X > this.Dest.X)
                {
                    this.VelX = this.Speed;
                }
                else if (this.Game.GetPlayer.Pos.X - this.Pos.X < this.Dest.X)
                {
                    this.VelX = -this.Speed;
                }
            }
            if (DateTime.UtcNow.Ticks - this.JumpedAt >= this.JumpTime && !this.Jumped && new Random().Next(0, 19) < 5)
            {
                this.JumpedAt = DateTime.UtcNow.Ticks;
                this.VelY = -10;
                for (int i = 0; i < 3; i++)
                {
                    switch (this.Collide)
                    {
                        case "bottom":
                            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                                Y = (double)new Random().Next((int)this.Bottom - 10, (int)this.Bottom + 10),
                            }, Color.White));
                            break;

                        case "left":
                            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Left - 20, (int)this.Left + 20),
                                Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                            }, Color.White));
                            break;

                        case "right":
                            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Right - 20, (int)this.Right + 20),
                                Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                            }, Color.White));
                            break;

                    }
                }
            }
            else
            {
                //_jumpedAt = DateTime.UtcNow.Ticks;
            }
        }
    }
}
