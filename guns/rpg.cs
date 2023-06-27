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
        public RPG(GameStage game, Object GunOf, float damage) : base(game, GunOf, "rpg", "singleshot", 250, 125, damage, 10000000, 1)
        {
        }
        public override void ShootAction()
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
