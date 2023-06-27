using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Cyberpunk77022
{

    public abstract class Item
    {
        string _type;
        Page _page;
        public string _name;
        int _price;
        public float _width;
        public float _height;
        float _xOnPage;
        float _yOnPage;
        float _Scale;
        public float x;
        public float y;
        Button buyButton;
        Button selectButton;
        Button deselectButton;
        string _state = "not_purchased";

        public Item(Page page, ItemBrief info, float width, float height, float xOnPage, float yOnPage)
        {
            _page = page;
            _type = info.type;
            _name = info.name;
            _price = info.price;
            _width = width;
            _height = height;
            _xOnPage = xOnPage;
            _yOnPage = yOnPage;
            x = _xOnPage;
            y = _yOnPage;   
            buyButton = new Button(_price.ToString(), Color.Red, x, y + 380, 200, 80);
            selectButton = new Button("Select", Color.Green, x, y + 380, 200, 80);
            deselectButton = new Button("Selected", Color.Yellow, x, y + 380, 200, 80);
        }

        public void Update()
        {
            if (_page.Shop.Manager.Gun == _name || _page.Shop.Manager.Skin == _name || _page.Shop.Manager.Skill == _name)
            {
                _state = "selected";
            }

            y = _page.Y + _page.ScrollY + _yOnPage;
            buyButton.Y = y + 180;
            selectButton.Y = y + 180;
            deselectButton.Y = y + 180;
            switch (_state)
            {
                case "not_purchased":
                    buyButton.Update();
                    if(buyButton.Hovering && SplashKit.MouseClicked(MouseButton.LeftButton) && SplashKit.MousePosition().Y > _page.Y)
                    {
                        if(_page.Shop.Manager.Coin > _price)
                        {
                            _state = "purchased";
                            _page.Shop.Manager.Coin -= _price;
                            _page.Shop.Save();
                        }
                    }
                    break;
                case "purchased":
                    selectButton.Update();
                    if (selectButton.Hovering && SplashKit.MouseClicked(MouseButton.LeftButton) && SplashKit.MousePosition().Y > _page.Y)
                    {
                        _page.Deselect(this);
                        switch(_type)
                        {
                            case "weapon":
                                _page.Shop.Manager.Gun = _name;
                                break;
                            case "skin":
                                _page.Shop.Manager.Skin = _name;
                                break;
                            case "skill":
                                _page.Shop.Manager.Skill = _name;
                                break;
                        }
                        _state = "selected";
                        _page.Shop.Save();
                    }
                    break;
                case "selected":
                    deselectButton.Update();
                    if (deselectButton.Hovering && SplashKit.MouseClicked(MouseButton.LeftButton) && SplashKit.MousePosition().Y > _page.Y)
                    {

                    }
                    break;
            }
        }

        public void Draw()
        {
            SplashKit.FillRectangle(Color.RGBColor(20,20,20), x - _width / 2, y - _height / 2, _width, _height);
            SplashKit.FillRectangle(Color.Black, x - _width / 2 + 20, y - _height / 2 + 20, _width - 40, _height/2 - 20);

            DrawGraphic();

            SplashKit.DrawText(_name, Color.White, "font", 60, x - SplashKit.TextWidth(_name, "font", 60) / 2, y + 20);

            switch (_state )
            {
                case "not_purchased":
                    buyButton.Draw();
                    break;
                case "purchased":
                    selectButton.Draw();
                    break;
                case "selected":
                    deselectButton.Draw();
                    break;
            }
        }

        public abstract void DrawGraphic();

        public void Deselect()
        {
            if(_state == "selected")
            {
                _state = "purchased";
            }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
    }

    public class WeaponItem : Item
    {
        Bitmap _image;
        float _scale;
        DrawingOptions opts;
        public WeaponItem(Page page, ItemBrief info, float width, float height, float XOnPage, float YOnPage) : base(page,info,width,height,XOnPage,YOnPage) {
            _image = SplashKit.BitmapNamed((info as WeaponBrief).image);
            _scale = 300 * 1.0f / _image.Width;
            opts = new DrawingOptions()
            {
                Dest = page.Shop.Manager.Window,
                ScaleX = _scale,
                ScaleY = _scale,
            };
        }

        public override void DrawGraphic()
        {
            SplashKit.DrawBitmap(_image, x - _image.Width / 2, y - _width / 4 - _image.Height / 2, opts);
        }
    }
    public class SkinItem : Item
    {
        Color _color;
        public SkinItem(Page page, ItemBrief info, float width, float height, float XOnPage, float YOnPage) : base(page, info, width, height, XOnPage, YOnPage)
        {
            _color = (info as SkinBrief).color;
        }

        public override void DrawGraphic()
        {
            SplashKit.FillRectangle(_color, x - 50, y - _width / 4 - 50, 100,100);
        }
    }


    public class SkillItem : Item
    {
        public SkillItem(Page page, ItemBrief info, float width, float height, float XOnPage, float YOnPage) : base(page, info, width, height, XOnPage, YOnPage)
        {
        }

        public override void DrawGraphic()
        {
            switch(_name)
            {
                case "Health":
                    SplashKit.FillRectangle(Color.Green, x - 20, y - _width / 4 - 60, 40, 120);
                    SplashKit.FillRectangle(Color.Green, x - 60, y - _width / 4 - 20, 120, 40);
                    break;
                case "Defense":
                    SplashKit.FillRectangle(Color.DarkGray, x - 50, y - _width / 4 - 50, 100, 100);
                    break;
            }
        }
    }

    public class Page
    {
        ShopStage _shop;
        List<Item> _items;
        InEffect inEf;
        OutEffect outEf;
        bool _closing = false;
        float _height;
        float _actualHeight;
        float _scrollY = 0;
        float _y;

        public Page (ShopStage shop, ItemBrief[] items, float height, float y)
        {
            _shop = shop; 
            _height = height;
            _items = new List<Item>();
            inEf = new InEffect(0, _y, _shop.Manager.Window.Width, _shop.Manager.Window.Height);
            outEf = new OutEffect(0, _y, _shop.Manager.Window.Width, _shop.Manager.Window.Height);
            _y = y;
            float width = _shop.Manager.Window.Width / 3;
            for (int i = 0; i < items.Length; i++)
            {
                int column = i % 3 + 1;
                int row = (int)Math.Ceiling((decimal)((i+1) * 1.0 / 3));
                switch(items[i].type)
                {
                    case "weapon":
                        _items.Add(new WeaponItem(this, items[i], width - 40, width - 40, (width) * (column - 1) + width / 2, (width) * (row - 1) + width / 2));
                        break;
                    case "skin":
                        _items.Add(new SkinItem(this, items[i], width - 40, width - 40, (width) * (column - 1) + width / 2, (width) * (row - 1) + width / 2));
                        break;
                    case "skill":
                        _items.Add(new SkillItem(this, items[i], width - 40, width - 40, (width) * (column - 1) + width / 2, (width) * (row - 1) + width / 2));
                        break;
                }
            }
            int rowCount = (int)Math.Ceiling((decimal)(_items.Count * 1.0 / 3));
            _actualHeight = rowCount * width;
            if( _actualHeight < _height )
            {
                _actualHeight = _height;
            }
        }

        public void Update()
        {
            _scrollY += (float)SplashKit.MouseWheelScroll().Y*30;
            if(_scrollY >= 0)
            {
                _scrollY = 0;
            } else if(_scrollY <= -_actualHeight + _height)
            {
                _scrollY = -_actualHeight + _height;
            }
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Update();
            }
            inEf.Update();
            if(_closing)
            {
                outEf.Update();
            }
        }

        public void Draw()
        {
            for(int i = 0; i < _items.Count; i++)
            {
                _items[i].Draw();
            }
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 30), _shop.Manager.Window.Width - 10, Y, 10, _height);
            SplashKit.FillRectangle(Color.Gray, _shop.Manager.Window.Width - 10, Y + -_scrollY * _height / _actualHeight, 10, _height * _height / _actualHeight);
            inEf.Draw();
            outEf.Draw();
        }

        public void Deselect(Item except)
        {
            for(int i = 0; i < _items.Count; i++)
            {
                if (_items[i] != except)
                {
                    _items[i].Deselect();
                }
            }
        }

        public void ResetEffect()
        {
            _closing = false;
            inEf = new InEffect(0, _y, _shop.Manager.Window.Width, _shop.Manager.Window.Height);
            outEf = new OutEffect(0, _y, _shop.Manager.Window.Width, _shop.Manager.Window.Height);
        }

        public bool Closing
        {
            get
            {
                return _closing;
            }
            set { _closing = value; }
        }

        public bool EffectComplete
        {
            get
            {
                return outEf._completed;
            }
        }

        public float ScrollY
        {
            get { return _scrollY; }
        }

        public float Y
        {
            get { return _y; }
        }

        public List<Item> Items
        {
            get { return _items; }
        }

        public ShopStage Shop { get { return _shop; } }
    }
    public abstract class ItemBrief
    {
        public string type;
        public string name;
        public string description;
        public int price;
        public int state = 0;

        public ItemBrief(string type, string name, string description, int price, int state)
        {
            this.type = type;
            this.name = name;
            this.description = description;
            this.price = price;
            this.state = state;
        }
    }

    public class WeaponBrief : ItemBrief
    {
        public string image;

        public WeaponBrief(string type, string name, string description, int price, int state, string image) : base(type, name, description, price,state)
        {
            this.image = image;
        }
    }


    public class SkinBrief : ItemBrief
    {
        public Color color;

        public SkinBrief(string type, string name, string description, int price, int state, Color color) : base(type, name, description, price, state)
        {
            this.color = color;
        }
    }

    class SkillBrief : ItemBrief
    {

        public SkillBrief(string type, string name, string description, int price, int state) : base(type, name, description, price, state)
        {
        }
    }
    public class ShopStage : Stage
    {
        Button backBtn;
        Button weaponBtn;
        Button skinBtn;
        Button skillBtn;
        Page weaponPage;
        Page skinPage;
        Page skillPage;
        string _nextShowing;
        string _nextState;
        string _prevState;
        string _showing = "weapon";
        float bodyTop = 205;



        public ShopStage(Manager manager, string prevState) : base(manager)
        {

            WeaponBrief[] weaponList = new WeaponBrief[8]
            {
                new WeaponBrief("weapon","Default Gun","A pistol",0,1,"default"),
                new WeaponBrief("weapon","Gun 1","A pistol",10,0,"gun1"),
                new WeaponBrief("weapon","Gun 2","A pistol",120,0,"gun2"),
                new WeaponBrief("weapon","Gun 3","A pistol",30,0,"gun3"),
                new WeaponBrief("weapon","Gun 4","A pistol",40,0,"gun4"),
                new WeaponBrief("weapon","Gun 5","A pistol",60,0,"gun5"),
                new WeaponBrief("weapon","Gun 6","A pistol",150,0,"gun6"),
                new WeaponBrief("weapon","RPG","A RPG",150,0,"rpg"),
            };

            SkinBrief[] skinList = new SkinBrief[6]
            {
                new SkinBrief("skin","Default Skin","Blue Alien",0,1,Color.Blue),
                new SkinBrief("skin","Green","Blue Alien",10,0,Color.Green),
                new SkinBrief("skin","Red","Blue Alien",20,0,Color.Red),
                new SkinBrief("skin","Yellow","Blue Alien",30,0,Color.Yellow),
                new SkinBrief("skin","Gray","Blue Alien",40,0,Color.Gray),
                new SkinBrief("skin","Pink","Blue Alien",50,0,Color.Pink),
            };

            SkillBrief[] skillList = new SkillBrief[2]
            {
                new SkillBrief("skill","Health","Increase health",100,0),
                new SkillBrief("skill","Defense","Reduce damage",100,0),
            };

            backBtn = new Button("<", Color.Red, 80, 80, 70, 70);
            float width = this.Manager.Window.Width / 3;
            weaponBtn = new Button("Weapon", Color.Gray, width/2, 180, width, 50);
            weaponBtn.Color = Color.White;
            skinBtn = new Button("Skin", Color.Gray, width + width/2, 180, width, 50);
            skinBtn.Color = Color.White;
            skillBtn = new Button("Power", Color.Gray, width*2 + width/2, 180, width, 50);
            skillBtn.Color = Color.White;
            weaponPage = new Page(this, weaponList, this.Manager.Window.Height - 205, 205);
            skinPage = new Page(this, skinList, this.Manager.Window.Height - 205, 205);
            skillPage = new Page(this, skillList, this.Manager.Window.Height - 205, 205);
            _prevState = prevState;

            this.Load();
        }

        public void Load()
        {
            for(int i = 0; i < this.Manager.UserData.Count; i++)
            {
                if(i < 8)
                {
                    switch (this.Manager.UserData[i])
                    {
                        case "0":
                            weaponPage.Items[i].State = "not_purchased";
                            break;
                        case "1":
                            weaponPage.Items[i].State = "purchased";
                            break;
                        case "2":
                            weaponPage.Items[i].State = "selected";
                            this.Manager.Gun = weaponPage.Items[i]._name;
                            break;
                    }
                } else if (i > 7 && i < 14)
                {
                    switch (this.Manager.UserData[i])
                    {
                        case "0":
                            skinPage.Items[i - 8].State = "not_purchased";
                            break;
                        case "1":
                            skinPage.Items[i - 8].State = "purchased";
                            break;
                        case "2":
                            skinPage.Items[i - 8].State = "selected";
                            this.Manager.Skin = skinPage.Items[i - 7]._name;
                            break;
                    }
                } else if ( i > 13 && i < 16)
                {
                    switch (this.Manager.UserData[i])
                    {
                        case "0":
                            skillPage.Items[i - 14].State = "not_purchased";
                            break;
                        case "1":
                            skillPage.Items[i - 14].State = "purchased";
                            break;
                        case "2":
                            skillPage.Items[i - 14].State = "selected";
                            this.Manager.Skill = skillPage.Items[i - 13]._name;
                            break;
                    }
                }
            }
            
        }

        public void Save()
        {
            List<string> saving = new List<string>();
            for(int i = 0; i < weaponPage.Items.Count; i++)
            {
                switch (weaponPage.Items[i].State)
                {
                    case "not_purchased":
                        saving.Add("0");
                        break;
                    case "purchased":
                        saving.Add("1");
                        break;
                    case "selected":
                        saving.Add("2");
                        break;
                }
            }

            for (int i = 0; i < skinPage.Items.Count; i++)
            {
                switch (skinPage.Items[i].State)
                {
                    case "not_purchased":
                        saving.Add("0");
                        break;
                    case "purchased":
                        saving.Add("1");
                        break;
                    case "selected":
                        saving.Add("2");
                        break;
                }
            }

            for (int i = 0; i < skillPage.Items.Count; i++)
            {
                switch (skillPage.Items[i].State)
                {
                    case "not_purchased":
                        saving.Add("0");
                        break;
                    case "purchased":
                        saving.Add("1");
                        break;
                    case "selected":
                        saving.Add("2");
                        break;
                }
            }

            this.Manager.UserData = saving;
            this.Manager.Save("../../../userdata.txt");
        }

        public override void Update()
        {
            backBtn.Update();
            weaponBtn.Update();
            skinBtn.Update();
            skillBtn.Update();

            weaponBtn.Active = false;
            skinBtn.Active = false;
            skillBtn.Active = false;
            switch(_showing)
            {
                case "weapon":
                    weaponBtn.Active = true;
                    weaponPage.Update();
                    if (weaponPage.EffectComplete)
                    {
                        _showing = _nextShowing;
                    }
                    break;
                case "skin":
                    skinBtn.Active = true;
                    skinPage.Update();
                    if (skinPage.EffectComplete)
                    {
                        _showing = _nextShowing;
                    }
                    break;
                case "skill":
                    skillBtn.Active = true;
                    skillPage.Update();
                    if (skillPage.EffectComplete)
                    {
                        _showing = _nextShowing;
                    }
                    break;
            }
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (backBtn.Hovering)
                {
                    this.Closing = true;
                    _nextState = _prevState;
                } else
                if (weaponBtn.Hovering)
                {
                    switch (_showing)
                    {
                        case "weapon":
                            weaponPage.Closing = true;
                            break;
                        case "skin":
                            skinPage.Closing = true;
                            break;
                        case "skill":
                            skillPage.Closing = true;
                            break;
                    }
                    weaponPage.ResetEffect();
                    _nextShowing = "weapon";
                } else
                if (skinBtn.Hovering)
                {
                    switch (_showing)
                    {
                        case "weapon":
                            weaponPage.Closing = true;
                            break;
                        case "skin":
                            skinPage.Closing = true;
                            break;
                        case "skill":
                            skillPage.Closing = true;
                            break;
                    }
                    skinPage.ResetEffect();
                    _nextShowing = "skin";
                } else
                if (skillBtn.Hovering)
                {
                    switch (_showing)
                    {
                        case "weapon":
                            weaponPage.Closing = true;
                            break;
                        case "skin":
                            skinPage.Closing = true;
                            break;
                        case "skill":
                            skillPage.Closing = true;
                            break;
                    }
                    skillPage.ResetEffect();
                    _nextShowing = "skill";
                }
            }
            if (this.OutEf._completed)
            {
                if (_nextState == "home")
                {
                    this.Manager.NewHome();
                }
                if (_nextState == "end")
                {
                    this.Manager.NewEnd();
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

            switch (_showing)
            {
                case "weapon":
                    weaponPage.Draw();
                    break;
                case "skin":
                    skinPage.Draw();
                    break;
                case "skill":
                    skillPage.Draw();
                    break;
            }
            SplashKit.FillRectangle(Color.Black,0,0, this.Manager.Window.Width,205);
            SplashKit.DrawText("SHOP", Color.White, "font", 70, this.Manager.Window.Width / 2 - SplashKit.TextWidth("SHOP", "font", 70) / 2, 80 - SplashKit.TextHeight("SHOP", "font", 70) / 2);
            SplashKit.DrawText(this.Manager.Coin.ToString(), Color.Yellow, "font", 50, this.Manager.Window.Width - SplashKit.TextWidth(this.Manager.Coin.ToString(), "font", 50) - 35, 80 - SplashKit.TextHeight("SHOP", "font", 70) / 2);
            backBtn.Draw();
            weaponBtn.Draw();
            skinBtn.Draw();
            skillBtn.Draw();

            this.InEf.Draw();
            this.OutEf.Draw();
        }

    }
}