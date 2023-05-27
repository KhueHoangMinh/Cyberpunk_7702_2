using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        Camera _camera;


        public Object(Camera camera, Point2D pos, float sizeX, float sizeY, Color color, bool gravity, float velX, float velY)
        {
            _camera = camera;
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
            SplashKit.FillRectangle(_color, (float)_pos.X - _sizeX / 2 - _camera.Pos.X, (float)_pos.Y - _sizeY / 2 - _camera.Pos.Y, _sizeX, _sizeY);
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

        public string IsCollideAt(Object obj)
        {
            if(obj.Pos.X > this.Left - (obj.Right - obj.Left)/2 && obj.Pos.X < this.Right + (obj.Right - obj.Left) / 2 && obj.Pos.Y > this.Top - (obj.Bottom - obj.Top)/2 && obj.Pos.Y < this.Bottom + (obj.Bottom - obj.Top) / 2)
            {

                Object areaTop;
                Object areaBottom;
                Object areaLeft;
                Object areaRight;
                if (this.Top > obj.Top)
                {
                    areaTop = this;
                }
                else
                {
                    areaTop = obj;
                }

                if (this.Left > obj.Left)
                {
                    areaLeft = this;
                }
                else
                {
                    areaLeft = obj;
                }

                if (this.Bottom < obj.Bottom)
                {
                    areaBottom = this;
                }
                else
                {
                    areaBottom = obj;
                }

                if (this.Right < obj.Right)
                {
                    areaRight = this;
                }
                else
                {
                    areaRight = obj;
                }

                if (Math.Abs(areaTop.Top - areaBottom.Bottom) > Math.Abs(areaLeft.Left - areaRight.Right))
                {
                    if (areaLeft == obj)
                    {
                        return "right";
                    }
                    else
                    {
                        return "left";
                    }
                }
                else
                {
                    if (areaTop == obj)
                    {
                        return "bottom";
                    }
                    else
                    {
                        return "top";
                    }
                }
            } else
            {
                return "no";
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
