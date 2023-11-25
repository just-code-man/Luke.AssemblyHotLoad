using Plugin.Abstractions.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Test1.Framework
{
    public class PluginCommand : IPluginCommand
    {
        public void Hello(string message)
        {
            Console.WriteLine("测试输出22" + message);
        }
    }
}
