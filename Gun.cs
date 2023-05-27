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

        public Gun(GameStage game, Window window, Object GunOf, Camera camera)
        {
            _game = game;
            _window = window;
            _GunOf = GunOf;
            _camera = camera;
            SplashKit.LoadBitmap("pistol", "guns/pistol.png");
            pistol = SplashKit.BitmapNamed("pistol");
            DisplayWidth = 100;
            scale = (float)(DisplayWidth / pistol.Width);
            //_drawingOptions.Angle = (float)80;
            _pos = new Point2D() { X = (pistol.Width - DisplayWidth)/2, Y = (pistol.Height - pistol.Height*scale) / 2, };
            drawingOptions = new DrawingOptions()
            {
                Dest = window,
                ScaleX = scale,
                ScaleY = scale,
                AnchorOffsetX = -pistol.Width/2,
                AnchorOffsetY = 0,
                //AnchorOffsetX = (float)(_GunOf.Pos.X - _camera.Pos.X - _pos.X),
                //AnchorOffsetY = (float)(_GunOf.Pos.Y - _camera.Pos.Y - _pos.Y),
                Angle = 0,
            };
        }
        public void Update()
        {
            if(SplashKit.MousePosition().X > _GunOf.Pos.X - _camera.Pos.X)
            {
                drawingOptions.Angle = (float)(360 / (Math.PI * 2)) * (float)Math.Atan((SplashKit.MousePosition().Y - _GunOf.Pos.Y + _camera.Pos.Y) / (SplashKit.MousePosition().X - _GunOf.Pos.X + _camera.Pos.X));
                drawingOptions.FlipY = false;
                drawingOptions.AnchorOffsetX = -pistol.Width / 2;
            } else
            {
                drawingOptions.Angle = - 360 + (float)(360 / (Math.PI * 2)) * (float)Math.Atan((SplashKit.MousePosition().Y - _GunOf.Pos.Y + _camera.Pos.Y) / (SplashKit.MousePosition().X - _GunOf.Pos.X + _camera.Pos.X));
                drawingOptions.FlipY = true;
                drawingOptions.AnchorOffsetX = pistol.Width / 2;
            }
        }
        public void Draw()
        {
            if (SplashKit.MousePosition().X > _GunOf.Pos.X - _camera.Pos.X)
            {
                SplashKit.DrawBitmap(pistol, _GunOf.Pos.X - _camera.Pos.X - _pos.X, _GunOf.Pos.Y - _camera.Pos.Y - _pos.Y - pistol.Height * scale / 2, drawingOptions);
            }
            else
            {
                SplashKit.DrawBitmap(pistol, _GunOf.Pos.X - _camera.Pos.X - _pos.X - DisplayWidth, _GunOf.Pos.Y - _camera.Pos.Y - _pos.Y - pistol.Height * scale / 2, drawingOptions);
            }
        }

        public void Shoot()
        {
            _game.AddBullet(new Bullet(_camera,_GunOf.Pos,100, 50));
        }
    }
}
