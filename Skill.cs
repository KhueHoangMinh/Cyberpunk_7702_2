using System;
using System.Collections.Generic;
using SplashKitSDK;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public abstract class Skill : ShopItem
    {
        string _name;
        string _id;
        string _desc;
        int _price;
        bool purchased = false;
        public Skill(string name, string id, string desc, int price)
        {
            _name = name;
            _id = id;
            _desc = desc;
        }

        public abstract void Graphic(float x, float y);

        public abstract void InGameGraphic(float x, float y);

        public string Name { get { return _name; } }
        public string ID { get { return _id; } }
        public string Description { get { return _desc; } }
        public int Price { get { return _price; } }
        public bool Purchased
        {
            get { return purchased; }
            set { purchased = value; }
        }

    }
}

