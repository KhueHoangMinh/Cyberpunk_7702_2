using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace Cyberpunk77022
{
    public class Explosion
    {
        GameStage _game;
        Camera _camera;
        float _rad;
        float _maxrad;
        Point2D _pos;
        Color _color;
        float _expand = 5;
        float _a = 0.9f;
        float _angle;
        float delta;
        float beta;
        float sinAngle;
        float cosAngle;

        public Explosion(GameStage game,Camera camera,float rad, float maxrad, Point2D pos, Color color)
        {
            _game = game;
            _camera = camera;
            _rad = rad;
            _maxrad = maxrad;
            _pos = pos;
            _color = color;
            _color.A = (float)0.8;
            _angle = new Random().Next(0, 180);
            delta = (float)((Math.Sqrt(2) * _rad / 2));
            beta = (float)(_angle - Math.Atan(1));
            sinAngle = (float)Math.Sin(_angle);
            cosAngle = (float)Math.Cos(_angle);
        }
        public Explosion(GameStage game, Camera camera, float rad, float maxrad, Point2D pos, Color color, float a, float expand)
        {
            _game = game;
            _camera = camera;
            _rad = rad;
            _maxrad = maxrad;
            _pos = pos;
            _color = color;
            _color.A = (float)a;
            _expand = expand;
            _angle = new Random().Next(0, 180);
            delta = (float)((Math.Sqrt(2) * _rad / 2));
            beta = (float)(_angle - Math.Atan(1));
            sinAngle = (float)Math.Sin(_angle);
            cosAngle = (float)Math.Cos(_angle);
        }

        public Quad calQuad()
        {
            delta = (float)((Math.Sqrt(2) * _rad / 2));
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

        public void Update()
        {
            if (_color.A >= 0.01)
            {
                _color.A -= (float)0.01;
                _rad += (float)_expand;
                _expand *= _a;
            } else
            {
                _game.RemoveExplosion();
            }
        }

        public void Draw()
        {
            SplashKit.FillQuad(_color, calQuad());
            //SplashKit.FillCircle(_color, _pos.X - _camera.Pos.X, _pos.Y - _camera.Pos.Y, _rad);
        }

        public Color GetColor
        {
            get { return _color; }
        }
    }
}
