using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Camera
    {
        Point2D _pos = new Point2D() { X = -100, Y = -100 };
        int _width;
        int _height;

        public Camera(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Update(Point2D dest)
        {
            _pos.X = _pos.X + (dest.X - this.Middle.X) * 0.08;
            _pos.Y = _pos.Y + (dest.Y - this.Middle.Y) * 0.08 - 25;
        }

        public Point2D Middle
        {
            get { 
                return new Point2D() { X = this.Pos.X + _width / 2, Y = this.Pos.Y + _height / 2 }; 
            }
        }

        public void Shock(float VelX,  float VelY , float speed)
        {
            _pos.X = _pos.X + 10 * VelX / speed;
            _pos.Y = _pos.Y + 10 * VelY / speed;
        }

        public Point2D Pos { 
            get { return _pos; } 
        }
    }
}
