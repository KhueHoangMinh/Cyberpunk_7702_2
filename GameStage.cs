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
        List<Enemy> enemies;
        List<Ground> grounds;
        List<Bullet> bullets;
        Queue<Trace> traces;
        int TracePop = 0;
        Queue<Explosion> explosions;
        int ExploPop = 0;
        Queue<Smoke> smokes;
        int SmokePop = 0;
        Camera camera;

        public GameStage(Window window, int width, int height, Action<string> ChangeStatus)
        {
            inEf = new InEffect(width, height);
            outEf = new OutEffect(width, height);
            camera = new Camera(width, height);
            player = new Player(this,window, camera, new Point2D() { X = width/2, Y = 50}, 100, 100, Color.Blue);
            enemies = new List<Enemy>()
            {
                new NormalEnemy(this,window, camera, new Point2D() { X = 300, Y = 50}, 50, 50, Color.Red),
                new NormalEnemy(this,window, camera, new Point2D() { X = 100, Y = 50}, 50, 50, Color.Red),
                new NormalEnemy(this,window, camera, new Point2D() { X = 500, Y = 100}, 50, 50, Color.Red),
                new NormalEnemy(this,window, camera, new Point2D() { X = 800, Y = 50}, 50, 50, Color.Red),
                new NormalEnemy(this,window, camera, new Point2D() { X = 1000, Y = 0}, 50, 50, Color.Red),
            };
            grounds = new List<Ground>();
            grounds.Add(new Ground(camera, new Point2D() { X = width / 2, Y = height }, width, 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = width / 2, Y = 780 }, 300, 50, Color.Brown));
            bullets = new List<Bullet>();
            traces = new Queue<Trace>();
            explosions = new Queue<Explosion>();
            smokes = new Queue<Smoke>();

        }

        public void Update()
        {
            foreach(Trace trace in traces)
            {
                trace.Update();
            }
            while (TracePop > 0 && traces.Count > 0)
            {
                traces.Dequeue();
                TracePop--;
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                for (int j = 0; j < grounds.Count; j++)
                {
                    if (grounds[j].IsCollided(bullets[i].Pos))
                    {
                        bullets[i].IsCollided = true;
                        explosions.Enqueue(new Explosion(this, camera, new Random().Next(20, 25), new Random().Next(40, 60), new Point2D() { 
                            X = (double)new Random().Next((int)bullets[i].Pos.X - 10, (int)bullets[i].Pos.X + 10),
                            Y = (double)new Random().Next((int)bullets[i].Pos.Y - 10, (int)bullets[i].Pos.Y + 10),
                        }, Color.Random()));
                        bullets.Remove(bullets[i]);
                        break;
                    }
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    if (enemies[j].IsCollided(bullets[i].Pos) && bullets[i].Gun.GunOf is Player)
                    {
                        bullets[i].IsCollided = true;
                        enemies[j].GetHit(bullets[i]);
                        explosions.Enqueue(new Explosion(this, camera, new Random().Next(20, 25), new Random().Next(40, 60), new Point2D()
                        {
                            X = (double)new Random().Next((int)bullets[i].Pos.X - 10, (int)bullets[i].Pos.X + 10),
                            Y = (double)new Random().Next((int)bullets[i].Pos.Y - 10, (int)bullets[i].Pos.Y + 10),
                        }, Color.Random()));
                        bullets.Remove(bullets[i]);
                        break;
                    }
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                if (player.IsCollided(bullets[i].Pos) && bullets[i].Gun.GunOf is Enemy)
                {
                    bullets[i].IsCollided = true;
                    player.GetHit(bullets[i]);
                    explosions.Enqueue(new Explosion(this, camera, new Random().Next(20, 25), new Random().Next(40, 60), new Point2D()
                    {
                        X = (double)new Random().Next((int)bullets[i].Pos.X - 10, (int)bullets[i].Pos.X + 10),
                        Y = (double)new Random().Next((int)bullets[i].Pos.Y - 10, (int)bullets[i].Pos.Y + 10),
                    }, Color.Random()));
                    bullets.Remove(bullets[i]);
                    break;
                }
            }
                player.Update(grounds, bullets);

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(grounds, bullets);
            }
            foreach (Smoke smoke in smokes)
            {
                smoke.Update();
            }
            while (SmokePop > 0 && smokes.Count > 0)
            {
                smokes.Dequeue();
                SmokePop--;
            }
            foreach (Explosion explosion in explosions)
            {
                explosion.Update();
            }
            while (ExploPop > 0 && explosions.Count > 0)
            {
                explosions.Dequeue();
                ExploPop--;
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
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw();
            }
            for (int i = 0; i < grounds.Count; i++)
            {
                grounds[i].Draw();
            }
            player.DrawHealth();
            player.DrawGun();

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].DrawHealth();
                enemies[i].DrawGun();
            }

            foreach (Smoke smoke in smokes)
            {
                smoke.Draw();
            }
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
        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
        }

        public void AddTrace(Trace trace)
        {
            traces.Enqueue(trace);
        }

        public void RemoveTrace()
        {
            TracePop++;
        }
        public void AddExplosion(Explosion explosion)
        {
            explosions.Enqueue(explosion);
        }

        public void RemoveExplosion()
        {
            ExploPop++;
        }
        public void AddSmoke(Smoke smoke)
        {
            smokes.Enqueue(smoke);
        }

        public void RemoveSmoke()
        {
            SmokePop++;
        }
    }
}
