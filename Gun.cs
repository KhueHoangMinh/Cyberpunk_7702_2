using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public abstract class Gun
    {
        Object _GunOf;
        Camera _camera;
        Window _window;
        SoundEffect singleshot;
        Bitmap pistol;
        Point2D _pos;
        DrawingOptions drawingOptions;
        float _DisplayWidth;
        float scale;
        GameStage _game;
        float _shock = 0;
        long _ShootTime;
        float _fireRate = 5000000;
        bool smoking = false;
        Point2D nozzle;
        Point2D _aimPoint;
        Point2D _basePoint;
        float angle;
        float _range = 2000;
        float _damage;
        float _recoil;
        float _butt;
        float _nozzleLength;
        float _speed = 100;
        float _initVelX;
        float _initVelY;
        int _smokeDensity = 30;

        public Gun(GameStage game, Object GunOf, string bitmapName, string soundName, float DisplayWidth, float butt, float damage, float fireRate, float recoil)
        {
            _game = game;
            _window = game.Manager.Window;
            _GunOf = GunOf;
            _camera = game.Camera;
            singleshot = SplashKit.SoundEffectNamed(soundName);
            pistol = SplashKit.BitmapNamed(bitmapName);
            _DisplayWidth = DisplayWidth;
            _butt = butt;
            _nozzleLength = _DisplayWidth - _butt;
            scale = (float)(_DisplayWidth / pistol.Width);
            _pos = new Point2D() { X = (pistol.Width - _DisplayWidth)/2, Y = (pistol.Height - pistol.Height*scale) / 2, };
            drawingOptions = new DrawingOptions()
            {
                Dest = _window,
                ScaleX = scale,
                ScaleY = scale,
                AnchorOffsetX = -pistol.Width/2,
                AnchorOffsetY = 0,
                Angle = 0,
            };
            _ShootTime = 999999999;
            _aimPoint = new Point2D();
            nozzle = new Point2D();
            _basePoint = new Point2D();
            _damage = damage;
            _fireRate = fireRate;
            _recoil = recoil;
        }
        public virtual void Update(Point2D aimPoint)
        {
            _aimPoint = aimPoint;
            //float a = (float)(_aimPoint.X - _GunOf.Pos.X);
            //float b = (float)(_aimPoint.Y - _GunOf.Pos.Y);
            //float c = (float)Math.Sqrt(a * a + b * b);
            //nozzle = new Point2D() { X = _GunOf.Pos.X + _nozzleLength * a / c, Y = _GunOf.Pos.Y + _nozzleLength * b / c };
            if (_aimPoint.X != _GunOf.Pos.X)
            {
                angle = (float)Math.Atan((_aimPoint.Y - _GunOf.Pos.Y) / (_aimPoint.X - _GunOf.Pos.X)) + _shock;
            }
            else
            {
                if (_aimPoint.Y > _GunOf.Pos.Y)
                {
                    angle = -(float)Math.PI / 2 + _shock;
                }
                else
                {
                    angle = (float)Math.PI / 2 + _shock;
                }
            }
            if (_aimPoint.X > _GunOf.Pos.X)
            {
                nozzle.X = _GunOf.Pos.X + _nozzleLength * ((float)Math.Sin(angle + Math.PI / 2));
                nozzle.Y = _GunOf.Pos.Y - _nozzleLength * ((float)Math.Cos(angle + Math.PI / 2));
                drawingOptions.Angle = (float)(360 / (Math.PI * 2)) * angle;
                drawingOptions.FlipY = false;
                drawingOptions.AnchorOffsetX = (-pistol.Width + _butt) / 2;
                _pos.X = (pistol.Width - _DisplayWidth) / 2 + _butt;
                _basePoint.X = _GunOf.Pos.X - _pos.X;
                _basePoint.Y = _GunOf.Pos.Y - _pos.Y - pistol.Height * scale / 2;
            }
            else
            {
                nozzle.X = _GunOf.Pos.X - _nozzleLength * ((float)Math.Sin(angle + Math.PI / 2));
                nozzle.Y = _GunOf.Pos.Y + _nozzleLength * ((float)Math.Cos(angle + Math.PI / 2));
                drawingOptions.Angle = -360 + (float)(360 / (Math.PI * 2)) * angle;
                drawingOptions.FlipY = true;
                drawingOptions.AnchorOffsetX = (pistol.Width - _butt) / 2;
                _pos.X = (pistol.Width - _DisplayWidth) / 2 - _butt;
                _basePoint.X = _GunOf.Pos.X - _pos.X - _DisplayWidth;
                _basePoint.Y = _GunOf.Pos.Y - _pos.Y - pistol.Height * scale / 2;
            }
            if (!smoking && DateTime.UtcNow.Ticks - _ShootTime >= _fireRate + 500000)
            { 
                smoking = true;
            }
            if (smoking && DateTime.UtcNow.Ticks - _ShootTime <= 40000000 && new Random().Next(1,100) <= _smokeDensity)
            {
                _initVelX = (float)(5 * ((float)Math.Sin(angle + Math.PI / 2)));
                _initVelY = (float)(5 * ((float)Math.Cos(angle + Math.PI / 2)));
                if (this.Reverse)
                {
                    _initVelX *= -1;
                }else
                {
                    _initVelY *= -1;
                }
                _game.AddSmoke(new Smoke(_game, _camera, new Random().Next(2, 3), new Random().Next(20, 50), new Point2D()
                {
                    X = (double)new Random().Next((int)nozzle.X - 10, (int)nozzle.X + 10),
                    Y = (double)new Random().Next((int)nozzle.Y - 10, (int)nozzle.Y + 10),
                }, Color.White,_initVelX,_initVelY));
            } else if(DateTime.UtcNow.Ticks - _ShootTime > 8000000)
            {
                smoking = false;
            }
            _shock = _shock*(float)0.9;
        }
        public virtual void Draw()
        {
            SplashKit.DrawBitmapOnWindow(_window, pistol,_basePoint.X - _camera.Pos.X, _basePoint.Y - _camera.Pos.Y, drawingOptions);
        }

        public virtual void Shoot()
        {
            if(DateTime.UtcNow.Ticks - _ShootTime >= _fireRate)
            {
                smoking = false;
                _ShootTime = DateTime.UtcNow.Ticks;
                singleshot.Play();
                if (_aimPoint.X > _GunOf.Pos.X)
                {
                    _shock -= _recoil;
                }
                else
                {
                    _shock += _recoil;
                }
                this.ShootAction();
            }
        }

        public virtual void ShootAction()
        {
            Bullet NewBullet = new Bullet(_game, this, 800, _speed, _damage);
            for (int i = 0; i < 3; i++)
            {
                _game.AddExplosion(new Explosion(_game, _camera, new Random().Next(8, 10), new Random().Next(30, 50), new Point2D()
                {
                    X = (double)new Random().Next((int)NewBullet.InitPos.X - 10, (int)NewBullet.InitPos.X + 10),
                    Y = (double)new Random().Next((int)NewBullet.InitPos.Y - 10, (int)NewBullet.InitPos.Y + 10),
                }, Color.Random()));
            }
            _game.AddBullet(NewBullet);
        }

        public Point2D BasePoint
        {
            get
            {
                return _GunOf.Pos;
            }
        }
        public Point2D AimPoint
        {
            get
            {
                return _aimPoint;
            }
        }

        public Object GunOf
        {
            get {
                return _GunOf;
            }
        }

        public Point2D Nozzle
        {
            get
            {
                return nozzle;
            }
        }

        public float Angle
        {
            get
            {
                return angle;
            }
        }

        public GameStage Game
        {
            get { return _game; }
        }

        public bool Reverse
        {
            get
            {
                if(_aimPoint.X > _GunOf.Pos.X)
                {
                    return false;
                } else
                {
                    return true;
                }
            }
        }

        public int SmokeDensity
        {
            get { return _smokeDensity; }
            set { _smokeDensity = value; }
        }

        public float Damage
        {
            get { return _damage; }
        }
    }
}
