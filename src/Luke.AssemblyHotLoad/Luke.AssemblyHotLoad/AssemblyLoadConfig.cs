using System;
using System.Collections.Generic;
using System.Text;

namespace Luke.AssemblyHotLoad
{
    public class AssemblyLoadConfig
    {
        public string Path { get; set; }

        public string Filter { get; set; }

        public bool AutoDelete { get; set; }

    }
}
