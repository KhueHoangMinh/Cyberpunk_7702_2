using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SplashKitSDK;

namespace Cyberpunk77022
{
    public class Coin : Object
    {
        GameStage _game;
        DrawingOptions _drawingOptions;
        Bitmap _coin;
        float drawingcell = 0;
        public Coin(GameStage game, Point2D pos, float VelX, float VelY): base(game.Camera, pos, 30, 30, Color.Yellow, true, VelX, VelY)
        {
            _game = game;
            _coin = SplashKit.BitmapNamed("coin_animation");
            float scale = (float)(30.0 / _coin.Width);
            _drawingOptions = new DrawingOptions()
            {
                Dest = game.Manager.Window,
                Angle = 0,
                ScaleX = scale,
                ScaleY = scale,
                DrawCell = (int)drawingcell
            };
        }
        public override void CollideTop(Object @object)
        {
            this.Pos = new Point2D() { X = this.Pos.X, Y = @object.Bottom + (this.Bottom - this.Top) / 2 + 1 };
            if (this.VelY < 0) this.VelY = -this.VelY * 0.35f;
            if (this.Collide == "no") this.Collide = "top";
        }
        public override void CollideBottom(Object @object)
        {
            this.Pos = new Point2D() { X = this.Pos.X, Y = @object.Top - (this.Bottom - this.Top) / 2 - 1 };
            this.VelX = this.VelX * 0.96f;
            if (this.VelY > 0) this.VelY = -this.VelY * 0.35f;
            if (this.Collide == "no") this.Collide = "bottom";
        }
        public override void CollideRight(Object @object)
        {
            this.Pos = new Point2D() { X = @object.Left - (this.Right - this.Left) / 2 - 1, Y = this.Pos.Y };
            if (this.VelX > 0) this.VelX = -this.VelX * 0.65f;
            if (this.Collide == "no") this.Collide = "right";
        }
        public override void CollideLeft(Object @object)
        {
            this.Pos = new Point2D() { X = @object.Right + (this.Right - this.Left) / 2 + 1, Y = this.Pos.Y };
            if (this.VelX < 0) this.VelX = -this.VelX * 0.65f;
            if (this.Collide == "no") this.Collide = "left";
        }

        public override void Update()
        {
            _drawingOptions.Angle += 5;
            if(this.IsCollideAt(_game.GetPlayer) != "no")
            {
                Earn();
            }
            this.MoveObject(_game.Grounds);
            if (this.Collide != "bottom") this.Gravity();
            drawingcell += 0.2f;
            if (drawingcell > SplashKit.BitmapCellCount(SplashKit.BitmapNamed("coin_animation"))) drawingcell = 1;
            _drawingOptions.DrawCell = (int)drawingcell;
            this.Collide = "no";
        }

        public override void Draw()
        {
            SplashKit.DrawBitmap(_coin, this.Left - _coin.Width / 2 + 15 - _game.Camera.Pos.X, this.Top - _coin.Width / 2 + 15 - _game.Camera.Pos.Y, _drawingOptions);
        }

        public void Earn()
        {
            SplashKit.SoundEffectNamed("coin").Play();
            _game.AddExplosion(new Explosion(_game,_game.Camera,15,100,this.Pos,Color.Yellow,0.5f,30));
            _game.RemoveCoin(this);
            _game.Manager.Coin++;
        }
    }
}
