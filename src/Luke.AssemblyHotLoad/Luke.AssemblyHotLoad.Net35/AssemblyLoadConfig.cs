﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Luke.AssemblyHotLoad.Net45
{
    public class AssemblyLoadConfig
    {
        public string Path { get; set; }

        public string Filter { get; set; }

        public bool AutoDelete { get; set; }
    }
}
