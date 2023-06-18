using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        float aX = 0;
        float aY = 0;
        public Enemy(GameStage game, Camera camera, Point2D pos, float sizeX, float sizeY, Color color) : base(camera, pos, sizeX, sizeY, color, true, 0, 0)
        {
            _manager = game.Manager;
            _game = game;
            _EnemyGun = new Pistol1(_game, this, 1);
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
        public void Update(List<Ground> grounds, List<Bullet> bullets)
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
            }
            if(this.Color.A <= 0)
            {
                for(int i = 0; i < 5; i++)
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
            string collide = "no";
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
            base.Gravity();
            aX /= 1.06f;
            aY /= 1.06f;
            VelX = 0;

            if (_alive)
            {
                if (_game.GetPlayer.Pos.X < this.Pos.X)
                {
                    if (-_game.GetPlayer.Pos.X + this.Pos.X > dest.X)
                    {
                        this.VelX = -_speed;
                    }
                    else if (-_game.GetPlayer.Pos.X + this.Pos.X < dest.X)
                    {
                        this.VelX = _speed;
                    }
                }
                else if (_game.GetPlayer.Pos.X > this.Pos.X)
                {
                    if (_game.GetPlayer.Pos.X - this.Pos.X > dest.X)
                    {
                        this.VelX = _speed;
                    }
                    else if (_game.GetPlayer.Pos.X - this.Pos.X < dest.X)
                    {
                        this.VelX = -_speed;
                    }
                }
                if (DateTime.UtcNow.Ticks - _jumpedAt >= _jumpTime && !_jumped && new Random().Next(0, 19) < 5)
                {
                    _jumpedAt = DateTime.UtcNow.Ticks;
                    this.VelY = -10;
                    for (int i = 0; i < 3; i++)
                    {
                        switch (collide)
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
                else
                {
                    //_jumpedAt = DateTime.UtcNow.Ticks;
                }
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
            _health -= bullet.Damage;
            _minusHealth -= _health;
            aX = 10 * bullet.VelX / bullet.Speed;
            aY = 10 * bullet.VelY / bullet.Speed;
            aY -= 10;
            //this.Pos = new Point2D() { X = this.Pos.X + bullet.VelX, Y = this.Pos.Y + bullet.VelY };
        }
        public float Health
        {
            get
            {
                return _health;
            }
        }
    }

    public class NormalEnemy : Enemy
    {
        public NormalEnemy(GameStage game, Camera camera, Point2D pos, float sizeX, float sizeY, Color color) : base(game, camera, pos, sizeX, sizeY, color) { 
        
        }

        public override void Behaviour()
        {
            throw new NotImplementedException();
        }
    }

    public class FlyEnemy : Enemy
    {
        public FlyEnemy(GameStage game, Camera camera, Point2D pos, float sizeX, float sizeY, Color color) : base(game, camera, pos, sizeX, sizeY, color)
        {

        }
        public override void Behaviour()
        {
            throw new NotImplementedException();
        }
    }
}
