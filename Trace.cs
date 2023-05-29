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
        Window _window;
        Camera _camera;
        float _width;
        float _height;
        float _angle;
        Point2D _initPoint;
        Bullet _tracing;
        Point2D _Pos;
        Color _color;
        float sinAngle;
        float cosAngle;
        float _maxHeight = 200;
        float _VelX;
        float _VelY;
        public Trace(Window window, Camera camera, Bullet tracing)
        {
            _window = window;
            _camera = camera;
            _width = tracing.Width;
            _height = (float)0.1;
            _tracing = tracing;
            _angle = tracing.Angle;
            _initPoint = tracing.InitPos;
            _Pos = new Point2D() { X = (_tracing.Pos.X - _initPoint.X)/2, Y = (_tracing.Pos.Y - _initPoint.Y) / 2 };
            _color = Color.Random();
            _color.A = (float)0.4;
            sinAngle = (float)Math.Sin(_angle);
            cosAngle = (float)Math.Cos(_angle);
            _VelX = tracing.VelX * 0.2f;
            _VelY = tracing.VelY * 0.2f;
        }

        public void Update()
        {
            _Pos = new Point2D() { X = _initPoint.X + (_tracing.Pos.X - _initPoint.X) / 2, Y = _initPoint.Y + (_tracing.Pos.Y - _initPoint.Y) / 2 };
            _height = (float)Math.Sqrt((_tracing.Pos.X - _initPoint.X) * (_tracing.Pos.X - _initPoint.X) + (_tracing.Pos.Y - _initPoint.Y) * (_tracing.Pos.Y - _initPoint.Y));
            
            if((_height > _maxHeight || _tracing.IsCollided) && _height > 5)
            {
                    _initPoint.X += _VelX;
                    _initPoint.Y += _VelY;
            }

            if(_color.A >= (float)0.005) _color.A -= (float)0.005;
        }

        public void Draw()
        {
            SplashKit.FillTriangleOnWindow(_window, _color, calQuad());
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
            if (_VelX > 0)
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
