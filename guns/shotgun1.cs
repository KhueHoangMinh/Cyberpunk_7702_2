using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cyberpunk77022
{
    public class Shotgun1 : Gun
    {
        public Shotgun1(GameStage game, Object GunOf, float damage) : base(game,GunOf,"gun6","singleshot",80,0,damage,5000000,1)
        {
        }
        //public virtual void ShootAction()
        //{
        //    Bullet NewBullet = new Bullet(this.GunOf.Game, this, 40, this.Damage);
        //    for (int i = 0; i < 3; i++)
        //    {
        //        _game.AddExplosion(new Explosion(_game, _camera, new Random().Next(8, 10), new Random().Next(30, 50), new Point2D()
        //        {
        //            X = (double)new Random().Next((int)NewBullet.InitPos.X - 10, (int)NewBullet.InitPos.X + 10),
        //            Y = (double)new Random().Next((int)NewBullet.InitPos.Y - 10, (int)NewBullet.InitPos.Y + 10),
        //        }, Color.Random()));
        //    }
        //    _game.AddBullet(NewBullet);
        //    _game.AddTrace(new Trace(_game, _window, _camera, NewBullet));
        //}
    }

}
