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
        public Coin(GameStage game, Point2D pos, float VelX, float VelY): base(game.Camera, pos, 30, 30, Color.Yellow, true, VelX, VelY)
        {
            _game = game;
        }

        public void Update()
        {
            if(this.IsCollideAt(_game.GetPlayer) != "no")
            {
                Earn();
            }
            bool falling = true;
            foreach(Ground ground in _game.Grounds) 
            { 
                if(ground.IsCollideAt(this) == "top")
                {
                    falling = false;
                    this.VelX = this.VelX * 0.96f;
                    if (this.VelY > 0) this.VelY = 0;
                    this.Pos = new Point2D() { X = this.Pos.X, Y = ground.Top - (this.Bottom - this.Top) / 2 + 1};
                }
                if (ground.IsCollideAt(this) == "right")
                {
                    this.VelX = 0;
                    this.Pos = new Point2D() { X = ground.Right + (this.Right - this.Left) / 2 - 1, Y = this.Pos.Y};
                }
                if (ground.IsCollideAt(this) == "bottom")
                {
                    if (this.VelY < 0) this.VelY = 0;
                    this.Pos = new Point2D() { X = this.Pos.X, Y = ground.Bottom + (this.Bottom - this.Top) / 2 - 1 };
                }
                if (ground.IsCollideAt(this) == "left")
                {
                    this.VelX = 0;
                    this.Pos = new Point2D() { X = ground.Left - (this.Right - this.Left) / 2 + 1, Y = this.Pos.Y };
                }
            }
            this.Pos = new Point2D() { X = this.Pos.X + this.VelX, Y = this.Pos.Y };
            if (falling) this.Gravity();
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
