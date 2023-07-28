using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Cyberpunk77022
{
    

    public interface ShopItem
    {
        public void Graphic(float x, float y);
        public string Name { get; }
        public string ID { get; }
        public string Description { get; }
        public int Price { get; }
        public bool Purchased
        {
            get;
            set;
        }
    }
    public class Item
    {
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
        ShopItem _actualItem;
        int _index = 0;

        public Item(Page page, int index, ShopItem actualItem, float width, float height, float xOnPage, float yOnPage)
        {
            _page = page;
            _index = index;
            _actualItem = actualItem;
            _name = actualItem.Name;
            _price = actualItem.Price;
            _width = width;
            _height = height;
            _xOnPage = xOnPage;
            _yOnPage = yOnPage;
            x = _xOnPage;
            y = _yOnPage;   
            buyButton = new Button(_price.ToString(), Color.Red, x, y + 380, 200, 80);
            selectButton = new Button("Select", Color.Green, x, y + 380, 200, 80);
            deselectButton = new Button("Selected", Color.Yellow, x, y + 380, 200, 80);

            if (_actualItem.Purchased)
            {
                if (_page.Shop.Manager.Gun == _actualItem || _page.Shop.Manager.Skin == _actualItem || _page.Shop.Manager.Skill == _actualItem)
                {
                    _state = "selected";
                }
                else
                {
                    _state = "purchased";
                }
            }
            else
            {
                _state = "not_purchased";
            }
        }

        public void Update()
        {

            y = _page.Y + _page.ScrollY + _yOnPage;
            buyButton.Y = y + 180;
            selectButton.Y = y + 180;
            deselectButton.Y = y + 180;

            if(_actualItem.Purchased)
            {
                selectButton.Update();
                if (_page.Shop.Manager.SelectedGun.Contains(_actualItem) || _page.Shop.Manager.Skin == _actualItem || _page.Shop.Manager.Skill == _actualItem)
                {
                    _state = "selected";
                } else
                {
                    if (selectButton.Hovering && SplashKit.MouseClicked(MouseButton.LeftButton) && SplashKit.MousePosition().Y > _page.Y)
                    {
                        _page.Deselect(this);
                        switch (_actualItem)
                        {
                            case Gun:
                                _page.Shop.Manager.Gun = _page.Shop.Manager.Guns[_index] as Gun;
                                break;
                            case Skin:
                                _page.Shop.Manager.Skin = _page.Shop.Manager.Skins[_index] as Skin;
                                break;
                            case Skill:
                                _page.Shop.Manager.Skill = _page.Shop.Manager.Skills[_index] as Skill;
                                break;
                        }
                        _state = "selected";
                        _page.Shop.Manager.Save("userdata.txt");
                    }
                }
            } else
            {
                buyButton.Update();
                if (buyButton.Hovering && SplashKit.MouseClicked(MouseButton.LeftButton) && SplashKit.MousePosition().Y > _page.Y)
                {
                    if (_page.Shop.Manager.Coin > _price)
                    {
                        _page.Shop.Manager.Coin -= _price;
                        _state = "purchased";
                        _actualItem.Purchased = true;
                        _page.Shop.Manager.Save("userdata.txt");
                    }
                }
            }
        }

        public void Draw()
        {
            SplashKit.FillRectangle(Color.RGBColor(20,20,20), x - _width / 2, y - _height / 2, _width, _height);
            SplashKit.FillRectangle(Color.Black, x - _width / 2 + 20, y - _height / 2 + 20, _width - 40, _height/2 - 20);

            _actualItem.Graphic(x, y - _height / 4);

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

        public Page (ShopStage shop, List<ShopItem> items, float height, float y)
        {
            _shop = shop; 
            _height = height;
            _items = new List<Item>();
            inEf = new InEffect(0, _y, _shop.Manager.Window.Width, _shop.Manager.Window.Height);
            outEf = new OutEffect(0, _y, _shop.Manager.Window.Width, _shop.Manager.Window.Height);
            _y = y;
            float width = _shop.Manager.Window.Width / 3;
            for (int i = 0; i < items.Count; i++)
            {
                int column = i % 3 + 1;
                int row = (int)Math.Ceiling((decimal)((i+1) * 1.0 / 3));
                _items.Add(new Item(this, i, items[i], width - 40, width - 40, (width) * (column - 1) + width / 2, (width) * (row - 1) + width / 2));
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
            backBtn = new Button("<", Color.Red, 80, 80, 70, 70);
            float width = this.Manager.Window.Width / 3;
            weaponBtn = new Button("Weapon", Color.Gray, width/2, 180, width, 50);
            weaponBtn.Color = Color.White;
            skinBtn = new Button("Skin", Color.Gray, width + width/2, 180, width, 50);
            skinBtn.Color = Color.White;
            skillBtn = new Button("Power", Color.Gray, width*2 + width/2, 180, width, 50);
            skillBtn.Color = Color.White;
            weaponPage = new Page(this, this.Manager.Guns, this.Manager.Window.Height - 205, 205);
            skinPage = new Page(this, this.Manager.Skins, this.Manager.Window.Height - 205, 205);
            skillPage = new Page(this, this.Manager.Skills, this.Manager.Window.Height - 205, 205);
            _prevState = prevState;

            //this.Load();
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