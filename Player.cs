using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Player : Object
    {
        float _a = (float)1;
        bool _jumped = false;
        public Player(Point2D pos, float sizeX, float sizeY, Color color) : base(pos,sizeX,sizeY,color,true,0,0) { 
        }

        public void Update(List<Ground> grounds)
        {
            bool graCheck = true;
            for(int i = 0; i < grounds.Count; i++)
            {
                if (grounds[i].IsCollided(new Point2D() { X = this.Pos.X, Y = this.Bottom}))
                {
                    graCheck = false; 
                    this.Pos = new Point2D() { X = this.Pos.X, Y = grounds[i].Top - (this.Bottom - this.Pos.Y) };
                    this.VelY = 0;
                    _jumped = false;
                    break;
                }
            }
            if(graCheck)
            {
                base.Gravity();
            }
            if(SplashKit.KeyDown(KeyCode.AKey))
            {
                this.VelX -= _a;
            } else if (SplashKit.KeyDown(KeyCode.DKey))
            {
                this.VelX += _a;
            }
            if (SplashKit.KeyDown(KeyCode.WKey) && !_jumped)
            {
                _jumped = true;
                this.VelY = -10;
            }
            VelX = VelX / (float)1.06;
            this.Pos = new Point2D() { X = this.Pos.X + this.VelX, Y = this.Pos.Y + this.VelY };
        }

    }
}
