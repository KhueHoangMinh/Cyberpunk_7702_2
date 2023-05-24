using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Button
    {
        float _x;
        float _y;
        float _sizeX;
        float _sizeY;
        Color _color;
        Color _background;
        string _text;
        bool _clicked = false;

        public Button(string text, Color color, float x, float y, float sizeX, float sizeY) { 
            _text = text;
            _color = color;
            _background = color;
            _background.A = (float)0.2;
            _x = x; 
            _y = y;
            _sizeX = sizeX;
            _sizeY = sizeY;
            SplashKit.LoadFont("font", "Roboto-Bold.ttf");
        }

        public void Update()
        {
            if(Hover())
            {
                if(_background.R <= 0.99 && _background.R < _color.R + 0.3) _background.R += (float)0.01;
                if (_background.G <= 0.99 && _background.G < _color.G + 0.3) _background.G += (float)0.01;
                if (_background.B <= 0.99 && _background.B < _color.B + 0.3) _background.B += (float)0.01;
            } else
            {
                if (_background.R >= 0.01 && _background.R > _color.R + 0.01) _background.R -= (float)0.01;
                if (_background.G >= 0.01 && _background.G > _color.G + 0.01) _background.G -= (float)0.01;
                if (_background.B >= 0.01 && _background.B > _color.B + 0.01) _background.B -= (float)0.01;
            }
        }

        public bool Hover()
        {
            Point2D mousePos = SplashKit.MousePosition();
            if(mousePos.X >= _x-_sizeX/2 && mousePos.X <= _x+_sizeX/2 && mousePos.Y >= _y - _sizeY/2 && mousePos.Y <= _y + _sizeY/2)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void Draw()
        {
            SplashKit.FillRectangle(_background,_x-_sizeX/2,_y-_sizeY/2,_sizeX,_sizeY);
            SplashKit.FillRectangle(_background, _x - _sizeX / 4, _y - _sizeY / 4, _sizeX/2, _sizeY/2);
            SplashKit.DrawText(_text, _color, "font", 40, _x - SplashKit.TextWidth(_text,"font",40)/2, _y - SplashKit.TextHeight(_text, "font", 40) / 2);
        }
    }
}
