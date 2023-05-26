using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public abstract class Object
    {
        Point2D _pos;
        float _sizeX;
        float _sizeY;
        Color _color;
        bool _gravity;
        float _g = (float)0.5;
        float _a = (float)0.1;
        float _velX = 0;
        float _velY = 0;


        public Object(Point2D pos, float sizeX, float sizeY, Color color, bool gravity, float velX, float velY)
        {
            _pos = pos;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _color = color;
            _gravity = gravity;
            _velX = velX;
            _velY = velY;
        }

        public void Gravity()
        {
            if(_gravity)
            {
                _velY += _g;
                _pos.Y += _velY;
            }
        }

        public void Draw()
        {
            SplashKit.FillRectangle(_color, (float)_pos.X - _sizeX / 2, (float)_pos.Y - _sizeY / 2, _sizeX, _sizeY);
        }

        public bool IsCollided(Point2D Pos)
        {
            if(Pos.X >= _pos.X - _sizeX/2 && Pos.X <= _pos.X + _sizeX/2 && Pos.Y >= _pos.Y - _sizeY / 2 && Pos.Y <= _pos.Y + _sizeY / 2)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public float VelX
        {
            get { return _velX; }
            set { _velX = value; }
        }
        public float VelY
        {
            get { return _velY; }
            set { _velY = value; }
        }

        public Point2D Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public float Top
        {
            get { return (float)_pos.Y - _sizeY/2; }
        }
        public float Right
        {
            get { return (float)_pos.X + _sizeX/2; }
        }
        public float Bottom
        {
            get { return (float)_pos.Y + _sizeY/2; }
        }
        public float Left
        {
            get { return (float)_pos.X - _sizeX/2; }
        }
    }
}
