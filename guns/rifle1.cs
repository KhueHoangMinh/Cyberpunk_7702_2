﻿using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk77022
{
    public class Rifle1 : Gun
    {
        public Rifle1(Window window, float damage) : base("Rifle 1", "rifle1", "a gun", 50, window, "gun3", "rifle", 250, 50, damage, 1500000, 0.1f)
        {
        }
        public Rifle1(Window window, float damage, bool enemys) : base("Rifle 1", "rifle1", "a gun", 50, window, "gun3", "rifle", 250, 50, damage, 1500000, 0.1f, enemys)
        {
        }
    }
}
