using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Pistol2 : Gun
    {
        public Pistol2(Window window, float damage) : base("Pistol 2", "pistol", "a gun", 50, window,"gun2","singleshot2",80,0,damage,5000000,1)
        {
        }
        public Pistol2(Window window, float damage, bool enemys) : base("Pistol 2", "pistol", "a gun", 50, window, "gun2", "singleshot2", 80, 0, damage, 5000000, 1, enemys)
        {
        }
    }
}
