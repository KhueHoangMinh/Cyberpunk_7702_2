using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public interface EnemyFactory
    {
        public Enemy CreateEnemy();
    }

    public class NormalEnemyFactory : EnemyFactory
    {
        GameStage _game;
        public NormalEnemyFactory(GameStage game)
        {
            _game = game;
        }

        public Enemy CreateEnemy()
        {
            return new NormalEnemy(_game, _game.Camera, new Point2D() { X = new Random().Next(10, 2000), Y = new Random().Next(10, 100) });
        }
    }
    public class FlyEnemyFactory : EnemyFactory
    {
        GameStage _game;
        public FlyEnemyFactory(GameStage game)
        {
            _game = game;
        }

        public Enemy CreateEnemy()
        {
            return new FlyEnemy(_game, _game.Camera, new Point2D() { X = new Random().Next(10, 2000), Y = new Random().Next(10, 100) });
        }
    }
    public class BigEnemyFactory : EnemyFactory
    {
        GameStage _game;
        public BigEnemyFactory(GameStage game)
        {
            _game = game;
        }

        public Enemy CreateEnemy()
        {
            return new BigEnemy(_game, _game.Camera, new Point2D() { X = new Random().Next(10, 2000), Y = new Random().Next(10, 100) });
        }
    }

}
