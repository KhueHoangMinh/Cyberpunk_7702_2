using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cyberpunk77022
{
    public class Shotgun1 : Gun
    {
        public Shotgun1(Window window, float damage) : base("Shotgun", "shotgun", "a gun", 50, window, "gun6","shotgun",200,0,damage,5000000,1)
        {
        }
        public override void ShootAction()
        {
            if (this.Game != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    Bullet NewBullet = new NormalBullet(this.Game, this, new Random().Next(200,400), 40, this.Damage);
                    NewBullet.Angle += (float)new Random().NextDouble() * 0.5f - 0.25f;
                    this.Game.AddBullet(NewBullet);
                }
                for (int i = 0; i < 3; i++)
                {
                    this.Game.AddExplosion(new Explosion(this.Game, this.Game.Camera, new Random().Next(8, 10), new Random().Next(30, 50), new Point2D()
                    {
                        X = (double)new Random().Next((int)this.Nozzle.X - 10, (int)this.Nozzle.X + 10),
                        Y = (double)new Random().Next((int)this.Nozzle.Y - 10, (int)this.Nozzle.Y + 10),
                    }, Color.Random()));
                }
            }
            
        }
    }

}
