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
        Camera _camera;
        float _width;
        float _height;
        float _angle;
        Point2D _initPoint;
        Bullet _tracing;
        Point2D _Pos;
        Color _color;
        public Trace(Camera camera, Bullet tracing)
        {
            _camera = camera;
            _width = tracing.Width;
            _height = (float)0.1;
            _tracing = tracing;
            _angle = tracing.Angle;
            _initPoint = tracing.InitPos;
            _Pos = new Point2D() { X = (_tracing.Pos.X - _initPoint.X)/2, Y = (_tracing.Pos.Y - _initPoint.Y) / 2 };
            _color = Color.White;
            _color.A = (float)0.6;
        }

        public void Update()
        {
            if(_tracing != null)
            {
                _Pos = new Point2D() { X = _initPoint.X + (_tracing.Pos.X - _initPoint.X) / 2, Y = _initPoint.Y + (_tracing.Pos.Y - _initPoint.Y) / 2 };
                _height = (float)Math.Sqrt((_tracing.Pos.X - _initPoint.X) * (_tracing.Pos.X - _initPoint.X) + (_tracing.Pos.Y - _initPoint.Y) * (_tracing.Pos.Y - _initPoint.Y));
            }
            _width -= (float)0.05;
            if(_color.A >= (float)0.005) _color.A -= (float)0.005;
        }

        public void Draw()
        {
            SplashKit.FillQuad(_color, calQuad());
        }

        public Color GetColor
        {
            get { return _color; }
        }

        public Quad calQuad()
        {
            float delta = (float)((Math.Sqrt(_width * _width + _height * _height) / 2));
            float beta = (float)(_angle - Math.Atan(_width / _height));
            float x = (float)_Pos.X - delta * (float)Math.Cos(beta) - (float)_camera.Pos.X;
            float y = (float)_Pos.Y + delta * (float)Math.Sin(beta) - (float)_camera.Pos.Y;
            float sinAngle = (float)Math.Sin(_angle);
            float cosAngle = (float)Math.Cos(_angle);
            return new Quad()
            {
                Points = new Point2D[4] {
                    new Point2D() { X = x, Y = y },
                    new Point2D() {
                        X = x + _height * cosAngle,
                        Y = y - _height * sinAngle
                    },
                    new Point2D() {
                        X = x + _width * sinAngle,
                        Y = y + _width * cosAngle
                    },
                    new Point2D() {
                        X = x + _width * sinAngle + _height * cosAngle,
                        Y = y + _width * cosAngle - _height * sinAngle
                    }
                }
            };
        }
    }
}
