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
        Player player;
        List<Ground> grounds;
        Camera camera;

        public GameStage(Window window, int width, int height, Action<string> ChangeStatus)
        {
            inEf = new InEffect(width, height);
            outEf = new OutEffect(width, height);
            camera = new Camera(width, height);
            player = new Player(camera, new Point2D() { X = 50, Y = 50}, 100, 100, Color.AliceBlue);
            Ground ground1 = new Ground(camera, new Point2D() { X = width / 2, Y = height }, width, 400, Color.Brown);
            grounds = new List<Ground>();
            grounds.Add(ground1);
        }

        public void Update()
        {
            player.Update(grounds);
            camera.Update(player.Pos);
            //for (int i = 0; i < grounds.Count; i++)
            //{
            //    grounds[i].Update(camera);
            //}
        }

        public Player GetPlayer
        {
            get { return player; }
        }

        public void Draw()
        {
            player?.Draw();
            for(int i = 0; i < grounds.Count; i++)
            {
                grounds[i].Draw();
            }
            inEf.Draw();
            if(_closing)
            {
                outEf.Draw();
            }
        }
    }
}
