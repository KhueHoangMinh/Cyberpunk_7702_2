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
        Bitmap pistol;
        Point2D _pos;
        DrawingOptions drawingOptions;
        float DisplayWidth;
        float scale;
        GameStage _game;
        float _shock = 0;
        long _ShootTime;
        // 0.5 secs
        float _fireRate = 2000000;

        public Gun(GameStage game, Window window, Object GunOf, Camera camera)
        {
            _game = game;
            _window = window;
            _GunOf = GunOf;
            _camera = camera;
            SplashKit.LoadBitmap("pistol", "guns/pistol1.png");
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
            _ShootTime = DateTime.UtcNow.Ticks - (long)_fireRate;
        }
        public void Update()
        {

            _shock = _shock*(float)0.9;
            float angle = (float)Math.Atan((SplashKit.MousePosition().Y - _GunOf.Pos.Y + _camera.Pos.Y) / (SplashKit.MousePosition().X - _GunOf.Pos.X + _camera.Pos.X)) + _shock;
            if(SplashKit.MousePosition().X > _GunOf.Pos.X - _camera.Pos.X)
            {
                drawingOptions.Angle = (float)(360 / (Math.PI * 2)) * angle;
                drawingOptions.FlipY = false;
                drawingOptions.AnchorOffsetX = -pistol.Width / 2;
            } else
            {
                drawingOptions.Angle = - 360 + (float)(360 / (Math.PI * 2)) * angle;
                drawingOptions.FlipY = true;
                drawingOptions.AnchorOffsetX = pistol.Width / 2;
            }
        }
        public void Draw()
        {
            if (SplashKit.MousePosition().X > _GunOf.Pos.X - _camera.Pos.X)
            {
                SplashKit.DrawBitmapOnWindow(_window, pistol, _GunOf.Pos.X - _camera.Pos.X - _pos.X, _GunOf.Pos.Y - _camera.Pos.Y - _pos.Y - pistol.Height * scale / 2, drawingOptions);
            }
            else
            {
                SplashKit.DrawBitmapOnWindow(_window, pistol, _GunOf.Pos.X - _camera.Pos.X - _pos.X - DisplayWidth, _GunOf.Pos.Y - _camera.Pos.Y - _pos.Y - pistol.Height * scale / 2, drawingOptions);
            }
        }

        public void Shoot()
        {
            if(DateTime.UtcNow.Ticks - _ShootTime >= _fireRate)
            {
                _ShootTime = DateTime.UtcNow.Ticks;
                if (SplashKit.MousePosition().X > _GunOf.Pos.X - _camera.Pos.X)
                {
                    _shock -= 2;
                }
                else
                {
                    _shock += 2;
                }
                Bullet NewBullet = new Bullet(_camera, _GunOf.Pos, 100, 30);
                _game.AddExplosion(new Explosion(_camera, new Random().Next(8,10), new Random().Next(30, 50), new Point2D()
                {
                    X = (double)new Random().Next((int)NewBullet.InitPos.X - 10, (int)NewBullet.InitPos.X + 10),
                    Y = (double)new Random().Next((int)NewBullet.InitPos.Y - 10, (int)NewBullet.InitPos.Y + 10),
                } , Color.Random()));
                _game.AddExplosion(new Explosion(_camera, new Random().Next(8, 10), new Random().Next(30, 50), new Point2D()
                {
                    X = (double)new Random().Next((int)NewBullet.InitPos.X - 10, (int)NewBullet.InitPos.X + 10),
                    Y = (double)new Random().Next((int)NewBullet.InitPos.Y - 10, (int)NewBullet.InitPos.Y + 10),
                }, Color.Random()));
                _game.AddExplosion(new Explosion(_camera, new Random().Next(8, 10), new Random().Next(30, 50), new Point2D()
                {
                    X = (double)new Random().Next((int)NewBullet.InitPos.X - 10, (int)NewBullet.InitPos.X + 10),
                    Y = (double)new Random().Next((int)NewBullet.InitPos.Y - 10, (int)NewBullet.InitPos.Y + 10),
                }, Color.Random()));
                _game.AddBullet(NewBullet);
                _game.AddTrace(new Trace(_window, _camera, NewBullet));
            }
        }
    }
}
