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

        public Explosion(GameStage game,Camera camera,float rad, float maxrad, Point2D pos, Color color)
        {
            _game = game;
            _camera = camera;
            _rad = rad;
            _maxrad = maxrad;
            _pos = pos;
            _color = color;
            _color.A = (float)0.8;
        }

        public void Update()
        {
            if (_color.A >= 0.01)
            {
                _color.A -= (float)0.01;
            } else
            {
                _game.RemoveExplosion();
            }
            _rad += (float)1;
        }

        public void Draw()
        {
            SplashKit.FillRectangle(_color, _pos.X - _camera.Pos.X - _rad/2, _pos.Y - _camera.Pos.Y - _rad / 2, _rad, _rad);
        }

        public Color GetColor
        {
            get { return _color; }
        }
    }
}
