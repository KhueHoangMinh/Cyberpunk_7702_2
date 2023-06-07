using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Sniper1 : Gun
    {
        public Sniper1(GameStage game, Object GunOf, float damage) : base(game, GunOf, "gun1", "singleshot", 250,50, damage, 10000000, 1)
        {
        }
    }
}
