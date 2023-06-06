using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Cyberpunk77022
{
    public class GameStage
    {
        Manager _manager;
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
        int round = 0;
        long _clearAt;
        bool _resting = true;
        long _restTime = 1000000;
        Button pauseBtn;
        Button resumeBtn;
        Button quitBtn;
        Color _background;
        bool paused = false;

        public GameStage(Manager manager)
        {
            _manager = manager;
            inEf = new InEffect(_manager.Window.Width, _manager.Window.Height);
            outEf = new OutEffect(_manager.Window.Width, _manager.Window.Height);
            camera = new Camera(_manager.Window.Width, _manager.Window.Height);
            player = new Player(this, camera, new Point2D() { X = _manager.Window.Width/2, Y = 50}, 100, 100, Color.Blue);
            enemies = new List<Enemy>();
            grounds = new List<Ground>();
            grounds.Add(new Ground(camera, new Point2D() { X = _manager.Window.Width / 2, Y = _manager.Window.Height }, _manager.Window.Width, 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = _manager.Window.Width / 2, Y = 780 }, 300, 50, Color.Brown));
            bullets = new List<Bullet>();
            traces = new Queue<Trace>();
            explosions = new Queue<Explosion>();
            smokes = new Queue<Smoke>();
            _clearAt = DateTime.UtcNow.Ticks;
            pauseBtn = new Button("||", Color.Red, _manager.Window.Width-80, 80, 70, 70);
            resumeBtn = new Button("Resume", Color.Green, _manager.Window.Width / 2, _manager.Window.Height/2-50, 250, 150);
            quitBtn = new Button("QUIT", Color.Red, _manager.Window.Width / 2, _manager.Window.Height/2 + 150, 250, 150);
            _manager.Score = 0;
            _background = Color.Black;
            _background.A = 0.5f;
        }

        public void Update()
        {
            if(!_closing && !paused)
            {
                if(enemies.Count == 0)
                {
                    if(_resting)
                    {
                        round++;
                        _clearAt = DateTime.UtcNow.Ticks;
                        _resting = false;
                    } else
                    {
                        if(DateTime.UtcNow.Ticks - _clearAt >= _restTime)
                        {
                            for(int i = 0; i < Math.Ceiling((decimal)round/3); i++)
                            {
                                enemies.Add(new NormalEnemy(this, camera, new Point2D() { X = new Random().Next(10,1000), Y = new Random().Next(10, 100) }, 50, 50, Color.Red));
                            }
                            _resting = true;
                        }
                    }
                }
                foreach (Trace trace in traces)
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
                if(player.Health <= 0) { 
                    this.EndGame();
                }

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
                pauseBtn.Update();
                if (pauseBtn.Hovering)
                {
                    if (SplashKit.MouseDown(MouseButton.LeftButton))
                    {
                        paused = !paused;
                    }
                }
            } else
            {
                if (outEf._completed)
                {
                    _manager.NewEnd();
                }
                if(paused)
                {
                    resumeBtn.Update();
                    quitBtn.Update();
                    if(SplashKit.MouseDown(MouseButton.LeftButton))
                    {
                        if(resumeBtn.Hovering)
                        {
                            paused = false;
                        } else if (quitBtn.Hovering)
                        {
                            this.EndGame();
                        }
                    }
                }
            }
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
            SplashKit.DrawText(_manager.Score.ToString(), Color.White, "font", 50, _manager.Window.Width / 2 - SplashKit.TextWidth(_manager.Score.ToString(), "font", 50) / 2, 20);
            pauseBtn.Draw();
            if(paused)
            {
                SplashKit.FillRectangle(_background, 0, 0, _manager.Window.Width, _manager.Window.Height);
                SplashKit.FillRectangle(Color.Gray,_manager.Window.Width/2-300, _manager.Window.Height/2-300,600,600);
                SplashKit.DrawText("Paused", Color.White, "font", 60, _manager.Window.Width / 2 - SplashKit.TextWidth("Paused", "font", 60) / 2, _manager.Window.Height / 2 - 250);
                resumeBtn.Draw();
                quitBtn.Draw();
            }

            inEf.Draw();
            if (_closing)
            {
                outEf.Draw();
            }
        }
        public Manager Manager { get { return _manager; } }
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
            _manager.Score++;
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

        public void EndGame()
        {
            _closing = true;
        }
    }
}
