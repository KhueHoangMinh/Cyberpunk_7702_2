using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cyberpunk77022
{
    public abstract class Object
    {
        Point2D _pos;
        float _sizeX;
        float _sizeY;
        Color _color;
        bool _gravity;
        float _g = (float)1;
        float _a = (float)0.1;
        float _velX = 0;
        float _velY = 0;
        Camera _camera;
        bool _enabled = true;
        float checkScale = 1;
        float checkUnitX;
        float checkUnitY;
        string _collide = "no";


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
            checkScale = 3.0f / (float)Math.Sqrt(_velX*_velX + _velY*_velY);
            checkUnitX = _velX * checkScale;
            checkUnitY = _velY * checkScale;
        }

        public void Gravity()
        {
            if(_gravity)
            {
                _velY += _g;
            }
        }

        public virtual void CollideNone() { }
        public virtual void CollideTop(Object @object) { }
        public virtual void CollideRight(Object @object) { }
        public virtual void CollideBottom(Object @object) { }
        public virtual void CollideLeft(Object @object) { }

        public string CheckCollide(List<Ground> objects)
        {
            string collide = "no";
            for (int i = 0; i < objects.Count; i++)
            {
                string isCollide = IsCollideAt(objects[i]);

                if (isCollide == "bottom")
                {
                    CollideBottom(objects[i]);
                }
                else
                if (isCollide == "top")
                {
                    CollideTop(objects[i]);
                }
                else
                if (isCollide == "right")
                {
                    CollideRight(objects[i]);
                }
                else
                if (isCollide == "left")
                {
                    CollideLeft(objects[i]);
                }

                if (isCollide != "no")
                {
                    collide = isCollide;
                }

            }

            if(collide == "no")
            {
                CollideNone();
            }

            return collide;
        }


        public void MoveObject(List<Ground> objects)
        {
            string finalCollide = "no";
            float StartX = (float)_pos.X;
            float StartY = (float)_pos.Y;
            checkScale = 3.0f / (float)Math.Sqrt(_velX * _velX + _velY * _velY);
            checkUnitX = _velX * checkScale * 1.0f;
            checkUnitY = _velY * checkScale * 1.0f;
            while (Math.Abs(_velX) > Math.Abs(_pos.X - StartX) && Math.Abs(_velY) > Math.Abs(_pos.Y - StartY))
            {
                _pos.X += checkUnitX;
                _pos.Y += checkUnitY;
                string collide = CheckCollide(objects);
                if (collide != "no")
                {
                    finalCollide = collide;
                    break;
                }
            }
            if (finalCollide != "left" && finalCollide != "right")
            {
                _pos.X = StartX + _velX;
            }
            if (finalCollide != "top" && finalCollide != "bottom")
            {
                _pos.Y = StartY + _velY;
            }
            CheckCollide(objects);
        }

        public abstract void Update();

        public virtual void Draw()
        {
            if(_enabled) SplashKit.FillRectangle(_color, (float)_pos.X - _sizeX / 2 - _camera.Pos.X, (float)_pos.Y - _sizeY / 2 - _camera.Pos.Y, _sizeX, _sizeY);
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
                else if (Math.Abs(areaTop.Top - areaBottom.Bottom) < Math.Abs(areaLeft.Left - areaRight.Right))
                {
                    if (areaTop == obj)
                    {
                        return "bottom";
                    }
                    else
                    {
                        return "top";
                    }
                } else
                {
                    return "no";
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

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
        }

        public string Collide
        {
            get { return _collide; }
            set { _collide = value; }
        }

        public float G
        {
            get { return _g; }
        }

        public bool GravityEffect
        {
            get { return _gravity; }
            set { _gravity = value; }
        }
    }
}
