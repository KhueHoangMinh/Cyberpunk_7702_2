using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cyberpunk77022
{
    public abstract class Bullet
    {
        GameStage _game;
        float _angle;
        Point2D _initPos;
        Color _color;
        Camera _camera;
        Gun _gun;
        float _VelX;
        float _VelY;
        float _speed;
        Point2D _Pos;
        float _width = 10;
        float _height = 40;
        float delta;
        float beta;
        float sinAngle;
        float cosAngle;
        bool _isCollided = false;
        float _range = 800;
        float _damage;
        bool _pierce = false;
        List<Enemy> _hitted;


        protected float checkUnitX;
        protected float checkUnitY;

        Trace _trace;

        public Bullet(GameStage game, Gun gun, float range, float speed, float damage)
            
        {
            _game = game;
            _color = Color.Random();
            _camera = game.Camera;
            _gun = gun;
            _range = range;
            _speed = speed;
            _angle = (float)Math.PI * 2 - gun.Angle;
            sinAngle = (float)Math.Sin(_angle);
            cosAngle = (float)Math.Cos(_angle);
            _VelX = (float)(speed * ((float)Math.Sin(_angle + Math.PI / 2)));
            _VelY = (float)(speed * ((float)Math.Cos(_angle + Math.PI / 2)));
            checkUnitX = 3.0f * ((float)Math.Sin(_angle + Math.PI / 2));
            checkUnitY = 3.0f * ((float)Math.Cos(_angle + Math.PI / 2));
            if (gun.Reverse)
            {
                _VelX *= -1;
                _VelY *= -1;
                checkUnitX *= -1;
                checkUnitY *= -1;
            }
            _initPos = gun.Nozzle;
            _Pos = _initPos;
            delta = (float)((Math.Sqrt(_width * _width + _height * _height) / 2));
            beta = (float)(_angle - Math.Atan(_width / _height));
            _damage = damage;
        }



        public virtual void CheckCollide()
        {
            if ((this.Pos.X - _initPos.X) * (this.Pos.X - _initPos.X) + (this.Pos.Y - _initPos.Y) * (this.Pos.Y - _initPos.Y) > _range * _range)
            {
                this.IsCollided = true;
                _game.RemoveBullet(this);
                Explode();
            }

            for (int j = 0; j < _game.Grounds.Count; j++)
            {
                if (_game.Grounds[j].IsCollided(this.Pos))
                {
                    this.IsCollided = true;
                    SplashKit.SoundEffectNamed("hit").Play();
                    _game.RemoveBullet(this);
                    Explode();
                    break;
                }
            }

            
            for (int j = 0; j < _game.Enemies.Count; j++)
            {
                if (_game.Enemies[j].IsCollided(this.Pos) && this.Gun.GunOf is Player)
                {
                    if(_pierce)
                    {
                        if(!_hitted.Contains(_game.Enemies[j]))
                        {
                            _hitted.Add(_game.Enemies[j]);

                            SplashKit.SoundEffectNamed("hit").Play();
                            _game.Enemies[j].GetHit(this);
                            Explode();
                        }
                    } else
                    {
                        this.IsCollided = true;
                        SplashKit.SoundEffectNamed("hit").Play();
                        _game.Enemies[j].GetHit(this);
                        _game.RemoveBullet(this);
                        Explode();
                    }
                    break;
                }
            }


            if (_game.GetPlayer.IsCollided(this.Pos) && this.Gun.GunOf is Enemy)
            {
                this.IsCollided = true;
                SplashKit.SoundEffectNamed("hit").Play();
                _game.GetPlayer.GetHit(this);
                _game.RemoveBullet(this);
                Explode();
            }
        }

        public virtual void Explode()
        {
            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(15, 20), 3000, new Point2D()
            {
                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                Y = (double)new Random().Next((int)this.Pos.Y - 20, (int)this.Pos.Y + 20),
            }, this.Color, 0.2f, 30));
        }

        public virtual void MoveBullet()
        {
            float StartX = (float)_Pos.X;
            float StartY = (float)_Pos.Y;
            while (Math.Abs(_VelX) > Math.Abs(_Pos.X - StartX) && Math.Abs(_VelY) > Math.Abs(_Pos.Y - StartY))
            {
                _Pos.X += checkUnitX;
                _Pos.Y += checkUnitY;
                CheckCollide();
                if (this.IsCollided)
                {
                    break;
                }
            }
            if (!this.IsCollided)
            {
                _Pos.X = StartX + _VelX;
                _Pos.Y = StartY + _VelY;
                CheckCollide();
            }
        }

        public virtual void Update()
        {
            MoveBullet();
        }

        public virtual void Draw()
        {
            //SplashKit.FillQuad(_color, calQuad());
            //SplashKit.FillCircle(_color, _Pos.X - _camera.Pos.X, _Pos.Y - _camera.Pos.Y, 5);
        }

        public Quad calQuad()
        {
            float x = (float)_Pos.X - delta * (float)Math.Cos(beta) - (float)_camera.Pos.X;
            float y = (float)_Pos.Y + delta * (float)Math.Sin(beta) - (float)_camera.Pos.Y;
            float heightxcos = _height * cosAngle;
            float heightxsin = _height * sinAngle;
            float widthxcos = _width * cosAngle;
            float widthxsin = _width * sinAngle;
            return new Quad()
            {
                Points = new Point2D[4] {
                    new Point2D() { X = x, Y = y},
                    new Point2D() {
                        X = x + heightxcos,
                        Y = y - heightxsin
                    },
                    new Point2D() {
                        X = x + widthxsin,
                        Y = y + widthxcos
                    },
                    new Point2D() {
                        X = x + widthxsin+ heightxcos,
                        Y = y + widthxcos - heightxsin
                    }
                }
            };
        }

        public GameStage Game
        {
            get { return _game; }
        }

        public bool IsCollided
        {
            get
            {
                return _isCollided;
            }
            set 
            {
                _isCollided = value; 
            }
        }

        public float VelX
        {
            get { return _VelX; }
            set { _VelX = value; }
        }
        public float VelY
        {
            get { return _VelY; }
            set { _VelY = value; }
        }

        public Point2D Pos
        {
            get { return _Pos; }
            set { _Pos = value; }
        }

        public Point2D InitPos
        {
            get { return _initPos; }
        }

        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public float Angle { 
            get { return _angle; } 
            set 
            { 
                _angle = value;
                if(_trace != null) _trace.ReCalAngle(_angle);
                sinAngle = (float)Math.Sin(_angle);
                cosAngle = (float)Math.Cos(_angle);
                _VelX = (float)(_speed * ((float)Math.Sin(_angle + Math.PI / 2)));
                _VelY = (float)(_speed * ((float)Math.Cos(_angle + Math.PI / 2)));
                if (_gun.Reverse)
                {
                    _VelX *= -1;
                    _VelY *= -1;
                }
            }
        }

        public float Damage
        {
            get
            {
                return _damage;
            }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Gun Gun
        {
            get
            {
                return _gun;
            }
        }

        public float Speed
        {
            get { return _speed; }
            set 
            { 
                _speed = value;
                _VelX = (float)(_speed * ((float)Math.Sin(_angle + Math.PI / 2)));
                _VelY = (float)(_speed * ((float)Math.Cos(_angle + Math.PI / 2)));
                if (_gun.Reverse)
                {
                    _VelX *= -1;
                    _VelY *= -1;
                }
            }
        }

        public Trace Trace
        {
            get { return _trace; }
            set { _trace = value; }
        }

        public bool Pierce
        {
            get { return _pierce; }
            set 
            { 
                _pierce = value; 
                if( _pierce )
                {
                    _hitted = new List<Enemy>();
                }
            }
        }
    }

    public class NormalBullet : Bullet
    {
        public NormalBullet(GameStage game, Gun gun, float range, float speed, float damage) : base(game, gun, range, speed, damage)
        {
            this.Trace = new Trace(game, game.Manager.Window, game.Camera, this);
            game.AddTrace(this.Trace);
        }
        public NormalBullet(GameStage game, Gun gun, float range, float speed, float damage, Color color) : base(game, gun, range, speed, damage)
        {
            this.Trace = new Trace(game, game.Manager.Window, game.Camera, this);
            this.Trace.GetColor = color;
            game.AddTrace(this.Trace);
        }
    }
    public class SniperBullet : Bullet
    {
        public SniperBullet(GameStage game, Gun gun, float range, float speed, float damage) : base(game, gun, range, speed, damage)
        {
            this.Pierce = true;
        }
        public override void MoveBullet()
        {
            float StartX = (float)this.Pos.X;
            float StartY = (float)this.Pos.Y;
            while (Math.Abs(this.VelX) > Math.Abs(this.Pos.X - StartX) && Math.Abs(this.VelY) > Math.Abs(this.Pos.Y - StartY))
            {
                this.Pos = new Point2D() { X = this.Pos.X + checkUnitX, Y = this.Pos.Y + checkUnitY }; 
                if(new Random().Next(0, 9) < 3) Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(2, 8), new Random().Next(20, 30), new Point2D() { X = new Random().Next((int)this.Pos.X - 2, (int)this.Pos.X + 2), Y = new Random().Next((int)this.Pos.Y - 2, (int)this.Pos.Y + 2) }, Color.White, (float)(new Random().NextDouble() * 0.4 + 0.1), new Random().Next(2, 3)));

                CheckCollide();
                if (this.IsCollided)
                {
                    break;
                }
            }
            if (!this.IsCollided)
            {
                this.Pos = new Point2D() { X = StartX + this.VelX, Y = StartY + this.VelY };
                CheckCollide();
            }
        }
    }
    public class RPGBullet : Bullet
    {
        float _explodeRange = 250;
        public RPGBullet(GameStage game, Gun gun, float range, float speed, float damage) : base(game, gun, range, speed, damage)
        {
            this.Height = 50;
            this.Width = 20;
            this.Color = Color.DarkGray;
        }

        public override void Explode()
        {
            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(25, 75), 450, new Point2D()
            {
                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                Y = (double)new Random().Next((int)this.Pos.Y - 20, (int)this.Pos.Y + 20),
            }, this.Color, 0.6f, 20));
            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(20, 75), 350, new Point2D()
            {
                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                Y = (double)new Random().Next((int)this.Pos.Y - 20, (int)this.Pos.Y + 20),
            }, this.Color, 0.5f, 30));
            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(15, 75), 400, new Point2D()
            {
                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                Y = (double)new Random().Next((int)this.Pos.Y - 20, (int)this.Pos.Y + 20),
            }, this.Color, 0.7f, 25));
            this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(15, 20), 3000, new Point2D()
            {
                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                Y = (double)new Random().Next((int)this.Pos.Y - 20, (int)this.Pos.Y + 20),
            }, this.Color, 0.5f, 250));

            for (int i = 0; i < this.Game.Enemies.Count; i++)
            {
                float dist = (float)((this.Pos.X - this.Game.Enemies[i].Pos.X) * (this.Pos.X - this.Game.Enemies[i].Pos.X) + (this.Pos.Y - this.Game.Enemies[i].Pos.Y) * (this.Pos.Y - this.Game.Enemies[i].Pos.Y));
                if (dist < _explodeRange * _explodeRange)
                {
                    this.Game.Enemies[i].GetHit(this);
                }
            }

            this.Game.RemoveBullet(this);
        }

        public override void Update()
        {
            base.MoveBullet();
            for(int i = 0; i < 5; i++)
            {
                if(new Random().Next(0,20) < 4) Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(2,8), new Random().Next(20,30), new Point2D() { X = new Random().Next((int)this.Pos.X - 5, (int)this.Pos.X + 5), Y = new Random().Next((int)this.Pos.Y - 5, (int)this.Pos.Y + 5) }, Color.White, (float)(new Random().NextDouble()*0.3 + 0.1), new Random().Next(2,5)));
            }
        }

        public override void Draw()
        {
            SplashKit.FillQuad(this.Color, calQuad());
        }

        public float ExplodeRange
        {
            get { return _explodeRange; }
        }
    }
}
