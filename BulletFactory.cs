using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace Cyberpunk77022
{
    public interface BulletFactory
    {
        public Bullet CreateBullet(GameStage game, Gun gun, Color color);
    }

    public class NormalBulletFactory : BulletFactory
    {
        public Bullet CreateBullet(GameStage game, Gun gun, Color color)
        {
            return new NormalBullet(game, gun, gun.Range, gun.Speed, gun.Damage, color);
        }
    }
    public class SniperBulletFactory : BulletFactory
    {
        public Bullet CreateBullet(GameStage game, Gun gun, Color color)
        {
            return new SniperBullet(game, gun, gun.Range, gun.Speed, gun.Damage);
        }
    }
    public class RPGBulletFactory : BulletFactory
    {
        public Bullet CreateBullet(GameStage game, Gun gun, Color color)
        {
            return new RPGBullet(game, gun, gun.Range, 10, gun.Damage);
        }
    }

}
