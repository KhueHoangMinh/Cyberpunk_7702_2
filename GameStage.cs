using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Cyberpunk77022
{
    public class GameStage
    {
        InEffect inEf;
        OutEffect outEf;
        bool _closing = false;

        public GameStage(Window window, int width, int height, Action<string> ChangeStatus)
        {
            inEf = new InEffect(width, height);
            outEf = new OutEffect(width, height);
        }

        public void Update()
        {

        }

        public void Draw()
        {
            inEf.Draw();
            if(_closing)
            {
                outEf.Draw();
            }
        }
    }
}
