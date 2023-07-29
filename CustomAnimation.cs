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

    public class CalAnimation : CustomAnimation
    {
        Color _color;
        float _fade = 0.01f;
        float delta;
        float beta;
        float sinAngle;
        float cosAngle;

        public CalAnimation(GameStage game, Color color, Point2D pos, float width, float height) : base(game, pos, width, height)
        {
            delta = (float)((Math.Sqrt(2) * this.width / 2));
            beta = (float)(this.angle - Math.Atan(1));
            sinAngle = (float)Math.Sin(this.angle);
            cosAngle = (float)Math.Cos(this.angle);
            _color = color;
            _color.A = 0.6f;
        }
        public CalAnimation(GameStage game, Color color, float fade, float angle, float expand, float velX, float velY, Point2D pos, float width, float height) : base(game, pos, width, height)
        {
            this.angle = angle;
            _fade = fade;
            delta = (float)((Math.Sqrt(2) * this.width / 2));
            beta = (float)(this.angle - Math.Atan(1));
            sinAngle = (float)Math.Sin(this.angle);
            cosAngle = (float)Math.Cos(this.angle);
            _color = color;
            _color.A = 0.6f;
            this.expand = expand;
            this.velX = velX;
            this.velY = velY;
            this.rotation = 8;
        }
        public Quad calQuad()
        {
            delta = (float)((Math.Sqrt(2) * this.width / 2));
            float x = (float)this.pos.X - delta * (float)Math.Cos(beta) - (float)this.game.Camera.Pos.X;
            float y = (float)this.pos.Y + delta * (float)Math.Sin(beta) - (float)this.game.Camera.Pos.Y;
            float heightxcos = this.width * cosAngle;
            float heightxsin = this.width * sinAngle;
            float widthxcos = this.width * cosAngle;
            float widthxsin = this.width * sinAngle;
            return new Quad()
            {
                Points = new Point2D[4] {
                    new Point2D() { X = x, Y = y},
                    new Point2D() {
                        X = x + heightxcos,
                        Y = y - heightxsin
                    },
                    new Point2D() {
                        X = x + widthxsin,
                        Y = y + widthxcos
                    },
                    new Point2D() {
                        X = x + widthxsin+ heightxcos,
                        Y = y + widthxcos - heightxsin
                    }
                }
            };
        }

        public override void Update()
        {
            if (this.isFloating)
            {
                this.pos.Y -= 3;
            }
            this.pos.X += this.velX;
            this.pos.Y += this.velY;
            this.velX *= 0.96f;
            this.velY *= 0.96f;
            this.angle = 360 % (this.angle + this.rotation);
            this.width += this.expand;
            this.expand *= this.a;
            if (_color.A >= _fade)
            {
                _color.A -= _fade;
            }
            else
            {
                this.isPlaying = false;
            }
        }
        public override void Draw()
        {
            if(this.isPlaying) SplashKit.FillQuad(_color, calQuad());
        }
    }

    public class PreLoadedAnimation : CustomAnimation
    {
        Bitmap _bitmap;
        DrawingOptions _drawingOptions;
        int _startCell = 0;
        int _endCell = 0;
        float _drawingcell = 0;
        bool _loop;
        bool _reverse = false;

        public PreLoadedAnimation(GameStage game, Bitmap bitmap, int startCell, int endCell, bool loop, Point2D pos, float width, float height) : base(game, pos, width, height)
        {
            _bitmap = bitmap;
            _startCell = startCell;
            _endCell = endCell;
            _drawingcell = _startCell;
            _loop = loop;
            this.scale = (float)(this.width * 1.0 / _bitmap.CellWidth);
            _drawingOptions = new DrawingOptions()
            {
                Dest = game.Manager.Window,
                ScaleX = this.scale,
                ScaleY = this.scale,
                DrawCell = (int)_drawingcell,
                Angle = 0
            };
        }


        public PreLoadedAnimation(GameStage game, Bitmap bitmap, int startCell, int endCell, bool loop, bool reverse, float angle, Point2D pos, float width, float height) : base(game, pos, width, height)
        {
            _bitmap = bitmap;
            _startCell = startCell;
            _endCell = endCell;
            _drawingcell = _startCell;
            _loop = loop;
            _reverse = reverse;
            this.angle = angle;
            this.scale = (float)(this.width * 1.0 / _bitmap.CellWidth);
            _drawingOptions = new DrawingOptions()
            {
                Dest = game.Manager.Window,
                ScaleX = this.scale,
                ScaleY = this.scale,
                DrawCell = (int)_drawingcell,
                FlipY = _reverse,
                Angle = 0
            };
        }

        public PreLoadedAnimation(GameStage game, Bitmap bitmap, int startCell, int endCell, bool loop, float expand, float velX, float velY, Point2D pos, float width, float height) : base(game, pos, width, height)
        {
            _bitmap = bitmap;
            _startCell = startCell;
            _endCell = endCell;
            _drawingcell = _startCell;
            _loop = loop;
            this.scale = (float)(this.width * 1.0 / _bitmap.CellWidth);
            _drawingOptions = new DrawingOptions()
            {
                Dest = this.game.Manager.Window,
                ScaleX = this.scale,
                ScaleY = this.scale,
                DrawCell = (int)_drawingcell,
                Angle = 0
            };
            this.expand = expand;
            this.velX = velX;
            this.velY = velY;
        }
        public PreLoadedAnimation(GameStage game, Bitmap bitmap, int startCell, int endCell, bool loop, bool isFloating, float expand, float velX, float velY, Point2D pos, float width, float height) : base(game, pos, width, height)
        {
            _bitmap = bitmap;
            _startCell = startCell;
            _endCell = endCell;
            _drawingcell = _startCell;
            _loop = loop;
            this.isFloating = isFloating;
            this.scale = (float)(this.width * 1.0 / _bitmap.CellWidth);
            _drawingOptions = new DrawingOptions()
            {
                Dest = this.game.Manager.Window,
                ScaleX = this.scale,
                ScaleY = this.scale,
                DrawCell = (int)_drawingcell,
                Angle = 0
            };
            this.expand = expand;
            this.velX = velX;
            this.velY = velY;
        }

        public override void Update()
        {
            _drawingcell += 0.3f;
            if(this.isFloating)
            {
                this.pos.Y -= 3;
            }
            this.pos.X += this.velX;
            this.pos.Y += this.velY;
            this.velX *= 0.96f;
            this.velY *= 0.96f;
            this.angle = (this.angle + this.rotation) % 360;
            this.width += this.expand;
            this.expand *= this.a;
            this.scale = (float)(this.width * 1.0 / _bitmap.CellWidth);
            _drawingOptions.Angle = this.angle;
            _drawingOptions.ScaleX = this.scale;
            _drawingOptions.ScaleY = this.scale;
            if (_drawingcell > _endCell + 0.3f)
            {
                if(_loop)
                {
                    _drawingcell = _startCell;
                } else
                {
                    //this.game.RemoveExplosion();
                    this.isPlaying = false;
                }
            }
            else
            {
                _drawingOptions.DrawCell = (int)_drawingcell;
            }
        }
        public override void Draw()
        {
            if (this.isPlaying) SplashKit.DrawBitmap(_bitmap, this.pos.X - _bitmap.CellWidth / 2 - this.game.Camera.Pos.X, this.pos.Y - _bitmap.CellHeight / 2 - this.game.Camera.Pos.Y, _drawingOptions);
        }
    }

    public abstract class CustomAnimation
    {
        public GameStage game;
        public bool isPlaying = true;
        public float angle = 0;
        public float rotation = 0;
        public float scale = 1;
        public float expand = 0;
        public float a = 0.9f;
        public float velX = 0;
        public float velY = 0;
        public bool isFloating = false;
        public Point2D pos;
        public float width;
        public float height;

        public CustomAnimation(GameStage Game, Point2D Pos, float Width, float Height) 
        {
            game = Game;
            pos = Pos;
            width = Width;
            height = Height;
        }

        public abstract void Update();

        public abstract void Draw();
    }
}
