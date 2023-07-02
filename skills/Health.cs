using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace Cyberpunk77022.skills
{
    public class Health : Skill
    {
        public Health() : base("Health","health","Boost health", 100) 
        { 

        }

        public override void Graphic(float x, float y)
        {
            SplashKit.FillRectangle(Color.Green, x - 20, y - 60, 40, 120);
            SplashKit.FillRectangle(Color.Green, x - 60, y - 20, 120, 40);
        }

        public override void InGameGraphic(float x, float y)
        {
            SplashKit.FillRectangle(Color.Green, x - 5, y - 10, 10, 20);
            SplashKit.FillRectangle(Color.Green, x - 10, y - 5, 20, 10);
        }
    }
}
