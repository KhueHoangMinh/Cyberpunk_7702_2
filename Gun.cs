using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Gun
    {
        Object _GunOf;
        Camera _camera;
        Window _window;
        SoundEffect singleshot;
        Bitmap pistol;
        Point2D _pos;
        DrawingOptions drawingOptions;
        float DisplayWidth;
        float scale;
        GameStage _game;
        float _shock = 0;
        long _ShootTime;
        // 0.5 secs
        float _fireRate = 1000000;
        bool smoking = false;
        Point2D nozzle;

        public Gun(GameStage game, Window window, Object GunOf, Camera camera)
        {
            _game = game;
            _window = window;
            _GunOf = GunOf;
            _camera = camera;
            singleshot = SplashKit.SoundEffectNamed("singleshot");
            //singleshot.
            pistol = SplashKit.BitmapNamed("pistol");
            DisplayWidth = 100;
            scale = (float)(DisplayWidth / pistol.Width);
            _pos = new Point2D() { X = (pistol.Width - DisplayWidth)/2, Y = (pistol.Height - pistol.Height*scale) / 2, };
            drawingOptions = new DrawingOptions()
            {
                Dest = window,
                ScaleX = scale,
                ScaleY = scale,
                AnchorOffsetX = -pistol.Width/2,
                AnchorOffsetY = 0,
                Angle = 0,
            };
            _ShootTime = 999999999;
            float a = (float)(SplashKit.MousePosition().X - _GunOf.Pos.X + camera.Pos.X);
            float b = (float)(SplashKit.MousePosition().Y - _GunOf.Pos.Y + camera.Pos.Y);
            float c = (float)Math.Sqrt(a * a + b * b);
            nozzle = new Point2D() { X = _GunOf.Pos.X + 100 * a / c, Y = _GunOf.Pos.Y + 100 * b / c };
        }
        public void Update()
        {
            if (!smoking && DateTime.UtcNow.Ticks - _ShootTime >= _fireRate + 500000)
            { 
                smoking = true;
            }
            if (smoking && DateTime.UtcNow.Ticks - _ShootTime <= 40000000 && new Random().Next(1,10) <= 3)
            {
                float a = (float)(SplashKit.MousePosition().X - _GunOf.Pos.X + _camera.Pos.X);
                float b = (float)(SplashKit.MousePosition().Y - _GunOf.Pos.Y + _camera.Pos.Y);
                float c = (float)Math.Sqrt(a * a + b * b);
                nozzle = new Point2D() { X = _GunOf.Pos.X + 100 * a / c, Y = _GunOf.Pos.Y + 100 * b / c };
                _game.AddSmoke(new Smoke(_camera, new Random().Next(2, 3), new Random().Next(20, 50), new Point2D()
                {
                    X = (double)new Random().Next((int)nozzle.X - 10, (int)nozzle.X + 10),
                    Y = (double)new Random().Next((int)nozzle.Y - 10, (int)nozzle.Y + 10),
                }, Color.White));
            } else if(DateTime.UtcNow.Ticks - _ShootTime > 8000000)
            {
                smoking = false;
            }
            _shock = _shock*(float)0.9;
        }
        public void Draw()
        {
            float angle = (float)Math.Atan((SplashKit.MousePosition().Y - _GunOf.Pos.Y + _camera.Pos.Y) / (SplashKit.MousePosition().X - _GunOf.Pos.X + _camera.Pos.X)) + _shock;
            if (SplashKit.MousePosition().X > _GunOf.Pos.X - _camera.Pos.X)
            {
                drawingOptions.Angle = (float)(360 / (Math.PI * 2)) * angle;
                drawingOptions.FlipY = false;
                drawingOptions.AnchorOffsetX = -pistol.Width / 2;
                SplashKit.DrawBitmapOnWindow(_window, pistol, _GunOf.Pos.X - _camera.Pos.X - _pos.X, _GunOf.Pos.Y - _camera.Pos.Y - _pos.Y - pistol.Height * scale / 2, drawingOptions);
            }
            else
            {
                drawingOptions.Angle = -360 + (float)(360 / (Math.PI * 2)) * angle;
                drawingOptions.FlipY = true;
                drawingOptions.AnchorOffsetX = pistol.Width / 2;
                SplashKit.DrawBitmapOnWindow(_window, pistol, _GunOf.Pos.X - _camera.Pos.X - _pos.X - DisplayWidth, _GunOf.Pos.Y - _camera.Pos.Y - _pos.Y - pistol.Height * scale / 2, drawingOptions);
            }
        }

        public void Shoot()
        {
            if(DateTime.UtcNow.Ticks - _ShootTime >= _fireRate)
            {
                smoking = false;
                _ShootTime = DateTime.UtcNow.Ticks;
                singleshot.Play();
                if (SplashKit.MousePosition().X > _GunOf.Pos.X - _camera.Pos.X)
                {
                    _shock -= 2;
                }
                else
                {
                    _shock += 2;
                }
                Bullet NewBullet = new Bullet(_camera, _GunOf.Pos, 100, 40);
                for(int i = 0; i < 3; i++)
                {
                    _game.AddExplosion(new Explosion(_camera, new Random().Next(8, 10), new Random().Next(30, 50), new Point2D()
                    {
                        X = (double)new Random().Next((int)NewBullet.InitPos.X - 10, (int)NewBullet.InitPos.X + 10),
                        Y = (double)new Random().Next((int)NewBullet.InitPos.Y - 10, (int)NewBullet.InitPos.Y + 10),
                    }, Color.Random()));
                }
                _game.AddBullet(NewBullet);
                _game.AddTrace(new Trace(_window, _camera, NewBullet));
            }
        }
    }
}
