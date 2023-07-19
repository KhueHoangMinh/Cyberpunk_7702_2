using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Cyberpunk77022
{
    public abstract class Gun : ShopItem
    {
        Object _GunOf;
        Camera _camera;
        Window _window;
        SoundEffect singleshot;
        Bitmap _graphic;
        Point2D _pos;
        DrawingOptions drawingOptions;
        DrawingOptions shopDrawingOption;
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
        string _name;
        string _id;
        string _desc;
        int _price;
        bool purchased = false;
        bool _minimized = false;
        bool _enemyGun = false;

        public Gun(string name, string id, string desc, int price, GameStage game, Object GunOf, string bitmapName, string soundName, float DisplayWidth, float butt, float damage, float fireRate, float recoil)
        {
            _name = name;
            _id = id;
            _desc = desc;
            _game = game;
            _price = price;
            _window = game.Manager.Window;
            _GunOf = GunOf;
            _camera = game.Camera;
            singleshot = SplashKit.SoundEffectNamed(soundName);
            _graphic = SplashKit.BitmapNamed(bitmapName);
            _DisplayWidth = DisplayWidth;
            _butt = butt;
            _nozzleLength = _DisplayWidth - _butt;
            scale = (float)(_DisplayWidth / _graphic.Width);
            _pos = new Point2D() { X = (_graphic.Width - _DisplayWidth)/2, Y = (_graphic.Height - _graphic.Height*scale) / 2, };
            drawingOptions = new DrawingOptions()
            {
                Dest = _window,
                ScaleX = scale,
                ScaleY = scale,
                AnchorOffsetX = 0,
                AnchorOffsetY = 0,
                Angle = 0,
            }; 
            float shopScale = 300 * 1.0f / _graphic.Width;
            shopDrawingOption = new DrawingOptions()
            {
                Dest = game.Manager.Window,
                ScaleX = shopScale,
                ScaleY = shopScale,
            };
            _ShootTime = 999999999;
            _aimPoint = new Point2D();
            nozzle = new Point2D();
            _basePoint = new Point2D();
            _damage = damage;
            _fireRate = fireRate;
            _recoil = recoil;
        }

        public Gun(string name, string id, string desc, int price, Window window, string bitmapName, string soundName, float DisplayWidth, float butt, float damage, float fireRate, float recoil)
        {
            _name = name;
            _id = id;
            _desc = desc;
            _price = price;
            _GunOf = GunOf;
            singleshot = SplashKit.SoundEffectNamed(soundName);
            _graphic = SplashKit.BitmapNamed(bitmapName);
            _DisplayWidth = DisplayWidth;
            _butt = butt;
            _nozzleLength = _DisplayWidth - _butt;
            scale = (float)(_DisplayWidth / _graphic.Width);
            _pos = new Point2D() { X = (_graphic.Width - _DisplayWidth) / 2, Y = (_graphic.Height - _graphic.Height * scale) / 2, };
            drawingOptions = new DrawingOptions()
            {
                Dest = _window,
                ScaleX = scale,
                ScaleY = scale,
                AnchorOffsetX = 0,
                AnchorOffsetY = 0,
                Angle = 0,
            };
            float shopScale = 300 * 1.0f / _graphic.Width;
            shopDrawingOption = new DrawingOptions()
            {
                Dest = window,
                ScaleX = shopScale,
                ScaleY = shopScale,
            };
            _ShootTime = 999999999;
            _aimPoint = new Point2D();
            nozzle = new Point2D();
            _basePoint = new Point2D();
            _damage = damage;
            _fireRate = fireRate;
            _recoil = recoil;
        }

        public Gun(string name, string id, string desc, int price, Window window, string bitmapName, string soundName, float DisplayWidth, float butt, float damage, float fireRate, float recoil, bool enemys)
        {
            _name = name;
            _id = id;
            _desc = desc;
            _price = price;
            _GunOf = GunOf;
            singleshot = SplashKit.SoundEffectNamed(soundName);
            _graphic = SplashKit.BitmapNamed(bitmapName);
            _DisplayWidth = DisplayWidth;
            _butt = butt;
            _nozzleLength = _DisplayWidth - _butt;
            scale = (float)(_DisplayWidth / _graphic.Width);
            _pos = new Point2D() { X = (_graphic.Width - _DisplayWidth) / 2, Y = (_graphic.Height - _graphic.Height * scale) / 2, };
            drawingOptions = new DrawingOptions()
            {
                Dest = _window,
                ScaleX = scale,
                ScaleY = scale,
                AnchorOffsetX = 0,
                AnchorOffsetY = 0,
                Angle = 0,
            };
            float shopScale = 300 * 1.0f / _graphic.Width;
            shopDrawingOption = new DrawingOptions()
            {
                Dest = window,
                ScaleX = shopScale,
                ScaleY = shopScale,
            };
            _ShootTime = 999999999;
            _aimPoint = new Point2D();
            nozzle = new Point2D();
            _basePoint = new Point2D();
            _damage = damage;
            _fireRate = fireRate;
            _recoil = recoil;
            _minimized = enemys;
            _enemyGun = enemys;

            if (_enemyGun)
            {
                _DisplayWidth = _DisplayWidth / 2;
                _butt = _butt / 2;
                _nozzleLength = _DisplayWidth - _butt;
                scale = (float)(_DisplayWidth / _graphic.Width);
                _pos = new Point2D() { X = (_graphic.Width - _DisplayWidth) / 2, Y = (_graphic.Height - _graphic.Height * scale) / 2, };
                drawingOptions = new DrawingOptions()
                {
                    Dest = _window,
                    ScaleX = scale,
                    ScaleY = scale,
                    AnchorOffsetX = 0,
                    AnchorOffsetY = 0,
                    Angle = 0,
                };
            }
        }
        public virtual void Update(Point2D aimPoint)
        {
            if(_game != null)
            {
                _aimPoint = aimPoint;
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
                    _pos.X = -(_graphic.Width - _DisplayWidth) / 2 - _butt;
                    drawingOptions.Angle = (float)(360 / (Math.PI * 2)) * angle;
                    drawingOptions.FlipY = false;
                    drawingOptions.AnchorOffsetX = -_graphic.Width / 2 + _butt / scale;
                }
                else
                {
                    nozzle.X = _GunOf.Pos.X - _nozzleLength * ((float)Math.Sin(angle + Math.PI / 2));
                    nozzle.Y = _GunOf.Pos.Y + _nozzleLength * ((float)Math.Cos(angle + Math.PI / 2));
                    _pos.X = -(_graphic.Width - _DisplayWidth) / 2 - _DisplayWidth + _butt;
                    drawingOptions.Angle = -360 + (float)(360 / (Math.PI * 2)) * angle;
                    drawingOptions.FlipY = true;
                    drawingOptions.AnchorOffsetX = _graphic.Width / 2 - _butt / scale;
                }
                _pos.Y = -(_graphic.Height) / 2;
                _basePoint.X = _GunOf.Pos.X + _pos.X;
                _basePoint.Y = _GunOf.Pos.Y + _pos.Y;

                if (!smoking && DateTime.UtcNow.Ticks - _ShootTime >= _fireRate + 500000)
                {
                    smoking = true;
                }
                if (smoking && DateTime.UtcNow.Ticks - _ShootTime <= 40000000 && new Random().Next(1, 100) <= _smokeDensity)
                {
                    _initVelX = (float)(5 * ((float)Math.Sin(angle + Math.PI / 2)));
                    _initVelY = (float)(5 * ((float)Math.Cos(angle + Math.PI / 2)));
                    if (this.Reverse)
                    {
                        _initVelX *= -1;
                    }
                    else
                    {
                        _initVelY *= -1;
                    }
                    _game.AddSmoke(new Smoke(_game, _camera, new Random().Next(2, 3), new Random().Next(20, 50), new Point2D()
                    {
                        X = (double)new Random().Next((int)nozzle.X - 10, (int)nozzle.X + 10),
                        Y = (double)new Random().Next((int)nozzle.Y - 10, (int)nozzle.Y + 10),
                    }, Color.White, _initVelX, _initVelY));
                }
                else if (DateTime.UtcNow.Ticks - _ShootTime > 8000000)
                {
                    smoking = false;
                }
                _shock = _shock * (float)0.9;
            }
        }
        public virtual void Draw()
        {
            SplashKit.DrawBitmap(_graphic,_basePoint.X - _camera.Pos.X, _basePoint.Y - _camera.Pos.Y, drawingOptions);
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
                Color color = Color.White;
                if (!enemyGun)
                {
                    color = Color.Random();
                }
                this.Game.AddExplosion(new Explosion(_game, _camera, new Random().Next(2, 5), 35, new Point2D()
                {
                    X = (double)new Random().Next((int)nozzle.X - 10, (int)nozzle.X + 10),
                    Y = (double)new Random().Next((int)nozzle.Y - 10, (int)nozzle.Y + 10),
                }, color, 0.3f, 5));
                this.Game.AddExplosion(new Explosion(_game, _camera, new Random().Next(5, 10), 35, new Point2D()
                {
                    X = (double)new Random().Next((int)nozzle.X - 10, (int)nozzle.X + 10),
                    Y = (double)new Random().Next((int)nozzle.Y - 10, (int)nozzle.Y + 10),
                }, color, 0.2f, 30));
                this.Game.AddExplosion(new Explosion(_game, _camera, new Random().Next(15, 20), 100, new Point2D()
                {
                    X = (double)new Random().Next((int)nozzle.X - 10, (int)nozzle.X + 10),
                    Y = (double)new Random().Next((int)nozzle.Y - 10, (int)nozzle.Y + 10),
                }, color, 0.1f, 50));
                this.ShootAction(color);
            }
        }

        public virtual void ShootAction(Color color)
        {
            if(_game != null)
            {
                Bullet NewBullet = new NormalBullet(_game, this, 800, _speed, _damage, color);
                if (enemyGun)
                {
                    NewBullet.Speed = 30;
                }
                NewBullet.Color = color;
                _game.AddBullet(NewBullet);
            }
        }

        public void Graphic(float x, float y)
        {
            SplashKit.DrawBitmap(_graphic, x - _graphic.Width / 2, y- _graphic.Height / 2, shopDrawingOption);
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
            set { _GunOf = value; }
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
            set { 
                _game = value;
                _camera = _game.Camera;
                drawingOptions.Dest = Game.Manager.Window;
            }
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
        public string Name { get { return _name; } }
        public string ID { get { return _id; } }
        public string Description { get { return _desc; } }
        public int Price { get { return _price; } }
        public bool Purchased
        {
            get { return purchased; }
            set { purchased = value; }
        }

        public bool Minimized
        {
            get { return _minimized; }
        }

        public bool enemyGun
        {
            get { return _enemyGun; }
        }

        public float Speed
        {
            get { return _speed; }
        }

        public Bitmap Bitmap
        {
            get { return _graphic; }
        }
    }
}
