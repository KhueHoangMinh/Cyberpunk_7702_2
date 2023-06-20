using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public abstract class Stage
    {
        Manager _manager;
        InEffect inEf;
        OutEffect outEf;
        bool _closing = false;
        public Stage(Manager manager) 
        { 
            _manager = manager;
            inEf = new InEffect(0, 0, _manager.Window.Width, _manager.Window.Height);
            outEf = new OutEffect(0, 0, _manager.Window.Width, _manager.Window.Height);
        }

        public abstract void Update();

        public abstract void Draw();

        public Manager Manager { get { return _manager; } }

        public InEffect InEf 
        {
            get { return inEf; }
            set { InEf = value;  }
        }

        public OutEffect OutEf
        {
            get { return outEf; }
            set { outEf = value; }
        }

        public bool Closing
        {
            get { return _closing; }
            set { _closing = value; }
        }
    }
}
