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
        public Pistol1(GameStage game, Object GunOf, float damage) : base(game,GunOf,"pistol","singleshot",80,0,damage,5000000,1)
        {
        }
    }
}
