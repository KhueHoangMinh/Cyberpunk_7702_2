using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cyberpunk77022
{
    public interface EnemyState
    {
        public Enemy Host { get; }
        public void Behaviour();
    }

    public abstract class State : EnemyState
    {
        Enemy _host;

        public State(Enemy host) { _host = host; }
        public abstract void Behaviour();

        public Enemy Host
        {
            get { return _host; }
        }
    }

    public class DyingState : State
    {
        public DyingState(Enemy host) : base (host)
        {

        }

        public override void Behaviour()
        {
            Color pale = Host.Color;
            pale.A -= 0.01f;
            Host.Color = pale;
            if (Host.Color.A <= 0.3)
            {
                if (new Random().Next(0, 15) < 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Host.Game.AddSmoke(new Smoke(Host.Game, Host.Game.Camera, new Random().Next(8, 15), new Random().Next(20, 50), new Point2D()
                        {
                            X = (double)new Random().Next((int)Host.Left, (int)Host.Right),
                            Y = (double)new Random().Next((int)Host.Top, (int)Host.Bottom),
                        }, Color.White, 0, 0));
                    }
                }
            }
        }
    }

    public class LivingState : State
    {
        public LivingState(Enemy host) : base(host)
        {

        }

        public override void Behaviour()
        {
            Host.Behaviour();
        }
    }
}
