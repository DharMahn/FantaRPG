﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG
{
    internal enum Movement
    {
        None = 0, Left = 1, Right = 2, Down = 4, Jump = 8, Up = 16, Interact = 32,
    }
}
