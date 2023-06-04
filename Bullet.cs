using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Bullet
    {
        GameStage _game;
        float _angle;
        Point2D _initPos;
        Color _color;
        Camera _camera;
        Gun _gun;
        float _VelX;
        float _VelY;
        Point2D _Pos;
        float _width = 10;
        float _height = 40;
        float delta;
        float beta;
        float sinAngle;
        float cosAngle;
        bool _isCollided = false;
        float range = 800;
        float _damage;

        public Bullet(GameStage game, Camera camera, Gun gun, float GunLength, float speed, float damage)
            
        {
            _game = game;
            _color = Color.Yellow;
            _camera = camera;
            _gun = gun;
            _color =  Color.Yellow;
            float a = (float)(gun.AimPoint.X - gun.BasePoint.X);
            float b = (float)(gun.AimPoint.Y - gun.BasePoint.Y);
            float c = (float)Math.Sqrt(a * a + b * b);
            _VelX = (float)(speed * a/c);
            _VelY = (float)(speed * b/c);
            _initPos = new Point2D() { X = gun.BasePoint.X + GunLength * a/c, Y = gun.BasePoint.Y + GunLength * b / c };
            _Pos = _initPos;
            _angle = (float)Math.PI * 2 - (float)Math.Atan(b / a);
            delta = (float)((Math.Sqrt(_width * _width + _height * _height) / 2));
            beta = (float)(_angle - Math.Atan(_width / _height));
            sinAngle = (float)Math.Sin(_angle);
            cosAngle = (float)Math.Cos(_angle);
            _damage = damage;
        }

        public void Update()
        {
            _Pos = new Point2D() { X = this.Pos.X + _VelX, Y = this.Pos.Y + _VelY };
            if((this.Pos.X - _initPos.X) * (this.Pos.X - _initPos.X) + (this.Pos.Y - _initPos.Y) * (this.Pos.Y - _initPos.Y) > range * range)
            {
                _game.RemoveBullet(this);
                _game.AddExplosion(new Explosion(_game, _camera, new Random().Next(20, 25), new Random().Next(40, 60), new Point2D()
                {
                    X = (double)new Random().Next((int)this.Pos.X - 10, (int)this.Pos.X + 10),
                    Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                }, Color.Random()));
            }
        }

        public void Draw()
        {
            SplashKit.FillQuad(_color, calQuad());
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
        }
        public float VelY
        {
            get { return _VelY; }
        }

        public Point2D Pos
        {
            get { return _Pos; }
        }

        public Point2D InitPos
        {
            get { return _initPos; }
        }

        public float Width
        {
            get { return _width; }
        }
        public float Height
        {
            get { return _height; }
        }

        public float Angle { get { return _angle; } }

        public float Damage
        {
            get
            {
                return _damage;
            }
        }

        public Gun Gun
        {
            get
            {
                return _gun;
            }
        }
    }
}
