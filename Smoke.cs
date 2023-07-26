using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace Cyberpunk77022
{
    public class Smoke
    {
        GameStage _game;
        Camera _camera;
        float _rad;
        float _maxrad;
        Point2D _pos;
        Color _color;
        float _velY = 3;
        float _initVelX;
        float _initVelY;

        float _angle = 0;
        float delta;
        float beta;
        float sinAngle;
        float cosAngle;
        float rotSpeed;


        public Smoke(GameStage game, Camera camera, float rad, float maxrad, Point2D pos, Color color, float initVelX, float initVelY)
        {
            _game = game;
            _camera = camera;
            _rad = rad;
            _maxrad = maxrad;
            _pos = pos;
            _color = color;
            _color.A = (float)0.5;
            _initVelX = initVelX;
            _initVelY = initVelY;
            rotSpeed = (float)new Random().NextDouble()*0.1f - 0.05f;
        }

        public Smoke(GameStage game, Camera camera, float rad, float maxrad, Point2D pos, Color color) : this(game,camera,rad,maxrad,pos,color,0,0)
        {
        }

        public virtual void Update()
        {
            _angle += rotSpeed;
            _pos.X += _initVelX;
            _pos.Y += _initVelY;
            _pos.Y -= _velY;
            _velY = _velY * 0.98f;
            _initVelX *= 0.92f;
            _initVelY *= 0.92f;
            if (_color.A >= 0.01)
            {
                _color.A -= (float)0.01;
            } else
            {
                _game.RemoveSmoke();
            }
            _rad += (float)1.5;

            delta = (float)((Math.Sqrt(2) * _rad / 2));
            beta = (float)(_angle - Math.Atan(1));
            sinAngle = (float)Math.Sin(_angle);
            cosAngle = (float)Math.Cos(_angle);
        }

        public virtual void Draw()
        {
            SplashKit.FillQuad(_color, calQuad());
        }


        public Quad calQuad()
        {
            float x = (float)_pos.X - delta * (float)Math.Cos(beta) - (float)_camera.Pos.X;
            float y = (float)_pos.Y + delta * (float)Math.Sin(beta) - (float)_camera.Pos.Y;
            float heightxcos = _rad * cosAngle;
            float heightxsin = _rad * sinAngle;
            float widthxcos = _rad * cosAngle;
            float widthxsin = _rad * sinAngle;
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

        public Color GetColor
        {
            get { return _color; }
        }

        public GameStage Game
        {
            get { return _game; }
        }

        public Point2D Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public float Rad
        {
            get { return _rad; }   
            set { _rad = value; }
        }

        public float RotSpeed
        {
            get { return rotSpeed; }
        }

        public float InitVelX
        {
            get { return _initVelX; }
            set { _initVelX = value; }
        }
        public float InitVelY
        {
            get { return _initVelY; }
            set { _initVelY = value; }
        }
        public float VelY
        {
            get { return _velY; }
            set { _velY = value; }
        }

    }
}
