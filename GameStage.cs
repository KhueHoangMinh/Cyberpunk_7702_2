using Cyberpunk77022.skills;
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
        float _coinScale = (float)(20.0 / SplashKit.BitmapNamed("coin_animation").Width);
        List<Enemy> enemies;
        List<Ground> grounds;
        List<Bullet> bullets;
        List<Coin> coins;
        NormalEnemyFactory _normalEnemyFactory;
        FlyEnemyFactory _flyEnemyFactory;
        BigEnemyFactory _bigEnemyFactory;
        Queue<MinusHealth> minusHealths;
        Queue<MinusHealth> MinusHealthAdd;
        int MinusHealthPop = 0;
        Queue<Trace> traces;
        Queue<Trace> TraceAdd;
        int TracePop = 0;
        List<CustomAnimation> animations;
        Queue<CustomAnimation> AniAdd;
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
        float drawingcell = 0;

        public GameStage(Manager manager) : base(manager)
        {
            camera = new Camera(this.Manager.Window.Width, this.Manager.Window.Height);
            _normalEnemyFactory = new NormalEnemyFactory(this);
            _flyEnemyFactory = new FlyEnemyFactory(this);
            _bigEnemyFactory = new BigEnemyFactory(this);
            player = new Player(this, camera, new Point2D() { X = this.Manager.Window.Width/2, Y = -100}, 100, 100);
            enemies = new List<Enemy>();
            grounds = new List<Ground>();
            minusHealths = new Queue<MinusHealth>();
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width / 2, Y = this.Manager.Window.Height }, this.Manager.Window.Width, 300, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width / 2, Y = -300 }, this.Manager.Window.Width + 600, 300, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = -150, Y = this.Manager.Window.Height / 2 }, 300, this.Manager.Window.Height + 300, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width + 150, Y = this.Manager.Window.Height / 2 }, 300, this.Manager.Window.Height + 300, Color.Brown));
            grounds.Add(new Ground(camera, new Point2D() { X = this.Manager.Window.Width / 2, Y = 780 }, 300, 50, Color.Brown));
            bullets = new List<Bullet>();
            coins = new List<Coin>();
            traces = new Queue<Trace>();
            animations = new List<CustomAnimation>();
            MinusHealthAdd = new Queue<MinusHealth>();
            TraceAdd = new Queue<Trace>();
            AniAdd = new Queue<CustomAnimation>();
            _clearAt = DateTime.UtcNow.Ticks;
            pauseBtn = new Button("||", Color.Red, this.Manager.Window.Width-60, 60, 60, 60);
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
                                    enemies.Add(_normalEnemyFactory.CreateEnemy());
                                } else if(rand > 4 && rand < 8)
                                {
                                    enemies.Add(_flyEnemyFactory.CreateEnemy());
                                } else
                                {
                                    enemies.Add(_bigEnemyFactory.CreateEnemy());
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
                for (int i = 0; i < coins.Count; i++)
                {
                    coins[i].Update();
                }
                player.Update();
                if(player.Health <= 0) { 
                    this.EndGame();
                }

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Update();
                }
                while(AniAdd.Count > 0) animations.Add(AniAdd.Dequeue());
                int m = 0;
                while (m < animations.Count)
                {
                    if (animations[m].isPlaying)
                    {
                        animations[m].Update();
                    } else 
                    {
                        animations.Remove(animations[m]);
                        m--;
                    }
                    m++;
                }
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
                    traces.Count.ToString() + " " + animations.Count.ToString());
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
        }

        public void DrawUI()
        {
            SplashKit.DrawText(this.Manager.Score.ToString(), Color.White, "font", 50, this.Manager.Window.Width / 2 - SplashKit.TextWidth(this.Manager.Score.ToString(), "font", 50) / 2, 20);
            pauseBtn.Draw();
            if (paused)
            {
                SplashKit.FillRectangle(_background, 0, 0, this.Manager.Window.Width, this.Manager.Window.Height);
                SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 0.6), this.Manager.Window.Width / 2 - 300, this.Manager.Window.Height / 2 - 300, 600, 600);
                SplashKit.DrawText("Paused", Color.White, "font", 60, this.Manager.Window.Width / 2 - SplashKit.TextWidth("Paused", "font", 60) / 2, this.Manager.Window.Height / 2 - 250);
                resumeBtn.Draw();
                quitBtn.Draw();
            }


            SplashKit.FillRectangle(Color.Gray, 30, 30, 200, 70);

            SplashKit.FillRectangle(Color.Black, 40, 40, 50, 50);
            SplashKit.FillRectangle(Color.Black, 105, 40, 50, 50);
            SplashKit.FillRectangle(Color.Black, 170, 40, 50, 50);

            if(player.Gun == this.Manager.SelectedGun[0])
            {
                SplashKit.FillRectangle(Color.RGBAColor(1, 1, 1, 0.1), 40, 40, 50, 50);
                SplashKit.DrawRectangle(Color.White,38,38,54,54);
            } else
            if (player.Gun == this.Manager.SelectedGun[1])
            {
                SplashKit.FillRectangle(Color.RGBAColor(1, 1, 1, 0.1), 105, 40, 50, 50);
                SplashKit.DrawRectangle(Color.White, 103, 38, 54, 54);
            }

            SplashKit.DrawText("1", Color.White, "font", 14, 41, 41);
            SplashKit.DrawText("2", Color.White, "font", 14, 106, 41);
            SplashKit.DrawText("power", Color.White, "font", 12, 171, 41);

            SplashKit.DrawBitmap(this.Manager.SelectedGun[0].Bitmap,
                65 - this.Manager.SelectedGun[0].Bitmap.Width / 2,
                65 - this.Manager.SelectedGun[0].Bitmap.Height / 2,
                new DrawingOptions()
                {
                    Dest = this.Manager.Window,
                    ScaleX = (float)45.0 / this.Manager.SelectedGun[0].Bitmap.Width,
                    ScaleY = (float)45.0 / this.Manager.SelectedGun[0].Bitmap.Width
                }
            );

            if(this.Manager.SelectedGun[1] != null)
            {
                SplashKit.DrawBitmap(this.Manager.SelectedGun[1].Bitmap,
                    130 - this.Manager.SelectedGun[1].Bitmap.Width / 2,
                    65 - this.Manager.SelectedGun[1].Bitmap.Height / 2,
                    new DrawingOptions()
                    {
                        Dest = this.Manager.Window,
                        ScaleX = (float)45.0 / this.Manager.SelectedGun[1].Bitmap.Width,
                        ScaleY = (float)45.0 / this.Manager.SelectedGun[1].Bitmap.Width
                    }
                );
            }

            if(this.Manager.Skill != null) this.Manager.Skill.InGameGraphic(195, 65);

            SplashKit.FillRectangle(Color.Gray, 30, 110, 200, 30);
            SplashKit.FillRectangle(Color.Green, 30, 110, 200 * player.Health / player.MaxHealth, 30);
            SplashKit.FillRectangle(Color.RGBAColor(255, 0, 0, 90), 30 + 200 * player.Health / player.MaxHealth , 110, 200 * player.MinusHealth / player.MaxHealth, 30);

            SplashKit.DrawText(((int)player.Health).ToString() + "/" + player.MaxHealth.ToString(), Color.White, "font", 20,
                130 - SplashKit.TextWidth(((int)player.Health).ToString() + "/" + player.MaxHealth.ToString(), "font", 20)/2,
                125 - SplashKit.TextHeight(((int)player.Health).ToString(), "font", 20)/2
            );
            drawingcell += 0.05f;
            if (drawingcell > SplashKit.BitmapCellCount(SplashKit.BitmapNamed("coin_animation"))) drawingcell = 0;
            SplashKit.DrawBitmap(
                SplashKit.BitmapNamed("coin_animation"),
                30 - SplashKit.BitmapNamed("coin_animation").Width / 2 + 10, 150 - SplashKit.BitmapNamed("coin_animation").Width / 2 + 10,
                new DrawingOptions()
                {
                    Dest = this.Manager.Window,
                    ScaleX = _coinScale,
                    ScaleY = _coinScale,
                    DrawCell = (int)drawingcell
                }
            );
            SplashKit.DrawText(this.Manager.Coin.ToString(), Color.Yellow, "font", 20,
                55,
                150
            );


            this.InEf.Draw();
            this.OutEf.Draw();
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
            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].Draw();
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
            foreach (CustomAnimation animation in animations)
            {
                animation.Draw();
            }
            foreach (MinusHealth minusHealth in minusHealths)
            {
                minusHealth.Draw();
            }
            DrawUI();
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
        public void AddCoin(Coin coin)
        {
            coins.Add(coin);
        }
        public void RemoveCoin(Coin coin)
        {
            coins.Remove(coin);
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
        public void AddAnimation(CustomAnimation animation)
        {
            AniAdd.Enqueue(animation);
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
            this.Manager.Save("userdata.txt");
            this.Closing = true;
        }
    }
}
