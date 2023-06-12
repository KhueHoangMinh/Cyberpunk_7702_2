using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public Smoke(GameStage game, Camera camera, float rad, float maxrad, Point2D pos, Color color) : this(game,camera,rad,maxrad,pos,color,0,0)
        {
        }

        public void Update()
        {
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
        }

        public void Draw()
        {
            SplashKit.FillRectangle(_color, _pos.X - _camera.Pos.X - _rad / 2, _pos.Y - _camera.Pos.Y - _rad / 2, _rad, _rad);
        }

        public Color GetColor
        {
            get { return _color; }
        }
    }
}
