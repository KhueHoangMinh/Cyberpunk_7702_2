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
        Color _textColor;
        Color _background;
        string _text;
        SoundEffect _soundEffect;
        bool _hovering = false;
        bool _active = false;

        public Button(string text, Color color, float x, float y, float sizeX, float sizeY) { 
            _text = text;
            _color = color;
            _textColor = color;
            _background = color;
            _background.A = (float)0.2;
            _x = x; 
            _y = y;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _soundEffect = SplashKit.SoundEffectNamed("click");
        }

        public bool Hovering
        {
            get
            {
                return _hovering;
            }
        }

        public void Update()
        {
            bool hovering = Hover();
            if(hovering || _active)
            {
                if(_background.R <= 0.99 && _background.R < _color.R + 0.5) _background.R += (float)0.01;
                if (_background.G <= 0.99 && _background.G < _color.G + 0.5) _background.G += (float)0.01;
                if (_background.B <= 0.99 && _background.B < _color.B + 0.5) _background.B += (float)0.01;
                if(SplashKit.MouseClicked(MouseButton.LeftButton) && hovering)
                {
                    _soundEffect.Play();
                }
            } else
            {
                if (_background.R >= 0.01 && _background.R > _color.R + 0.01) _background.R -= (float)0.01;
                if (_background.G >= 0.01 && _background.G > _color.G + 0.01) _background.G -= (float)0.01;
                if (_background.B >= 0.01 && _background.B > _color.B + 0.01) _background.B -= (float)0.01;
            }
            _hovering = hovering;
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
            SplashKit.DrawRectangle(_background, _x - _sizeX / 2, _y - _sizeY / 2, _sizeX, _sizeY);
            SplashKit.DrawText(_text, _textColor, "font", 50, _x - SplashKit.TextWidth(_text,"font",50)/2, _y - SplashKit.TextHeight(_text, "font", 50) / 2);
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public Color Color { get { return _textColor; } set {  _textColor = value; } }
    }
}
