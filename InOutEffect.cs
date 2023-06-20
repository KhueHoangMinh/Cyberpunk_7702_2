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
        float _x;
        float _y;
        int _width;
        int _height;
        public InEffect(float x, float y, int width, int height)
        {
            _color = Color.Black;
            _color.A = (float)1;
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public void Update()
        {
            if (_color.A >= 0.02)
            {
                _color.A -= (float)0.02;
            }
        }

        public void Draw()
        {
            SplashKit.FillRectangle(_color, _x, _y, _width, _height);
        }
    }
    public class OutEffect
    {
        Color _color;
        public bool _completed = false;
        float _x;
        float _y;
        int _width;
        int _height;
        public OutEffect(float x, float y, int width, int height)
        {
            _color = Color.Black;
            _color.A = (float)0;
            _width = width;
            _height = height;
        }

        public void Update()
        {
            if (_color.A <= 0.98)
            {
                _color.A += (float)0.02;
            }
            else
            {
                _completed = true;
            }
        }

        public void Draw()
        {
            SplashKit.FillRectangle(_color, _x, _y, _width, _height);
        }
    }
}
