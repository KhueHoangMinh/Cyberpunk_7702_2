using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class InEffect
    {
        Color _color;
        int _width;
        int _height;
        public InEffect(int width, int height)
        {
            _color = Color.Black;
            _color.A = (float)1;
            _width = width;
            _height = height;
        }

        public void Draw()
        {
            if(_color.A >= 0.02)
            {
                _color.A -= (float)0.02;
            }
            SplashKit.FillRectangle(_color, 0, 0, _width, _height);
        }
    }
    public class OutEffect
    {
        Color _color;
        public bool _completed = false;
        int _width;
        int _height;
        public OutEffect(int width, int height)
        {
            _color = Color.Black;
            _color.A = (float)0;
            _width = width;
            _height = height;
        }

        public void Draw()
        {
            if (_color.A <= 0.98)
            {
                _color.A += (float)0.02;
            } else
            {
                _completed = true;
            }
            SplashKit.FillRectangle(_color, 0, 0, _width, _height);
        }
    }
}
