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
    public class GameStage : Stage
    {
        Player player;
        List<Enemy> enemies;
        List<Ground> grounds;
        List<Bullet> bullets;
        Queue<MinusHealth> minusHealths;
        Queue<MinusHealth> MinusHealthAdd;
        int MinusHealthPop = 0;
        Queue<Trace> traces;
        Queue<Trace> TraceAdd;
        int TracePop = 0;
        List<Explosion> explosions;
        Queue<Explosion> ExploAdd;
        int ExploPop = 0;
        Queue<Smoke> smokes;
        Queue<Smoke> SmokeAdd;
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

        public GameStage(Manager manager) : base(manager)
        {
            camera = new Camera(this.Manager.Window.Width, this.Manager.Window.Height);
            player = new Player(this, camera, new Point2D() { X = this.Manager.Window.Width/2, Y = 50}, 100, 100);
            enemies = new List<Enemy>();
            grounds = new List<Ground>();
            minusHealths = new Queue<MinusHealth>();
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width / 2, Y = this.Manager.Window.Height }, this.Manager.Window.Width, 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = -50, Y = this.Manager.Window.Height / 2 }, 100, this.Manager.Window.Height + 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width + 50, Y = this.Manager.Window.Height / 2 }, 100, this.Manager.Window.Height + 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width / 2, Y = 780 }, 300, 50, Color.Brown));
            bullets = new List<Bullet>();
            traces = new Queue<Trace>();
            explosions = new List<Explosion>();
            smokes = new Queue<Smoke>();
            MinusHealthAdd = new Queue<MinusHealth>();
            TraceAdd = new Queue<Trace>();
            ExploAdd = new Queue<Explosion>();
            SmokeAdd = new Queue<Smoke>();
            _clearAt = DateTime.UtcNow.Ticks;
            pauseBtn = new Button("||", Color.Red, this.Manager.Window.Width-80, 80, 70, 70);
            resumeBtn = new Button("Resume", Color.Green, this.Manager.Window.Width / 2, this.Manager.Window.Height/2-50, 250, 150);
            quitBtn = new Button("QUIT", Color.Red, this.Manager.Window.Width / 2, this.Manager.Window.Height/2 + 150, 250, 150);
            this.Manager.Score = 0;
            _background = Color.Black;
            _background.A = 0.5f;
        }

        public override void Update()
        {
            if(!this.Closing && !paused)
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
                                int rand = new Random().Next(1, 10);
                                if (rand < 4)
                                {
                                    enemies.Add(new NormalEnemy(this, camera, new Point2D() { X = new Random().Next(10, 1000), Y = new Random().Next(10, 100) }));
                                } else if(rand > 4 && rand < 8)
                                {
                                    enemies.Add(new FlyEnemy(this, camera, new Point2D() { X = new Random().Next(10, 1000), Y = new Random().Next(10, 100) }));
                                } else
                                {
                                    enemies.Add(new BigEnemy(this, camera, new Point2D() { X = new Random().Next(10, 1000), Y = new Random().Next(10, 100) }));
                                }
                            }
                            _resting = true;
                        }
                    }
                }
                while (TraceAdd.Count > 0) traces.Enqueue(TraceAdd.Dequeue());
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
                player.Update(grounds, bullets);
                if(player.Health <= 0) { 
                    this.EndGame();
                }

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Update(grounds, bullets);
                }
                while (SmokeAdd.Count > 0) smokes.Enqueue(SmokeAdd.Dequeue());
                foreach (Smoke smoke in smokes)
                {
                    smoke.Update();
                }
                while (SmokePop > 0 && smokes.Count > 0)
                {
                    smokes.Dequeue();
                    SmokePop--;
                }
                while (ExploAdd.Count > 0) explosions.Add(ExploAdd.Dequeue());
                foreach (Explosion explosion in explosions)
                {
                    explosion.Update();
                }
                int m = 0;
                while (m < explosions.Count)
                {
                    if (explosions[m].GetColor.A < 0.01)
                    {
                        explosions.Remove(explosions[m]);
                        m--;
                    }
                    m++;
                }
                //while (ExploPop > 0 && explosions.Count > 0)
                //{
                //    explosions.Dequeue();
                //    ExploPop--;
                //}
                foreach (MinusHealth minusHealth in minusHealths)
                {
                    minusHealth.Update();
                }
                while (MinusHealthAdd.Count > 0) minusHealths.Enqueue(MinusHealthAdd.Dequeue());
                while (MinusHealthPop > 0 && minusHealths.Count > 0)
                {
                    minusHealths.Dequeue();
                    MinusHealthPop--;
                }
                camera.Update(player.Gun.Nozzle);
                pauseBtn.Update();
                if (pauseBtn.Hovering)
                {
                    if (SplashKit.MouseDown(MouseButton.LeftButton))
                    {
                        paused = !paused;
                    }
                }
                Console.WriteLine(enemies.Count.ToString() + " " + grounds.Count.ToString() + " " + bullets.Count.ToString() + " " + minusHealths.Count.ToString() + " " + 
                    traces.Count.ToString() + " " + explosions.Count.ToString() + " " + smokes.Count.ToString() + " ");
            } else
            {
                if (this.OutEf._completed)
                {
                    this.Manager.NewEnd();
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
            this.InEf.Update();
            if (this.Closing)
            {
                this.OutEf.Update();
            }
            //Console.WriteLine(bullets.Count.ToString() + " " + traces.Count.ToString());
        }

        public override void Draw()
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
            foreach (MinusHealth minusHealth in minusHealths)
            {
                minusHealth.Draw();
            }
            SplashKit.DrawText(this.Manager.Score.ToString(), Color.White, "font", 50, this.Manager.Window.Width / 2 - SplashKit.TextWidth(this.Manager.Score.ToString(), "font", 50) / 2, 20);
            pauseBtn.Draw();
            if(paused)
            {
                SplashKit.FillRectangle(_background, 0, 0, this.Manager.Window.Width, this.Manager.Window.Height);
                SplashKit.FillRectangle(Color.RGBAColor(0,0,0,0.6),this.Manager.Window.Width/2-300, this.Manager.Window.Height/2-300,600,600);
                SplashKit.DrawText("Paused", Color.White, "font", 60, this.Manager.Window.Width / 2 - SplashKit.TextWidth("Paused", "font", 60) / 2, this.Manager.Window.Height / 2 - 250);
                resumeBtn.Draw();
                quitBtn.Draw();
            }

            this.InEf.Draw();
            this.OutEf.Draw();
        }

        public Camera Camera { get { return camera; } }

        public Player GetPlayer
        {
            get { return player; }
        }

        public List<Ground> Grounds { 
            get { return grounds; }
        }
        public List<Enemy> Enemies
        {
            get { return enemies; }
        }
        public List<Bullet> Bullets
        {
            get { return bullets; }
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
            this.Manager.Score++;
            this.Manager.Coin++;
        }


        public void AddTrace(Trace trace)
        {
            TraceAdd.Enqueue(trace);
        }

        public void RemoveTrace()
        {
            TracePop++;
        }
        public void AddExplosion(Explosion explosion)
        {
            ExploAdd.Enqueue(explosion);
        }

        public void RemoveExplosion()
        {
            ExploPop++;
        }
        public void AddSmoke(Smoke smoke)
        {
            SmokeAdd.Enqueue(smoke);
        }

        public void RemoveSmoke()
        {
            SmokePop++;
        }
        public void AddMinusHealth(MinusHealth minusHealth)
        {
            MinusHealthAdd.Enqueue(minusHealth);
        }

        public void RemoveMinusHealth()
        {
            
        }

        public void EndGame()
        {
            this.Manager.Save("../../../userdata.txt");
            this.Closing = true;
        }
    }
}
