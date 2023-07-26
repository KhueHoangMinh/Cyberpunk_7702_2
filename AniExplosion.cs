using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cyberpunk77022
{
    public class AniExplosion : Explosion
    {
        Bitmap _explo;
        DrawingOptions _drawingOptions;
        float drawingcell = 0;
        bool drawing = true;
        public AniExplosion(GameStage game, Camera camera, float rad, Point2D pos, Bitmap explo) : base(game,camera,rad,rad,pos,Color.White)
        {
            _explo = explo;
            float scale = (float)(rad / _explo.CellHeight);
            _drawingOptions = new DrawingOptions()
            {
                Dest = game.Manager.Window,
                Angle = new Random().Next(0,360),
                ScaleX = scale,
                ScaleY = scale,
                DrawCell = (int)drawingcell
            };
        }

        public override void Update()
        {
            drawingcell += 0.3f;
            if(drawingcell > _explo.CellCount + 0.3f)
            {
                this.Game.RemoveExplosion();
                drawing = false;
            } else
            {
                _drawingOptions.DrawCell = (int)drawingcell;
            }
        }

        public override void Draw()
        {
            
            if(drawing) SplashKit.DrawBitmap(_explo, this.Pos.X - _explo.CellHeight / 2 - this.Game.Camera.Pos.X, this.Pos.Y - _explo.CellHeight / 2 - this.Game.Camera.Pos.Y, _drawingOptions);
        }

        public bool Drawing
        {
            get { return drawing; }
        }
    }
}
