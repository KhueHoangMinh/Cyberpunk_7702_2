using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace Cyberpunk77022.skills
{
    public class Defense : Skill
    {
        public Defense() : base("Defense", "defense", "Reduce bullet damage", 100)
        {

        }

        public override void Graphic(float x, float y)
        {
            SplashKit.FillRectangle(Color.DarkGray, x - 50, y - 50, 100, 100);
        }

        public override void InGameGraphic(float x, float y)
        {
            SplashKit.FillRectangle(Color.DarkGray, x - 5, y - 5, 10, 10);
        }
    }
}