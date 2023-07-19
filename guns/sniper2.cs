﻿using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Sniper2 : Gun
    {
        public Sniper2(Window window, float damage) : base("Sniper 2", "sniper2", "a gun", 50, window, "gun5", "sniper", 250,50, damage, 10000000, 1)
        {
        }
        public Sniper2(Window window, float damage, bool enemys) : base("Sniper 2", "sniper2", "a gun", 50, window, "gun5", "sniper", 250, 50, damage, 10000000, 1, enemys)
        {
        }
        public override void ShootAction(Color color)
        {
            if (this.Game != null)
            {
                Bullet NewBullet = new SniperBullet(this.Game, this, 1000, this.Speed, this.Damage);
                this.Game.AddBullet(NewBullet);
            }
        }
    }
}
