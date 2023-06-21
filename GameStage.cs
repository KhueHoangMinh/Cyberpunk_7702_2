using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
        Player enemy;
        List<Enemy> enemies;
        List<Ground> grounds;
        List<Bullet> bullets;
        Queue<MinusHealth> minusHealths;
        int MinusHealthPop = 0;
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
        long _restTime = 5000000;
        Button pauseBtn;
        Button resumeBtn;
        Button quitBtn;
        Color _background;
        bool paused = false;
        bool server = true;

        static string[] ToDouble(string p2dstring)
        {
            p2dstring = p2dstring.Trim();
            string[] res;
            string sPosX = "";
            string sPosY = "";
            string ticks = "";
            bool doneX = false;
            bool doneY = false;

            for (int i = 0; i < p2dstring.Length; i++)
            {
                if (p2dstring[i] == ' ' && !doneX)
                {
                    doneX = true;
                } else if(p2dstring[i] == ' ' && doneX)
                {
                    doneY = true;
                } else
                {
                    if(!doneX)
                    {
                        sPosX += p2dstring[i];
                    } else if (doneX && !doneY)
                    {
                        sPosY += p2dstring[i];
                    } else if (doneX && doneY)
                    {
                        ticks += p2dstring[i];
                    }
                }
            }
            res = new string[3] { sPosX, sPosY,ticks };
            return res;
        }

        public GameStage(Manager manager) : base(manager)
        {
            camera = new Camera(this.Manager.Window.Width, this.Manager.Window.Height);
            player = new Player(this, camera, new Point2D() { X = this.Manager.Window.Width/2, Y = 50}, 100, 100, this.Manager.Gun, this.Manager.Skin, this.Manager.Skill);
            enemy = new Player(this, camera, new Point2D() { X = this.Manager.Window.Width / 2, Y = 50 }, 100, 100, "Gun 6", "Pink", this.Manager.Skill);
            enemies = new List<Enemy>();
            grounds = new List<Ground>();
            minusHealths = new Queue<MinusHealth>();
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width / 2, Y = this.Manager.Window.Height }, this.Manager.Window.Width, 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = -50, Y = this.Manager.Window.Height / 2 }, 100, this.Manager.Window.Height + 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width + 50, Y = this.Manager.Window.Height / 2 }, 100, this.Manager.Window.Height + 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width / 2, Y = 780 }, 300, 50, Color.Brown));
            bullets = new List<Bullet>();
            traces = new Queue<Trace>();
            explosions = new Queue<Explosion>();
            smokes = new Queue<Smoke>();
            _clearAt = DateTime.UtcNow.Ticks;
            pauseBtn = new Button("||", Color.Red, this.Manager.Window.Width-80, 80, 70, 70);
            resumeBtn = new Button("Resume", Color.Green, this.Manager.Window.Width / 2, this.Manager.Window.Height/2-50, 250, 150);
            quitBtn = new Button("QUIT", Color.Red, this.Manager.Window.Width / 2, this.Manager.Window.Height/2 + 150, 250, 150);
            this.Manager.Score = 0;
            _background = Color.Black;
            _background.A = 0.5f;

            // Listener

            new Thread(() =>
            {
                const int listenPort = 11000;

                UdpClient listener = new UdpClient(listenPort);
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

                long prevTicks = 0;

                try
                {
                    while (true)
                    {
                        Console.WriteLine("Waiting for broadcast");
                        byte[] bytes = listener.Receive(ref groupEP);
                        if (server)
                        {

                            string msg = $" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}";
                            Console.WriteLine($"Key pressed from {groupEP} : " + msg);
                        }
                        else
                        {

                            string msg = $" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}";
                            Console.WriteLine($"Data from from {groupEP} : " + msg);

                            string[] received = ToDouble(msg);
                            if (long.Parse(received[2]) > prevTicks)
                            {
                                prevTicks = long.Parse(received[2]);
                                player.Pos = new Point2D() { X = Double.Parse(received[0]), Y = Double.Parse(received[1]) };
                            }
                        }
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    listener.Close();
                }

            }).Start();

            // Sender

            new Thread(() =>
            {

                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress broadcast = IPAddress.Parse("192.168.1.255");

                IPEndPoint ep = new IPEndPoint(broadcast, 11000);


                if (server)
                {
                    while (true)
                    {
                        byte[] sendbuf = Encoding.ASCII.GetBytes(player.Pos.X.ToString() + " " + player.Pos.Y.ToString() + " " + DateTime.UtcNow.Ticks.ToString());
                        s.SendTo(sendbuf, ep);
                        Thread.Sleep((int)0 / 10000);
                    }
                }
                else
                {
                    while (true)
                    {
                        if (SplashKit.KeyDown(KeyCode.AKey))
                        {
                            byte[] sendbuf = Encoding.ASCII.GetBytes("A");
                            s.SendTo(sendbuf, ep);
                        }
                        else if (SplashKit.KeyDown(KeyCode.DKey))
                        {
                            byte[] sendbuf = Encoding.ASCII.GetBytes("D");
                            s.SendTo(sendbuf, ep);
                        }
                        else
                        if (SplashKit.KeyDown(KeyCode.WKey))
                        {
                            byte[] sendbuf = Encoding.ASCII.GetBytes("W");
                            s.SendTo(sendbuf, ep);
                        }
                        Thread.Sleep((int)0 / 10000);
                    }
                }

            }).Start();
        }

        public override void Update()
        {
            if(!this.Closing && !paused)
            {
                //if(enemies.Count == 0)
                //{
                //    if(_resting)
                //    {
                //        round++;
                //        _clearAt = DateTime.UtcNow.Ticks;
                //        _resting = false;
                //    } else
                //    {
                //        if(DateTime.UtcNow.Ticks - _clearAt >= _restTime)
                //        {
                //            for(int i = 0; i < Math.Ceiling((decimal)round/3); i++)
                //            {
                //                enemies.Add(new NormalEnemy(this, camera, new Point2D() { X = new Random().Next(10,1000), Y = new Random().Next(10, 100) }, 50, 50, Color.Red));
                //            }
                //            _resting = true;
                //        }
                //    }
                //}
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
                enemy.Update(grounds, bullets);
                if (player.Health <= 0) { 
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
                foreach (MinusHealth minusHealth in minusHealths)
                {
                    minusHealth.Update();
                }
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
            enemy.Draw();
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
            enemy.DrawHealth();
            enemy.DrawGun();

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
        public void AddMinusHealth(MinusHealth minusHealth)
        {
            minusHealths.Enqueue(minusHealth);
        }

        public void RemoveMinusHealth()
        {
            MinusHealthPop++;
        }

        public void EndGame()
        {
            this.Closing = true;
        }
    }
}
