using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    class Star
    {
        float _velocity = (float)(new Random().Next(1, 5));
        float _x = new Random().Next(-500, 2420);
        float _y = new Random().Next(0, 1080);
        float _sizeX = (float)(new Random().Next(5, 10));
        float _sizeY = (float)(new Random().Next(25, 40));
        Color _color = Color.RandomRGB(100);
        float diff_dis = (float)(new Random().Next(1, 10));
        string _state;
        float _initX;
        float _initY;

        public Star()
        {
            _initX = _x;
            _initY = _y;
        }

        public void Update(string state, int width, int height, Point2D coord)
        {

            if (_initY < -500)
            {
                _initY = (float)height;
                _y = (float)height;
            }
            else
            {
                _initY = _initY - _velocity;
            }
            _x += (-_x + (_initX - ((float)coord.X - (float)width / 2) / diff_dis)) * (float)0.04;
            _y += (-_y + (_initY - ((float)coord.Y - (float)height / 2) / diff_dis)) * (float)0.04;
        }

        public void Draw()
        {
            SplashKit.FillRectangle(_color, _x - _sizeX / 2, _y - _sizeY / 2, _sizeX, _sizeY);
        }
    }
}
