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
        Queue<Trace> traces;
        Queue<Explosion> explosions;
        Camera camera;

        public GameStage(Window window, int width, int height, Action<string> ChangeStatus)
        {
            inEf = new InEffect(width, height);
            outEf = new OutEffect(width, height);
            camera = new Camera(width, height);
            player = new Player(this,window, camera, new Point2D() { X = 50, Y = 50}, 100, 100, Color.Blue);
            grounds = new List<Ground>();
            grounds.Add(new Ground(camera, new Point2D() { X = width / 2, Y = height }, width, 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = width / 2, Y = 780 }, 300, 10, Color.Brown));
            bullets = new List<Bullet>();
            traces = new Queue<Trace>();
            explosions = new Queue<Explosion>();
            
        }

        public void Update()
        {
            foreach(Trace trace in traces)
            {
                trace.Update();
            }
            foreach (Trace trace in traces)
            {
                if (trace.GetColor.A <= 0.005)
                {
                    traces.Dequeue();
                    break;
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
                    bullets[i].IsCollided = true;
                    bullets.Remove(bullets[i]);
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                for (int j = 0; j < grounds.Count; j++)
                {
                    if (grounds[j].IsCollided(bullets[i].Pos))
                    {
                        bullets[i].IsCollided = true;
                        explosions.Enqueue(new Explosion(camera, new Random().Next(8, 10), new Random().Next(30, 50), new Point2D() { 
                            X = (double)new Random().Next((int)bullets[i].Pos.X - 10, (int)bullets[i].Pos.X + 10),
                            Y = (double)new Random().Next((int)bullets[i].Pos.Y - 10, (int)bullets[i].Pos.Y + 10),
                        }, Color.Random()));
                        bullets.Remove(bullets[i]);
                        break;
                    }
                }
            }
            player.Update(grounds, bullets);
            foreach (Explosion explosion in explosions)
            {
                explosion.Update();
            }
            foreach (Explosion explosion in explosions)
            {
                if (explosion.GetColor.A <= 0.02)
                {
                    explosions.Dequeue();
                    break;
                }
            }
            camera.Update(player.Pos);
        }

        public Player GetPlayer
        {
            get { return player; }
        }

        public void Draw()
        {
            foreach (Trace trace in traces)
            {
                trace.Draw();
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

            foreach (Explosion explosion in explosions)
            {
                explosion.Draw();
            }
            inEf.Draw();
            if (_closing)
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
            traces.Enqueue(trace);
        }

        public void RemoveTrace(Trace trace)
        {
            traces.Dequeue();
        }
        public void AddExplosion(Explosion explosion)
        {
            explosions.Enqueue(explosion);
        }

        public void RemoveExplosion(Explosion explosion)
        {
            explosions.Dequeue();
        }
    }
}
