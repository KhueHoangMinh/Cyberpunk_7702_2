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
    public class Info
    {
        float _x;
        float _y;
        string _name;
        string _content;
        public Info(string name, string content) 
        {
            _name = name;
            _content = content;
        }

        public void Update(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public void Draw()
        {
            SplashKit.DrawText(_name, Color.White, "font", 70, _x - SplashKit.TextWidth(_name, "font", 70) / 2, _y - SplashKit.TextHeight(_name, "font", 70) - 2);
            SplashKit.DrawText(_content, Color.White, "font", 40, _x - SplashKit.TextWidth(_content, "font", 40) / 2, _y + 2);
        }

        public float Top
        {
            get { return _y - SplashKit.TextHeight(_name, "font", 70) - 2; }
        }

        public float Bottom
        {
            get { return _y + SplashKit.TextHeight(_content, "font", 40) + 2; }
        }

        public float Height
        {
            get { return -this.Top + this.Bottom; }
        }
    }

    public class Content
    {
        float _x;
        float _y = 0;
        List<Info> _info;

        public Content(float x, float y, List<Info> info)
        {
            _x = x;
            _y = y;
            _info = info;
        }

        public void Update()
        {
            for (int i = 0; i < _info.Count; i++)
            {
                _info[i].Update(_x,_y + i * (_info[i].Height + 20));
            }
            _y -= 1;
        }

        public void Draw()
        {
            for(int i = 0; i < _info.Count; i++)
            {
                _info[i].Draw();
            }
        }
    }

    public class AboutStage : Stage
    {
        Content _content;
        List<Info> _info;
        Button backBtn;
        string _nextState;
        public AboutStage(Manager manager) : base(manager)
        {
            _info = new List<Info>()
            {
                new Info("Developer","Hoang Minh Khue"),
                new Info("Sounds", "pixabay.com"),
                new Info("Images", "pngegg.com")
            };
            _content = new Content(this.Manager.Window.Width/2, this.Manager.Window.Height,_info);
            backBtn = new Button("<", Color.Red, 80, 80, 70, 70);
        }

        public override void Update()
        {
            backBtn.Update();
            _content.Update();
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (backBtn.Hovering)
                {
                    this.Closing = true;
                    _nextState = "home";
                }
            }
            if (this.OutEf._completed)
            {
                if (_nextState == "home")
                {
                    this.Manager.NewHome();
                }
            }

            this.InEf.Update();
            if (this.Closing)
            {
                this.OutEf.Update();
            }
        }

        public override void Draw()
        {

            backBtn.Draw();
            _content.Draw();

            this.InEf.Draw();
            this.OutEf.Draw();
        }
    }
}

