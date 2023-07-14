using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class RPG : Gun
    {
        public RPG(Window window, float damage) : base("RPG", "rpg", "a gun", 50, window, "rpg", "singleshot", 250, 125, damage, 10000000, 1)
        {
        }
        public RPG(Window window, float damage,bool enemys) : base("RPG", "rpg", "a gun", 50, window, "rpg", "singleshot", 250, 125, damage, 10000000, 1, enemys)
        {
        }
        public override void ShootAction()
        {
            if(this.Game != null)
            {
                Bullet NewBullet = new RPGBullet(this.Game, this, 1000, 10, this.Damage);
                this.Game.AddBullet(NewBullet);
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
