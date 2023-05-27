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

        public Gun(Window window, Object GunOf, Camera camera)
        {
            _window = window;
            _GunOf = GunOf;
            _camera = camera;
            SplashKit.LoadBitmap("pistol", "guns/pistol.png");
            pistol = SplashKit.BitmapNamed("pistol");
            //_drawingOptions.Angle = (float)80;
            _pos = new Point2D() { X = 200, Y = pistol.Height * (200.0 / pistol.Width) };
            drawingOptions = new DrawingOptions()
            {
                Dest = window,
                ScaleX = (float)(200.0/pistol.Width),
                ScaleY = (float)(200.0 / pistol.Width),
                AnchorOffsetX = 0,
                AnchorOffsetY = 0,
                Angle = 0,
            };
        }
        public void Update()
        {

        }
        public void Draw()
        {
            SplashKit.DrawBitmap(pistol,_GunOf.Pos.X - _camera.Pos.X - _pos.X,_GunOf.Pos.Y - _camera.Pos.Y - _pos.Y, drawingOptions);
        }
    }
}
