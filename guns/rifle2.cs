using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Rifle2 : Gun
    {
        public Rifle2(Window window, float damage) : base("Rifle 2", "rifle2", "a gun", 50, window, "gun4", "singleshot", 250, 50, damage, 1500000, 0.1f)
        {
        }
    }
}
