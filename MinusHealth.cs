using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace Cyberpunk77022
{
    public class MinusHealth
    {
        float _minusHealth;
        Color _color;
        Point2D _init;
        float _fontSize = 20;
         
        GameStage _game;
        public MinusHealth(GameStage game, Object minused, float minusHealth) { 
            _minusHealth = minusHealth;
            _game = game;
            _color = Color.Red;
            _init = new Point2D();
            _init.X = minused.Pos.X;
            _init.Y = minused.Top - 20;

        }

        public void Update()
        {
            _init.Y -= 0.3;
            _fontSize -= 0.5f;
            if (_fontSize < 0)
            {
                _fontSize = 0;
                _game.RemoveMinusHealth();
            }
        }

        public void Draw()
        {

            SplashKit.DrawText(
                "-" + _minusHealth.ToString(),
                _color,
                "font",
                (int)_fontSize,
                _init.X - SplashKit.TextWidth("-" + _minusHealth.ToString(), "font", (int)_fontSize) / 2 - _game.Camera.Pos.X,
                _init.Y - SplashKit.TextHeight("-" + _minusHealth.ToString(), "font", (int)_fontSize) / 2 - _game.Camera.Pos.Y
            );
        }
    }
}
