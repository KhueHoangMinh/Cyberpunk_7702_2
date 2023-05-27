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
        Gun _PlayerGun;
        public Player(GameStage game,Window window, Camera camera, Point2D pos, float sizeX, float sizeY, Color color) : base(camera, pos, sizeX,sizeY,color,true,0,0) { 
            _PlayerGun = new Gun(game, window, this, camera);
        }

        public void Update(List<Ground> grounds, List<Bullet> bullets)
        {
            bool graCheck = true;
            for(int i = 0; i < grounds.Count; i++)
            {
                string collide = this.IsCollideAt(grounds[i]);
                if (collide == "bottom")
                {
                    graCheck = false;
                    this.Pos = new Point2D() { X = this.Pos.X, Y = grounds[i].Top - (this.Bottom - this.Pos.Y) + 1 };
                    this.VelY = 0;
                    _jumped = false;
                } else
                if (collide == "top")
                {
                    this.Pos = new Point2D() { X = this.Pos.X, Y = grounds[i].Bottom + (-this.Top + this.Pos.Y) };
                    this.VelY = 0;
                } else
                if (collide == "right")
                {
                    this.Pos = new Point2D() { X = grounds[i].Left + (-this.Right + this.Pos.X), Y = this.Pos.Y };
                    this.VelX = 0;
                } else
                if (collide == "left")
                {
                    this.Pos = new Point2D() { X = grounds[i].Right - (this.Left - this.Pos.X), Y = this.Pos.Y };
                    this.VelX = 0;
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
            _PlayerGun.Update();
            if(SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                _PlayerGun.Shoot();
            }
        }

        public void DrawGun() 
        {
            _PlayerGun.Draw();
        }
    }
}
