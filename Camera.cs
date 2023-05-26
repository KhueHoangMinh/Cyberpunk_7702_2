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
        Point2D _pos = new Point2D() { X = 0, Y = 0 };
        int _width;
        int _height;

        public Camera(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Update(Point2D dest)
        {
            _pos  = new Point2D() { X = this.Pos.X + (dest.X - this.Middle.X)*0.1, Y = this.Pos.Y + (dest.Y - this.Middle.Y) * 0.1 };
        }

        public Point2D Middle
        {
            get { 
                return new Point2D() { X = this.Pos.X + _width / 2, Y = this.Pos.Y + _height / 2 }; 
            }
        }

        public Point2D Pos { 
            get { return _pos; } 
        }
    }
}
