using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Bullet
    {
        float _speed;
        float _angle;
        Point2D _initPos;
        Color _color;
        Camera _camera;
        float _VelX;
        float _VelY;
        Point2D _Pos;
        Quad _corners;
        float _width = 20;
        float _height = 50;

        public Bullet(Camera camera, Point2D initPos, float speed)
            
        {
            _color = Color.Yellow;
            _camera = camera;
            _initPos = initPos;
            _Pos = initPos;
            _color =  Color.Yellow;
            float a = (float)(SplashKit.MousePosition().X - initPos.X + camera.Pos.X);
            float b = (float)(SplashKit.MousePosition().Y - initPos.Y + camera.Pos.Y);
            float c = (float)Math.Sqrt(a * a + b * b);
            _VelX = (float)(speed * a/c);
            _VelY = (float)(speed * b/c);
            _corners = new Quad();
        }

        public void Update()
        {
            _Pos = new Point2D() { X = this.Pos.X + _VelX, Y = this.Pos.Y + _VelY };
        }

        public void Draw()
        {
            SplashKit.FillCircle(_color, _Pos.X - _camera.Pos.X, _Pos.Y - _camera.Pos.Y, 10);
        }

        public Point2D Pos
        {
            get { return _Pos; }
        }
    }
}
