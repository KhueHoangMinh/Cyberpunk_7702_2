using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Ground : Object
    {
        public Ground(Camera camera, Point2D pos, float sizeX, float sizeY, Color color) : base(camera, pos,sizeX,sizeY,color,false,0,0) { 
        }

        //public void Update(Camera camera)
        //{

        //}
    }
}
