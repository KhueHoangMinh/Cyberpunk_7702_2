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
        Queue<MinusHealth> MinusHealthAdd;
        int MinusHealthPop = 0;
        Queue<Trace> traces;
        Queue<Trace> TraceAdd;
        int TracePop = 0;
        Queue<Explosion> explosions;
        Queue<Explosion> ExploAdd;
        int ExploPop = 0;
        Queue<Smoke> smokes;
        Queue<Smoke> SmokeAdd;
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

        static string[] splitMsg(string p2dstring)
        {
            p2dstring = p2dstring.Trim();
            return p2dstring.Split(',');
        }

        static string StringifyBullets(List<Bullet> bullets)
        {
            string res = "";
            foreach (Bullet bullet in bullets) 
            { 
                res += bullet.ToString(); 
            }
            return res;
        }

        public GameStage(Manager manager) : base(manager)
        {
            camera = new Camera(this.Manager.Window.Width, this.Manager.Window.Height);
            player = new Player(this, camera, new Point2D() { X = this.Manager.Window.Width / 2 - 30, Y = 50 }, 100, 100, this.Manager.Gun, this.Manager.Skin, this.Manager.Skill);
            enemy = new Player(this, camera, new Point2D() { X = this.Manager.Window.Width / 2+30, Y = 50 }, 100, 100, "Gun 6", "Pink", this.Manager.Skill);
            //enemy = new Player(this, camera, new Point2D() { X = this.Manager.Window.Width / 2 - 30, Y = 50 }, 100, 100, this.Manager.Gun, this.Manager.Skin, this.Manager.Skill);
            //player = new Player(this, camera, new Point2D() { X = this.Manager.Window.Width / 2 + 30, Y = 50 }, 100, 100, "Gun 6", "Pink", this.Manager.Skill);
            enemy.HostControl = false;
            enemies = new List<Enemy>();
            grounds = new List<Ground>();
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width / 2, Y = this.Manager.Window.Height }, this.Manager.Window.Width, 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = -50, Y = this.Manager.Window.Height / 2 }, 100, this.Manager.Window.Height + 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width + 50, Y = this.Manager.Window.Height / 2 }, 100, this.Manager.Window.Height + 100, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width / 2, Y = 780 }, 300, 50, Color.Brown));
            bullets = new List<Bullet>();
            minusHealths = new Queue<MinusHealth>();
            traces = new Queue<Trace>();
            explosions = new Queue<Explosion>();
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

            Queue<string> buffer = new Queue<string>();

            int sequence = 0;


            // Listener ==========================================================================================

            new Thread(() =>
            {
                const int listenPort = 11000;


                IPAddress broadcast = IPAddress.Parse("192.168.1.255");
                IPAddress playerIP = IPAddress.Parse("192.168.1.8");
                IPAddress hostIP = IPAddress.Parse("192.168.1.12");


                IPEndPoint ep1 = new IPEndPoint(hostIP, 11000);
                IPEndPoint ep2 = new IPEndPoint(playerIP, 11000);

                UdpClient listener = new UdpClient(listenPort);
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

                long prevTicks = 0;


                int mismatch = 0;



                try
                {

                    while (true)
                    {

                        byte[] bytes = listener.Receive(ref groupEP);
                        string msg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                        if (server)
                        {
                            //Console.WriteLine(msg);

                            string[] received = splitMsg(msg);
                            //Console.WriteLine("move player: " + msg);
                            if (received[0][0] == '1')
                            {
                                enemy.MoveLeft();
                            }
                            else if (received[0][1] == '1')
                            {
                                enemy.MoveRight();
                            }
                            if (received[0][2] == '1' && !enemy.Jumped)
                            {
                                enemy.Jump();
                            }
                            if (received[0][3] == '1')
                            {
                                enemy.Gun.Shoot();
                            }
                            enemy.Gun.AimPoint = new Point2D() { X = Double.Parse(received[1]), Y = Double.Parse(received[2]) };
                        }
                        else
                        {
                            //Console.WriteLine(msg);

                            string[] received = splitMsg(msg);
                            string[] buffered = null;
                            if(buffer.Count > 0) buffered = splitMsg(buffer.Dequeue());
                            //Console.WriteLine("move player: " + msg);
                            if (received[0] == "0")
                            {
                                if (received[4][0] == '1')
                                {
                                    enemy.MoveLeft();
                                }
                                else if (received[4][1] == '1')
                                {
                                    enemy.MoveRight();
                                }
                                if (received[4][2] == '1')
                                {
                                    enemy.Jump();
                                }
                                if (received[4][3] == '1')
                                {
                                    enemy.Gun.Shoot();
                                }
                                if(buffered != null)
                                {
                                    Console.WriteLine(int.Parse(received[3]));

                                    Console.WriteLine(int.Parse(buffered[3]));

                                    if (int.Parse(received[3]) == int.Parse(buffered[3]))
                                    {
                                        if (Math.Abs(double.Parse(received[8]) - double.Parse(buffered[1])) >= 2 || Math.Abs(double.Parse(received[9])) - (int)(double.Parse(buffered[2])) >= 2)
                                        { 
                                            sequence = int.Parse(received[3]) + 1;
                                            enemy.Pos = new Point2D() { X = Double.Parse(received[1]), Y = Double.Parse(received[2]) };
                                            player.Pos = new Point2D() { X = Double.Parse(received[8]), Y = Double.Parse(received[9]) };
                                            buffer = new Queue<string>();
                                            Console.WriteLine("Diff: " + received[8] + " " + received[9] + " " + buffered[1] + " " + buffered[2]);
                                        } else
                                        {
                                            Console.WriteLine("EQ");
                                        }
                                    }
                                    else if (int.Parse(received[3]) < int.Parse(buffered[3]))
                                    {
                                        sequence = int.Parse(received[3]) + 1;
                                        enemy.Pos = new Point2D() { X = Double.Parse(received[1]), Y = Double.Parse(received[2]) };
                                        player.Pos = new Point2D() { X = Double.Parse(received[8]), Y = Double.Parse(received[9]) };
                                        buffer = new Queue<string>();
                                        Console.WriteLine("Smaller: " + received[3] + " " + buffered[3]);
                                    }
                                    else if(int.Parse(received[3]) > int.Parse(buffered[3]))
                                    {
                                        sequence = int.Parse(received[3]) + 1;
                                        enemy.Pos = new Point2D() { X = Double.Parse(received[1]), Y = Double.Parse(received[2]) };
                                        player.Pos = new Point2D() { X = Double.Parse(received[8]), Y = Double.Parse(received[9]) };
                                        buffer = new Queue<string>();
                                        Console.WriteLine("Larger: " + received[3] + " " + buffered[3]);
                                        //while (long.Parse(received[3]) > long.Parse(buffered[3]) && buffer.Count > 0)
                                        //{
                                        //    buffered = splitMsg(buffer.Dequeue());
                                        //}
                                    }

                                }
                                
                                //if (
                                //long.Parse(received[3]) > prevTicks
                                ////true
                                //)
                                //{
                                //    //prevTicks = long.Parse(received[2]);
                                //    //enemy.Pos = new Point2D() { X = Double.Parse(received[1]), Y = Double.Parse(received[2]) };
                                //    enemy.Gun.AimPoint = new Point2D() { X = Double.Parse(received[5]), Y = Double.Parse(received[6]) };
                                //}
                                //else
                                //{
                                //    mismatch++;
                                //    Console.WriteLine(mismatch.ToString());
                                //}
                            //}
                                //if (
                                //long.Parse(received[10]) > prevTicks
                                ////true
                                //)
                                //{
                                //    prevTicks = long.Parse(received[10]);
                                //    //player.Pos = new Point2D() { X = Double.Parse(received[8]), Y = Double.Parse(received[9]) };
                                //}
                                //else
                                //{
                                //    mismatch++;
                                //    Console.WriteLine(mismatch.ToString());
                                //}
                            }
                        }
                        //sequence++;
                        //Thread.Sleep(15);
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

            // Sender ==========================================================================================

            new Thread(() =>
            {

                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress broadcast = IPAddress.Parse("192.168.1.255");
                IPAddress playerIP = IPAddress.Parse("192.168.1.8");
                IPAddress hostIP = IPAddress.Parse("192.168.1.12");


                IPEndPoint ep1 = new IPEndPoint(hostIP, 11000);
                IPEndPoint ep2 = new IPEndPoint(playerIP, 11000);



                if (server)
                {
                    while (true)
                    {
                        int dir1 = 0; int dir2 = 0; int dir3 = 0; int shoot = 0;
                        if (SplashKit.KeyDown(KeyCode.AKey))
                        {
                            dir1 = 1;
                            player.MoveLeft();
                        }
                        else if (SplashKit.KeyDown(KeyCode.DKey))
                        {
                            dir2 = 1;
                            player.MoveRight();
                        }
                        if (SplashKit.KeyDown(KeyCode.WKey))
                        {
                            dir3 = 1;
                            player.Jump();
                        }
                        if (SplashKit.MouseDown(MouseButton.LeftButton))
                        {
                            shoot = 1;
                            player.Gun.Shoot();
                        }
                        byte[] sendbuf = Encoding.ASCII.GetBytes(
                            "0" + "," + player.Pos.X.ToString() + "," + player.Pos.Y.ToString() + "," + sequence.ToString() + "," + dir1.ToString() + dir2.ToString() + dir3.ToString() + shoot.ToString() + "," +
                            (SplashKit.MousePosition().X + camera.Pos.X).ToString() + "," + (SplashKit.MousePosition().Y + camera.Pos.Y).ToString() + "," +

                            "1" + "," + enemy.Pos.X.ToString() + "," + enemy.Pos.Y.ToString() + "," + sequence.ToString()

                            );
                        s.SendTo(sendbuf, ep2);
                        if (dir1 + dir2 + dir3 != 0)
                        {
                            //sendbuf = Encoding.ASCII.GetBytes(dir1.ToString() + dir2.ToString() + dir3.ToString());
                            //s.SendTo(sendbuf, ep2);
                        }
                        sequence++;
                        Thread.Sleep(15);
                    }
                }
                else
                {
                    while (true)
                    {
                        buffer.Enqueue(
                            "0" + "," + player.Pos.X.ToString() + "," + player.Pos.Y.ToString() + "," + sequence.ToString() + "," +

                            "1" + "," + enemy.Pos.X.ToString() + "," + enemy.Pos.Y.ToString() + "," + sequence.ToString()
                            );
                        int dir1 = 0; int dir2 = 0; int dir3 = 0; int shoot = 0;
                        if (SplashKit.KeyDown(KeyCode.AKey))
                        {
                            dir1 = 1;
                            player.MoveLeft();
                        }
                        else if (SplashKit.KeyDown(KeyCode.DKey))
                        {
                            dir2 = 1;
                            player.MoveRight();
                        }
                        if (SplashKit.KeyDown(KeyCode.WKey))
                        {
                            dir3 = 1;
                            player.Jump();
                        }
                        if (SplashKit.MouseDown(MouseButton.LeftButton))
                        {
                            shoot = 1;
                            player.Gun.Shoot();
                        }
                        byte[] sendbuf = Encoding.ASCII.GetBytes(
                            //"1" + "," + player.Pos.X.ToString() + "," + player.Pos.Y.ToString() + "," + sequence.ToString() + "," + 
                            dir1.ToString() + dir2.ToString() + dir3.ToString() + shoot.ToString() + "," +
                           (SplashKit.MousePosition().X + camera.Pos.X).ToString() + "," + (SplashKit.MousePosition().Y + camera.Pos.Y).ToString());
                        s.SendTo(sendbuf, ep1);
                        //if(dir1 + dir2 + dir3 != 0)
                        //{
                        //    byte[] sendbuf = Encoding.ASCII.GetBytes(dir1.ToString() + dir2.ToString() + dir3.ToString());
                        //    s.SendTo(sendbuf, ep1);
                        //}
                        sequence++;
                        Thread.Sleep(15);
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
                if(TraceAdd.Count > 0) traces.Enqueue(TraceAdd.Dequeue());
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
                if (SmokeAdd.Count > 0) smokes.Enqueue(SmokeAdd.Dequeue());
                foreach (Smoke smoke in smokes)
                {
                    smoke.Update();
                }
                while (SmokePop > 0 && smokes.Count > 0)
                {
                    smokes.Dequeue();
                    SmokePop--;
                }
                if (ExploAdd.Count > 0) explosions.Enqueue(ExploAdd.Dequeue());
                foreach (Explosion explosion in explosions)
                {
                    explosion.Update();
                }
                while (ExploPop > 0 && explosions.Count > 0)
                {
                    explosions.Dequeue();
                    ExploPop--;
                }
                if (MinusHealthAdd.Count > 0) minusHealths.Enqueue(MinusHealthAdd.Dequeue());
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

        public Player Enemy
        {
            get { return enemy; }
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
            MinusHealthPop++;
        }

        public void EndGame()
        {
            this.Closing = true;
        }
    }
}
