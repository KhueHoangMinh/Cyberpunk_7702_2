using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Sniper2 : Gun
    {
        public Sniper2(GameStage game, Object GunOf, float damage) : base(game, GunOf, "gun5", "singleshot", 250,50, damage, 10000000, 1)
        {
        }
    }
}
