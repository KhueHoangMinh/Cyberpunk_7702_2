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
            this.BulletFactory = new RPGBulletFactory();
        }
        public RPG(Window window, float damage,bool enemys) : base("RPG", "rpg", "a gun", 50, window, "rpg", "singleshot", 250, 125, damage, 10000000, 1, enemys)
        {
            this.BulletFactory = new RPGBulletFactory();
        }
    }
}
