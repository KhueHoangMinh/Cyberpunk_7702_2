﻿using SplashKitSDK;
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

    public class Item
    {
        Page _page;
        string _name;
        int _price;
        Bitmap _image;
        float _width;
        float _height;
        float _xOnPage;
        float _yOnPage;
        float x;
        float y;
        Button buyButton;
        Button selectButton;
        Button deselectButton;
        string _state = "not_purchased";
        DrawingOptions opts;

        public Item(Page page, string name, int price, Bitmap image, float width, float height, float xOnPage, float yOnPage)
        {
            _page = page;
            _name = name;
            _price = price;
            _image = image;
            float Scale = 300 / _image.Width;
            opts = new DrawingOptions()
            {
                Dest = _page.Shop.Manager.Window,
                ScaleX = Scale,
                ScaleY = Scale,
            };    
            _width = width;
            _height = height;
            _xOnPage = xOnPage;
            _yOnPage = yOnPage;
            x = _xOnPage;
            y = _yOnPage;   
            buyButton = new Button(_price.ToString(), Color.Red, x, y + 380, 200, 80);
            selectButton = new Button("Select", Color.Green, x, y + 380, 200, 80);
            deselectButton = new Button("Deselect", Color.Yellow, x, y + 380, 200, 80);
        }

        public void Update()
        {
            y = _page.Y + _page.ScrollY + _yOnPage;
            buyButton.Y = y + 180;
            selectButton.Y = y + 180;
            deselectButton.Y = y + 180;
            switch (_state)
            {
                case "not_purchased":
                    buyButton.Update();
                    if(buyButton.Hovering && SplashKit.MouseClicked(MouseButton.LeftButton))
                    {

                    }
                    break;
                case "purchased":
                    selectButton.Update();
                    if (selectButton.Hovering && SplashKit.MouseClicked(MouseButton.LeftButton))
                    {

                    }
                    break;
                case "selected":
                    deselectButton.Update();
                    if (deselectButton.Hovering && SplashKit.MouseClicked(MouseButton.LeftButton))
                    {

                    }
                    break;
            }
        }

        public void Draw()
        {
            SplashKit.FillRectangle(Color.RGBColor(20,20,20), x - _width / 2, y - _height / 2, _width, _height);
            SplashKit.FillRectangle(Color.Black, x - _width / 2 + 20, y - _height / 2 + 20, _width - 40, _height/2 - 20);
            SplashKit.DrawBitmap(_image, x - _image.Width / 2, y - _width/4 - _image.Height/2,opts);
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
    }

    public class Page
    {
        ShopStage _shop;
        List<Item> _items;
        float _height;
        float _actualHeight;
        float _scrollY = 0;
        float _y;

        public Page (ShopStage shop, string[] items, float height, float y)
        {
            _shop = shop; 
            _height = height;
            _items = new List<Item>();
            _y = y;
            float width = _shop.Manager.Window.Width / 3;
            for (int i = 0; i < items.Length; i++)
            {
                int column = i % 3 + 1;
                int row = (int)Math.Ceiling((decimal)((i+1) * 1.0 / 3));
                _items.Add(new Item(this, items[i], 100, SplashKit.BitmapNamed(items[i]), width - 40, width - 40, (width) * (column - 1) + width / 2, (width) * (row - 1) + width / 2));
            }
            int rowCount = (int)Math.Ceiling((decimal)(_items.Count * 1.0 / 3));
            _actualHeight = rowCount * width;
        }

        public void Update()
        {
            _scrollY += (float)SplashKit.MouseWheelScroll().Y*5;
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Update();
            }
        }

        public void Draw()
        {
            for(int i = 0; i < _items.Count; i++)
            {
                _items[i].Draw();
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

        public ShopStage Shop { get { return _shop; } }
    }
    public class ShopStage
    {
        Manager _manager;
        Button backBtn;
        Button weaponBtn;
        Button skinBtn;
        Button skillBtn;
        Page weaponPage;
        Page skinPage;
        Page skillPage;
        InEffect inEf;
        OutEffect outEf;
        bool _closing = false;
        string _nextState;
        string _prevState;
        string _showing = "weapon";
        float bodyTop = 205;
        public ShopStage(Manager manager, string prevState)
        {
            _manager = manager;
            inEf = new InEffect(_manager.Window.Width, _manager.Window.Height);
            outEf = new OutEffect(_manager.Window.Width, _manager.Window.Height);
            backBtn = new Button("<", Color.Red, 80, 80, 70, 70);
            float width = _manager.Window.Width / 3;
            weaponBtn = new Button("Weapon", Color.Green, width/2, 180, width, 50);
            skinBtn = new Button("Skin", Color.Green, width + width/2, 180, width, 50);
            skillBtn = new Button("Skill", Color.Green, width*2 + width/2, 180, width, 50);
            weaponPage = new Page(this, new string[6]
            {
                "gun1",
                "gun2",
                "gun3",
                "gun4",
                "gun5",
                "gun6",
            }, _manager.Window.Height - 205, 205);
            _prevState = prevState;
        }

        public void Update()
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
                    break;
                case "skin":
                    skinBtn.Active = true;
                    break;
                case "skill":
                    skillBtn.Active = true;
                    break;
            }
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (backBtn.Hovering)
                {
                    _closing = true;
                    _nextState = _prevState;
                } else
                if (weaponBtn.Hovering)
                {
                    _showing = "weapon";
                } else
                if (skinBtn.Hovering)
                {
                    _showing = "skin";
                } else
                if (skillBtn.Hovering)
                {
                    _showing = "skill";
                }
            }
            weaponPage.Update();
            if (outEf._completed)
            {
                if (_nextState == "home")
                {
                    _manager.NewHome();
                }
                if (_nextState == "end")
                {
                    _manager.NewEnd();
                }
            }
        }

        public void Draw()
        {

            weaponPage.Draw();
            SplashKit.FillRectangle(Color.Black,0,0, _manager.Window.Width,205);
            SplashKit.DrawText("SHOP", Color.White, "font", 70, _manager.Window.Width / 2 - SplashKit.TextWidth("SHOP", "font", 70) / 2, 40);
            backBtn.Draw();
            weaponBtn.Draw();
            skinBtn.Draw();
            skillBtn.Draw();

            inEf.Draw();
            if (_closing)
            {
                outEf.Draw();
            }
        }

        public Manager Manager { get { return _manager; } }

        public Page WeaponPage
        {
            get { return weaponPage; }  
        }
    }
}