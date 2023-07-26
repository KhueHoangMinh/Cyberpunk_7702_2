using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class AniSmoke : Smoke
    {
        Bitmap _smoke;
        DrawingOptions _drawingOptions;
        float drawingcell = 0;
        bool drawing = true;
        public AniSmoke(GameStage game, Camera camera, float rad, float maxRad, Point2D pos, float initVelX, float initVelY) : base(game,camera,rad,maxRad,pos,Color.White,initVelX,initVelY)
        {
            _smoke = SplashKit.BitmapNamed("smoke_animation");
            float scale = (float)(rad / (_smoke.Height / 2));
            _drawingOptions = new DrawingOptions()
            {
                Dest = game.Manager.Window,
                Angle = new Random().Next(0, 360),
                ScaleX = scale,
                ScaleY = scale,
                DrawCell = (int)drawingcell
            };
        }
        public override void Update()
        {
            _drawingOptions.ScaleX = (float)(this.Rad / (_smoke.Height / 2));
            _drawingOptions.ScaleY = (float)(this.Rad / (_smoke.Height / 2));
            _drawingOptions.Angle += this.RotSpeed;
            this.Pos = new Point2D() { X = this.Pos.X + this.InitVelX, Y = this.Pos.Y + this.InitVelY - this.VelY};
            this.VelY = this.VelY * 0.98f;
            this.InitVelX *= 0.92f;
            this.InitVelY *= 0.92f;
            this.Rad += 3f;
            drawingcell += 0.3f;
            if (drawingcell > SplashKit.BitmapNamed("smoke_animation").CellCount + 0.3f)
            {
                this.Game.RemoveSmoke();
                drawing = false;
            }
            else
            {
                _drawingOptions.DrawCell = (int)drawingcell;
            }
        }

        public override void Draw()
        {
            if (drawing) SplashKit.DrawBitmap(_smoke, this.Pos.X - _smoke.Height / 4 - this.Game.Camera.Pos.X, this.Pos.Y - _smoke.Height / 4 - this.Game.Camera.Pos.Y, _drawingOptions);
        }

        public bool Drawing
        {
            get { return drawing; }
        }

    }
}
