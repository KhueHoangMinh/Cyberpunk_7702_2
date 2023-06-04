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
        public Enemy(GameStage game, Camera camera, Point2D pos, float sizeX, float sizeY, Color color) : base(camera, pos, sizeX, sizeY, color, true, 0, 0)
        {
            _manager = game.Manager;
            _game = game;
            _EnemyGun = new Gun(_game, _manager.Window, this, camera);
            _camera = camera;
            _maxHealth = 100;
            _health = _maxHealth;
            dest = new Point2D() { X = new Random().Next(200, 300), Y = new Random().Next(200, 300) };
            _speed = (float)(new Random().NextDouble() + 0.5);
            _jumpTime = new Random().Next(3000000, 5000000);
            _fireRate = new Random().Next(3000000, 5000000);
            _jumpedAt = -_jumpTime;
            _firedAt = -_fireRate;
        }
        public void Update(List<Ground> grounds, List<Bullet> bullets)
        {
            if(_health <= 0)
            {
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
                    if (this.VelY > 0) this.VelY = 0;
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
            VelX = 0;
            if (_game.GetPlayer.Pos.X < this.Pos.X)
            {
                if(-_game.GetPlayer.Pos.X + this.Pos.X > dest.X)
                {
                    this.VelX = -_speed;
                } else if(-_game.GetPlayer.Pos.X + this.Pos.X < dest.X)
                {
                    this.VelX = _speed;
                }
            }
            else if(_game.GetPlayer.Pos.X > this.Pos.X)
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
            if (DateTime.UtcNow.Ticks - _jumpedAt >= _jumpTime && !_jumped && new Random().Next(0,19) < 5)
            {
                _jumpedAt = DateTime.UtcNow.Ticks;
                this.VelY = -10;
                for (int i = 0; i < 3; i++)
                {
                    switch (collide)
                    {
                        case "bottom":
                            _game.AddExplosion(new Explosion(_game,_camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                                Y = (double)new Random().Next((int)this.Bottom - 10, (int)this.Bottom + 10),
                            }, Color.White));
                            break;

                        case "left":
                            _game.AddExplosion(new Explosion(_game,_camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
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
            } else
            {
                //_jumpedAt = DateTime.UtcNow.Ticks;
            }
            this.Pos = new Point2D() { X = this.Pos.X + this.VelX, Y = this.Pos.Y + this.VelY };
            _EnemyGun.Update(_game.GetPlayer.Pos);
            if(DateTime.UtcNow.Ticks - _firedAt >= _fireRate && new Random().Next(0, 19) < 5)
            {
                _firedAt = DateTime.UtcNow.Ticks;
                _EnemyGun.Shoot();
            } else
            {
                //_firedAt = DateTime.UtcNow.Ticks;
            }
        }

        public void DrawGun()
        {
            _EnemyGun.Draw();
        }

        public void DrawHealth()
        {
            SplashKit.FillRectangle(Color.Gray,this.Left - _camera.Pos.X, this.Top - 20 - _camera.Pos.Y, this.Right - this.Left, 10);
            SplashKit.FillRectangle(Color.Red, this.Left - _camera.Pos.X, this.Top - 20 - _camera.Pos.Y, (this.Right - this.Left)*_health/_maxHealth, 10);
        }
        public void GetHit(Bullet bullet)
        {
            _health -= bullet.Damage;
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
    }
}
