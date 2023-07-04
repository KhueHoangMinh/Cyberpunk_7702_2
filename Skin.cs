using System;
using System.Collections.Generic;
using SplashKitSDK;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Skin : ShopItem
    {
        string _name;
        string _id;
        string _desc;
        Color _graphic;
        int _price;
        bool purchased = false;
        public Skin(string name, string id, string desc, int price, Color graphic)
        {
            _name = name;
            _id = id;
            _desc = desc;
            _price = price;
            _graphic = graphic;
        }

        public void Graphic(float x, float y)
        {
            SplashKit.FillRectangle(_graphic,x-50,y-50,100,100);
        }

        public string Name { get { return _name; } }
        public string ID { get { return _id; } }
        public string Description { get { return _desc; } }
        public int Price { get { return _price; } }
        public bool Purchased { 
            get {  return purchased; }
            set { purchased = value; }
        }

        public Color Color { get { return _graphic; } }

    }
}
