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
        public Shotgun1(Window window, float damage, bool enemys) : base("Shotgun", "shotgun", "a gun", 50, window, "gun6", "shotgun", 200, 0, damage, 5000000, 1, enemys)
        {
            this.Range = 350;
        }
        public Shotgun1(Window window, float damage) : base("Shotgun", "shotgun", "a gun", 50, window, "gun6","shotgun",200,0,damage,5000000,1)
        {
            this.Range = 350;
        }
        public override void ShootAction(Color color)
        {
            if (this.Game != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    Bullet NewBullet = this.BulletFactory.CreateBullet(this.Game,this,color);
                    NewBullet.Range = new Random().Next((int)this.Range - 100, (int)this.Range + 100);
                    if (enemyGun)
                    {
                        NewBullet.Speed = 20;
                    }
                    NewBullet.Angle += (float)new Random().NextDouble() * 0.5f - 0.25f;
                    this.Game.AddBullet(NewBullet);
                }
            }
            
        }
    }

}
