using Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Test1
{
    public class PluginCommand : IPluginCommand
    {
        public void Hello(string message)
        {
            Console.WriteLine("测试输出22" +  message);
        }
    }
}
