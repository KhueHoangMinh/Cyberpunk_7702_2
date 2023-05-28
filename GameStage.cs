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
        List<Bullet> bullets;
        List<Trace> traces;
        Camera camera;

        public GameStage(Window window, int width, int height, Action<string> ChangeStatus)
        {
            inEf = new InEffect(width, height);
            outEf = new OutEffect(width, height);
            camera = new Camera(width, height);
            player = new Player(this,window, camera, new Point2D() { X = 50, Y = 50}, 100, 100, Color.Blue);
            grounds = new List<Ground>();
            grounds.Add(new Ground(camera, new Point2D() { X = width / 2, Y = height }, width, 400, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = width / 2, Y = 780 }, 300, 50, Color.Brown));
            bullets = new List<Bullet>();
            traces = new List<Trace>();
        }

        public void Update()
        {
            for (int i = 0; i < traces.Count; i++)
            {
                traces[i].Update();
            }
            for (int i = 0; i < traces.Count; i++)
            {
                if (traces[i].GetColor.A <= 0.005)
                {
                    traces.Remove(traces[i]);
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Pos.X < -10000 || bullets[i].Pos.X > 10000 || bullets[i].Pos.Y < -10000 || bullets[i].Pos.Y > 10000)
                {
                    bullets.Remove(bullets[i]);
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                for (int j = 0; j < grounds.Count; j++)
                {
                    if (grounds[j].IsCollided(bullets[i].Pos))
                    {
                        bullets.Remove(bullets[i]);
                        break;
                    }
                }
            }
            player.Update(grounds, bullets);
            camera.Update(player.Pos);
        }

        public Player GetPlayer
        {
            get { return player; }
        }

        public void Draw()
        {
            for (int i = 0; i < traces.Count; i++)
            {
                traces[i].Draw();
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw();
            }
            player.Draw();
            for(int i = 0; i < grounds.Count; i++)
            {
                grounds[i].Draw();
            }
            player.DrawGun();
            inEf.Draw();
            if(_closing)
            {
                outEf.Draw();
            }
        }

        public void AddBullet(Bullet bullet)
        {
            bullets.Add(bullet);
        }

        public void RemoveBullet(Bullet bullet) 
        {  
            bullets.Remove(bullet);
        }

        public void AddTrace(Trace trace)
        {
            traces.Add(trace);
        }

        public void RemoveTrace(Trace trace)
        {
            traces.Remove(trace);
        }
    }
}
