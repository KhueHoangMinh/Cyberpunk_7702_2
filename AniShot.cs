using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class AniShot
    {
        GameStage _game;
        Camera _camera;
        Bitmap _shot;
        Point2D _pos;
        DrawingOptions _drawingOptions;
        float drawingcell = 0;
        bool drawing = true;
        public AniShot(GameStage game, Camera camera, Point2D pos, float angle, bool reversed)
        {
            _game = game;
            _camera = camera;
            _pos = pos;
            _shot = SplashKit.BitmapNamed("shot_animation");
            float scale = (float)(500.0 / (_shot.Height / 5.0));
            _drawingOptions = new DrawingOptions()
            {
                Dest = game.Manager.Window,
                Angle = angle,
                ScaleX = scale,
                ScaleY = scale,
                FlipY = reversed,
                DrawCell = (int)drawingcell
            };
        }

        public void Update()
        {
            drawingcell += 0.2f;
            if (drawingcell > 3 + 0.2f)
            {
                _game.RemoveAniShot();
                drawing = false;
            }
            else
            {
                _drawingOptions.DrawCell = (int)drawingcell;
            }
        }

        public void Draw()
        {
            if (drawing) SplashKit.DrawBitmap(_shot, _pos.X - _shot.Height / 10 - _camera.Pos.X, _pos.Y - _shot.Height / 10 - _camera.Pos.Y, _drawingOptions);
        }

        public bool Drawing
        {
            get { return drawing; }
        }
    }
}
