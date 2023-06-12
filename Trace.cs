using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Trace
    {
        GameStage _game;
        Window _window;
        Camera _camera;
        float _width;
        float _height;
        float _angle;
        Point2D _initPoint;
        Bullet _tracing;
        Point2D _Pos;
        Point2D _Dest;
        Color _color;
        float sinAngle;
        float cosAngle;
        float _maxHeight = 200;
        float _VelX;
        float _VelY;
        bool _reverse = false;

        float checkUnitX;
        float checkUnitY;
        public Trace(GameStage game, Window window, Camera camera, Bullet tracing)
        {
            _game = game;
            _window = window;
            _camera = camera;
            _width = tracing.Width;
            _height = (float)0.1;
            _tracing = tracing;
            _angle = tracing.Angle;
            _initPoint = tracing.InitPos;
            _Pos = new Point2D() { X = (_tracing.Pos.X - _initPoint.X)/2, Y = (_tracing.Pos.Y - _initPoint.Y) / 2 };
            _Dest = new Point2D();
            _color = Color.Random();
            _color.A = (float)0.4;
            sinAngle = (float)Math.Sin(_angle);
            cosAngle = (float)Math.Cos(_angle);
            _VelX = tracing.VelX * 0.2f;
            _VelY = tracing.VelY * 0.2f;
            _reverse = _tracing.Gun.Reverse;

            checkUnitX = 1 * ((float)Math.Sin(_angle + Math.PI / 2));
            checkUnitY = 1 * ((float)Math.Cos(_angle + Math.PI / 2));
            if (_reverse)
            {
                checkUnitX *= -1;
                checkUnitY *= -1;
            }
        }

        public void Update()
        {
            if(_tracing != null)
            {
                _Dest = _tracing.Pos;
            }
            _Pos = new Point2D() { X = _initPoint.X + (_Dest.X - _initPoint.X) / 2, Y = _initPoint.Y + (_Dest.Y - _initPoint.Y) / 2 };
            _height = (float)Math.Sqrt((_Dest.X - _initPoint.X) * (_Dest.X - _initPoint.X) + (_Dest.Y - _initPoint.Y) * (_Dest.Y - _initPoint.Y));
            //Point2D DestStop = _initPoint;
            //DestStop.X += _VelX;
            //DestStop.Y += _VelY;
            //while (Math.Abs(_VelX) > Math.Abs(_initPoint.X - DestStop.X) && Math.Abs(_VelY) > Math.Abs(_initPoint.Y - DestStop.Y))
            //{
            //    if (_height > 2)
            //    {
            //        _height = (float)Math.Sqrt((_Dest.X - _initPoint.X) * (_Dest.X - _initPoint.X) + (_Dest.Y - _initPoint.Y) * (_Dest.Y - _initPoint.Y));
            //        _initPoint.X += checkUnitX;
            //        _initPoint.Y += checkUnitY;
            //    }
            //}
            _initPoint.X += checkUnitX;
            _initPoint.Y += checkUnitY;

            if (_color.A >= (float)0.005)
            {
                _color.A -= (float)0.005;
            } else
            {
                _game.RemoveTrace();
            }
        }

        public void Draw()
        {
            SplashKit.FillTriangle(_color, calQuad());
        }

        public Color GetColor
        {
            get { return _color; }
        }

        public Triangle calQuad()
        {
            float delta = (float)((Math.Sqrt(_width * _width + _height * _height) / 2));
            float beta = (float)(_angle - Math.Atan(_width / _height));
            float heightxcos = _height * cosAngle;
            float heightxsin = _height * sinAngle;
            float widthxcos = _width * cosAngle;
            float widthxsin = _width * sinAngle;
            float x = (float)_Pos.X - delta * (float)Math.Cos(beta) - (float)_camera.Pos.X;
            float y = (float)_Pos.Y + delta * (float)Math.Sin(beta) - (float)_camera.Pos.Y;
            if (!_reverse)
            {
                return new Triangle()
                {
                    Points = new Point2D[3] {
                    //new Point2D() { X = x, Y = y},
                    new Point2D() {
                        X = (2*x + widthxsin)/2,
                        Y = (2*y + widthxcos)/2
                    },
                    new Point2D() {
                        X = x + heightxcos,
                        Y = y - heightxsin
                    },
                    new Point2D() {
                        X = x + widthxsin + heightxcos,
                        Y = y + widthxcos - heightxsin
                    }
                }
                };
            } else
            {
                return new Triangle()
                {
                    Points = new Point2D[3] {
                    new Point2D() { X = x, Y = y},
                    new Point2D() {
                        X = x + widthxsin,
                        Y = y + widthxcos
                    },
                    new Point2D() {
                        X = (2*x + widthxsin + 2*heightxcos)/2,
                        Y = (2*y + widthxcos - 2*heightxsin)/2
                    }
                }
                };

            }
        }
    }
}
