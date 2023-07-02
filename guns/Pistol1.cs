using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Pistol1 : Gun
    {
        public Pistol1(Window window, float damage) : base("Default Gun", "default", "a gun", 0, window,"default","singleshot",80,0,damage,5000000,1)
        {
        }
    }
}
