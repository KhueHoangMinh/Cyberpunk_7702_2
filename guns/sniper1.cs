using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Sniper1 : Gun
    {
        public Sniper1(Window window, float damage) : base("Sniper 1", "sniper1", "a gun", 50, window, "gun1", "sniper", 250,50, damage, 10000000, 1)
        {
        }
    }
}
