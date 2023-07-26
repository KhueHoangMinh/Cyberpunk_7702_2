using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
        string _dying;
        EnemyState _state;
        //float _aX;
        //float _aY;
        public Enemy(GameStage game, Camera camera, Point2D pos, float sizeX, float sizeY, Color color, string dying) : base(camera, pos, sizeX, sizeY, color, true, 0, 0)
        {
            _manager = game.Manager;
            _game = game;
            _EnemyGun = new Pistol1(_game.Manager.Window, 5, true);
            this.Gun.Game = this.Game;
            this.Gun.GunOf = this;
            _EnemyGun.SmokeDensity = 5;
            _camera = camera;
            _maxHealth = 100;
            _health = _maxHealth;
            dest = new Point2D() { X = new Random().Next(200, 300), Y = new Random().Next(200, 300) };
            _speed = (float)(new Random().NextDouble() + 0.2);
            _jumpTime = new Random().Next(30000000, 50000000);
            _fireRate = new Random().Next(30000000, 50000000);
            _jumpedAt = -_jumpTime;
            _firedAt = -_fireRate;
            _dying = dying;
            _state = new LivingState(this);
        }
        public Enemy(GameStage game, Camera camera, Point2D pos, float sizeX, float sizeY, Color color, string dying, float MaxHealth) : base(camera, pos, sizeX, sizeY, color, true, 0, 0)
        {
            _manager = game.Manager;
            _game = game;
            _EnemyGun = new Pistol1(_game.Manager.Window, 5);
            this.Gun.Game = this.Game;
            this.Gun.GunOf = this;
            _EnemyGun.SmokeDensity = 5;
            _camera = camera;
            _maxHealth = MaxHealth;
            _health = _maxHealth;
            dest = new Point2D() { X = new Random().Next(200, 300), Y = new Random().Next(200, 300) };
            _speed = (float)(new Random().NextDouble() + 0.2);
            _jumpTime = new Random().Next(30000000, 50000000);
            _fireRate = new Random().Next(30000000, 50000000);
            _jumpedAt = -_jumpTime;
            _firedAt = -_fireRate;
            _dying = dying;
            _state = new LivingState(this);
        }

        public override void CollideTop(Object @object)
        {
            this.Pos = new Point2D() { X = this.Pos.X, Y = @object.Bottom + (-this.Top + this.Pos.Y - 1) };
            if (this.VelY < 0)
            {
                if(_alive)
                {
                    this.VelY = 0;
                } else
                {
                    this.VelY = -this.VelY * 0.35f;
                }
            }
            if (this.Collide == "no") this.Collide = "top";
        }
        public override void CollideBottom(Object @object)
        {
            this.Pos = new Point2D() { X = this.Pos.X, Y = @object.Top - (this.Bottom - this.Pos.Y) + 1 };
            if (this.VelY > 0)
            {
                if (_alive)
                {
                    this.VelY = 0;
                }
                else
                {
                    this.VelY = -this.VelY * 0.35f;
                }
            }
            this.VelX *= 0.98f;
            _jumped = false;
            if (this.Collide == "no") this.Collide = "bottom";
        }
        public override void CollideRight(Object @object)
        {
            _jumped = false;
            this.Pos = new Point2D() { X = @object.Left + (-this.Right + this.Pos.X), Y = this.Pos.Y - 1 };
            if (this.VelX > 0)
            {
                if (_alive)
                {
                    this.VelX = 0;
                }
                else
                {
                    this.VelX = -this.VelX * 0.65f;
                }
            }
            if (this.Collide == "no") this.Collide = "right";
        }
        public override void CollideLeft(Object @object)
        {
            _jumped = false;
            this.Pos = new Point2D() { X = @object.Right - (this.Left - this.Pos.X), Y = this.Pos.Y + 1 };
            if (this.VelX < 0)
            {
                if (_alive)
                {
                    this.VelX = 0;
                }
                else
                {
                    this.VelX = -this.VelX * 0.65f;
                }
            }
        }
        public override void Update()
        {
            if(this.Pos.Y > 3000)
            {
                _health = -1;
            }
            if(_health < 0 && _alive)
            {
                _alive = false;
                this.GravityEffect = true;
                _state = new DyingState(this);
                SplashKit.SoundEffectNamed(_dying).Play();
                this.Color = Color.DarkGray;
                _game.AddCoin(new Coin(this.Game, this.Pos, this.VelX * 0.5f, this.VelY * 0.5f));
                //_game.RemoveEnemy(this);
            }
            _state.Behaviour();
            if (this.Color.A <= 0)
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
            this.Gravity();
            this.MoveObject(_game.Grounds);


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
            if (bullet is RPGBullet)
            {
                float scale = ((bullet as RPGBullet).ExplodeRange - (float)Math.Sqrt((this.Pos.X - bullet.Pos.X) * (this.Pos.X - bullet.Pos.X) + (this.Pos.Y - bullet.Pos.Y) * (this.Pos.Y - bullet.Pos.Y)))/ (bullet as RPGBullet).ExplodeRange;

                _health -= (int)(scale * bullet.Damage);
                if (_health >= 0)
                {
                    this.VelX += 10.0f * bullet.VelX * scale;
                    this.VelY += 10.0f * bullet.VelY * scale;
                    this.VelY -= 10;
                }
                else
                {
                    this.VelX += 30.0f * bullet.VelX * scale;
                    this.VelY += 30.0f * bullet.VelY * scale;
                    this.VelY -= 15;
                }
            } else
            {
                
                _health -= bullet.Damage;
                if (_health >= 0)
                {
                    this.VelX += 0.2f * bullet.VelX;
                    this.VelY += 0.2f * bullet.VelY;
                    this.VelY -= 3;
                }
                else
                {
                    this.VelX += 1.0f * bullet.VelX;
                    this.VelY += 1.0f * bullet.VelY;
                    this.VelY -= 10;
                }
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

        public long FiredAt
        {
            get { return _firedAt; }
            set { _firedAt = value; }
        }

        public long FireRate
        {
            get { return _fireRate; }
        }
    }

    public class NormalEnemy : Enemy
    {
        public NormalEnemy(GameStage game, Camera camera, Point2D pos) : base(game, camera, pos, 60, 60, Color.Red,"dying1") { 
        
        }

        public override void Behaviour()
        {
            if (this.Game.GetPlayer.Pos.X < this.Pos.X)
            {
                if (-this.Game.GetPlayer.Pos.X + this.Pos.X > this.Dest.X)
                {
                    this.VelX += -this.Speed;
                }
                else if (-this.Game.GetPlayer.Pos.X + this.Pos.X < this.Dest.X)
                {
                    this.VelX += this.Speed;
                }
            }
            else if (this.Game.GetPlayer.Pos.X > this.Pos.X)
            {
                if (this.Game.GetPlayer.Pos.X - this.Pos.X > this.Dest.X)
                {
                    this.VelX += this.Speed;
                }
                else if (this.Game.GetPlayer.Pos.X - this.Pos.X < this.Dest.X)
                {
                    this.VelX += -this.Speed;
                }
            }
            if (DateTime.UtcNow.Ticks - this.JumpedAt >= this.JumpTime && !this.Jumped && new Random().Next(0, 19) < 5)
            {
                this.JumpedAt = DateTime.UtcNow.Ticks;
                this.VelY += -10;
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

            this.Gun.Update(this.Game.GetPlayer.Pos);
            if (DateTime.UtcNow.Ticks - this.FiredAt >= this.FireRate && new Random().Next(0, 19) < 5)
            {
                this.FiredAt = DateTime.UtcNow.Ticks;
                this.Gun.Shoot();
            }
        }
    }

    public class FlyEnemy : Enemy
    {
        public FlyEnemy(GameStage game, Camera camera, Point2D pos) : base(game, camera, pos, 50, 50, Color.Orange, "dying2", 60)
        {
            this.Gun = new Rifle1(this.Game.Manager.Window, 8, true);
            this.Gun.Game = this.Game;
            this.Gun.GunOf = this;
            this.GravityEffect = false;
        }
        public override void Behaviour()
        {
            double tempVelX = 0;
            double tempVelY = 0;
            if (this.Game.GetPlayer.Pos.X < this.Pos.X)
            {
                if (-this.Game.GetPlayer.Pos.X + this.Pos.X > this.Dest.X)
                {
                    tempVelX = -1;
                }
                else if (-this.Game.GetPlayer.Pos.X + this.Pos.X < this.Dest.X)
                {
                    tempVelX = 1;
                }
            }
            else if (this.Game.GetPlayer.Pos.X > this.Pos.X)
            {
                if (this.Game.GetPlayer.Pos.X - this.Pos.X > this.Dest.X)
                {
                    tempVelX = 1;
                }
                else if (this.Game.GetPlayer.Pos.X - this.Pos.X < this.Dest.X)
                {
                    tempVelX = -1;
                }
            }
            if (this.Game.GetPlayer.Pos.Y < this.Pos.Y)
            {
                if (-this.Game.GetPlayer.Pos.Y + this.Pos.Y > this.Dest.Y)
                {
                    tempVelY = -1;
                }
                else if (-this.Game.GetPlayer.Pos.Y + this.Pos.Y < this.Dest.Y)
                {
                    tempVelY = 1;
                }
            }
            else if (this.Game.GetPlayer.Pos.Y > this.Pos.Y)
            {
                if (this.Game.GetPlayer.Pos.Y - this.Pos.Y > this.Dest.Y)
                {
                    tempVelY = 1;
                }
                else if (this.Game.GetPlayer.Pos.Y - this.Pos.Y < this.Dest.Y)
                {
                    tempVelY = -1;
                }
            }
            this.Pos = new Point2D() { X = this.Pos.X + tempVelX, Y = this.Pos.Y + tempVelY };
            this.Gun.Update(this.Game.GetPlayer.Pos);
            if (DateTime.UtcNow.Ticks - this.FiredAt >= this.FireRate && new Random().Next(0, 19) < 5)
            {
                this.FiredAt = DateTime.UtcNow.Ticks;
                this.Gun.Shoot();
            }
        }
    }
    public class BigEnemy : Enemy
    {
        public BigEnemy(GameStage game, Camera camera, Point2D pos) : base(game, camera, pos, 100, 100, Color.LightBlue, "dying3", 300)
        {
            this.Gun = new Shotgun1(this.Game.Manager.Window, 3, true);
            this.Gun.Game = this.Game;
            this.Gun.GunOf = this;
        }

        public override void Behaviour()
        {
            if (this.Game.GetPlayer.Pos.X < this.Pos.X)
            {
                if (-this.Game.GetPlayer.Pos.X + this.Pos.X > this.Dest.X)
                {
                    this.VelX += -this.Speed;
                }
                else if (-this.Game.GetPlayer.Pos.X + this.Pos.X < this.Dest.X)
                {
                    this.VelX += this.Speed;
                }
            }
            else if (this.Game.GetPlayer.Pos.X > this.Pos.X)
            {
                if (this.Game.GetPlayer.Pos.X - this.Pos.X > this.Dest.X)
                {
                    this.VelX += this.Speed;
                }
                else if (this.Game.GetPlayer.Pos.X - this.Pos.X < this.Dest.X)
                {
                    this.VelX += -this.Speed;
                }
            }
            if (DateTime.UtcNow.Ticks - this.JumpedAt >= this.JumpTime && !this.Jumped && new Random().Next(0, 19) < 5)
            {
                this.JumpedAt = DateTime.UtcNow.Ticks;
                this.VelY += -10;
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
            this.Gun.Update(this.Game.GetPlayer.Pos);
            if (DateTime.UtcNow.Ticks - this.FiredAt >= this.FireRate && new Random().Next(0, 19) < 5)
            {
                this.FiredAt = DateTime.UtcNow.Ticks;
                this.Gun.Shoot();
            }
        }
    }
}
