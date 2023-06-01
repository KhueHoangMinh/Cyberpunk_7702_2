using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public abstract class Enemy : Object
    {
        float _a = (float)0.5;
        bool _jumped = false;
        Gun _EnemyGun;
        GameStage _game;
        Camera _camera;
        Point2D dest;
        public Enemy(GameStage game, Window window, Camera camera, Point2D pos, float sizeX, float sizeY, Color color) : base(camera, pos, sizeX, sizeY, color, true, 0, 0)
        {
            _EnemyGun = new Gun(game, window, this, camera);
            _game = game;
            _camera = camera;
            dest = new Point2D() { X = new Random().Next(200, 500), Y = new Random().Next(200, 500) };
            if(new Random().Next(0,1) < 1)
            {
                dest.X *= -1;
            }
            if (new Random().Next(0, 1) < 1)
            {
                dest.Y *= -1;
            }
        }
        public void Update(List<Ground> grounds, List<Bullet> bullets)
        {
            _jumped = true;
            string collide = "no";
            for (int i = 0; i < grounds.Count; i++)
            {
                string isCollide = this.IsCollideAt(grounds[i]);
                if (isCollide != "no") collide = isCollide;
                if (isCollide == "bottom")
                {
                    this.Pos = new Point2D() { X = this.Pos.X, Y = grounds[i].Top - (this.Bottom - this.Pos.Y) + 1 };
                    if (this.VelY > 0) this.VelY = 0;
                    _jumped = false;
                }
                else
                if (isCollide == "top")
                {
                    this.Pos = new Point2D() { X = this.Pos.X, Y = grounds[i].Bottom + (-this.Top + this.Pos.Y - 1) };
                    if (this.VelY < 0) this.VelY = 0;
                }
                else
                if (isCollide == "right")
                {
                    _jumped = false;
                    this.Pos = new Point2D() { X = grounds[i].Left + (-this.Right + this.Pos.X), Y = this.Pos.Y - 1 };
                    if (this.VelX > 0) this.VelX = 0;
                }
                else
                if (isCollide == "left")
                {
                    _jumped = false;
                    this.Pos = new Point2D() { X = grounds[i].Right - (this.Left - this.Pos.X), Y = this.Pos.Y + 1 };
                    if (this.VelX < 0) this.VelX = 0;
                }
            }
            base.Gravity();
            if (_game.GetPlayer.Pos.X + dest.X < this.Pos.X)
            {
                this.VelX -= _a;
            }
            else if (_game.GetPlayer.Pos.X + dest.X > this.Pos.X)
            {
                this.VelX += _a;
            }
            if (false)
            {
                this.VelY = -10;
                for (int i = 0; i < 3; i++)
                {
                    switch (collide)
                    {
                        case "bottom":
                            _game.AddExplosion(new Explosion(_game,_camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Pos.X - 20, (int)this.Pos.X + 20),
                                Y = (double)new Random().Next((int)this.Bottom - 10, (int)this.Bottom + 10),
                            }, Color.White));
                            break;

                        case "left":
                            _game.AddExplosion(new Explosion(_game,_camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Left - 20, (int)this.Left + 20),
                                Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                            }, Color.White));
                            break;

                        case "right":
                            _game.AddExplosion(new Explosion(_game, _camera, new Random().Next(10, 25), new Random().Next(30, 60), new Point2D()
                            {
                                X = (double)new Random().Next((int)this.Right - 20, (int)this.Right + 20),
                                Y = (double)new Random().Next((int)this.Pos.Y - 10, (int)this.Pos.Y + 10),
                            }, Color.White));
                            break;

                    }
                }
            }
            VelX = VelX / (float)1.06;
            this.Pos = new Point2D() { X = this.Pos.X + this.VelX, Y = this.Pos.Y + this.VelY };
            _EnemyGun.Update(_game.GetPlayer.Pos);
            if (SplashKit.MouseDown(MouseButton.LeftButton))
            {
                _EnemyGun.Shoot();
            }
        }

        public void DrawGun()
        {
            _EnemyGun.Draw();
        }
    }

    public class NormalEnemy : Enemy
    {
        public NormalEnemy(GameStage game, Window window, Camera camera, Point2D pos, float sizeX, float sizeY, Color color) : base(game, window, camera, pos, sizeX, sizeY, color) { 
        
        }
    }
}
