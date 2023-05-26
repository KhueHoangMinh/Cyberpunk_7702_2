using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Something
    {
        Color _color = Color.RandomRGB(2);
        float _x = new Random().Next(0, 1920);
        float _y = new Random().Next(0, 1080);
        float _sizeX = (float)(new Random().Next(200, 1200));
        float _sizeY = (float)(new Random().Next(200, 1200));
        float diff_dis = (float)(new Random().Next(1, 10));
        string _state;
        float _initX;
        float _initY;
        bool clrDirRed = false;
        bool clrDirGreen = false;
        bool clrDirBlue = false;

        public Something()
        {
            _initX = _x;
            _initY = _y;
        }
        public void Update(string state, int width, int height, Point2D coord)
        {
            if (_color.R == 1) clrDirRed = false;
            if (_color.G == 1) clrDirGreen = false;
            if (_color.B == 1) clrDirBlue = false;
            if (_color.R == 0) clrDirRed = true;
            if (_color.G == 0) clrDirGreen = true;
            if (_color.B == 0) clrDirBlue = true;
            if (clrDirRed)
            {
                _color.R += (float)0.01;
            } else
            {
                _color.R -= (float)0.01;
            }
            if (clrDirGreen)
            {
                _color.G += (float)0.01;
            }
            else
            {
                _color.G -= (float)0.01;
            }
            if (clrDirBlue)
            {
                _color.B += (float)0.01;
            }
            else
            {
                _color.B -= (float)0.01;
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
