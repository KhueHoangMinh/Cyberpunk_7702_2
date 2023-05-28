﻿using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        float _width = 15;
        float _height = 40;

        public Bullet(Camera camera, Point2D BasePos, float GunLength, float speed)
            
        {
            _color = Color.Yellow;
            _camera = camera;
            _color =  Color.Yellow;
            float a = (float)(SplashKit.MousePosition().X - BasePos.X + camera.Pos.X);
            float b = (float)(SplashKit.MousePosition().Y - BasePos.Y + camera.Pos.Y);
            float c = (float)Math.Sqrt(a * a + b * b);
            _VelX = (float)(speed * a/c);
            _VelY = (float)(speed * b/c);
            _initPos = new Point2D() { X = BasePos.X + GunLength * a/c, Y = BasePos.Y + GunLength * b / c };
            _Pos = _initPos;
            _corners = new Quad();
            _angle = (float)Math.PI * 2 - (float)Math.Atan(b / a);
        }

        public void Update()
        {
            _Pos = new Point2D() { X = this.Pos.X + _VelX, Y = this.Pos.Y + _VelY };
        }

        public void Draw()
        {
            //SplashKit.FillCircle(_color, _Pos.X - _camera.Pos.X, _Pos.Y - _camera.Pos.Y, 10);
            SplashKit.FillQuad(_color, calQuad());
        }

        public Quad calQuad()
        {
            float delta = (float)((Math.Sqrt(_width * _width + _height * _height) / 2));
            float beta = (float)(_angle - Math.Atan(_width / _height));
            float x = (float)_Pos.X - delta * (float)Math.Cos(beta) - (float)_camera.Pos.X;
            float y = (float)_Pos.Y + delta * (float)Math.Sin(beta) - (float)_camera.Pos.Y;
            float sinAngle = (float)Math.Sin(_angle);
            float cosAngle = (float)Math.Cos(_angle);
            return new Quad()
            {
                Points = new Point2D[4] {
                    new Point2D() { X = x, Y = y },
                    new Point2D() {
                        X = x + _height * cosAngle,
                        Y = y - _height * sinAngle
                    },
                    new Point2D() {
                        X = x + _width * sinAngle,
                        Y = y + _width * cosAngle
                    },
                    new Point2D() {
                        X = x + _width * sinAngle + _height * cosAngle,
                        Y = y + _width * cosAngle - _height * sinAngle
                    }
                }
            };
        }

        public Point2D Pos
        {
            get { return _Pos; }
        }

        public Point2D InitPos
        {
            get { return _initPos; }
        }

        public float Width
        {
            get { return _width; }
        }
        public float Height
        {
            get { return _height; }
        }

        public float Angle { get { return _angle; } }
    }
}
