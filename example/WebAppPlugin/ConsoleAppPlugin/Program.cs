using Luke.AssemblyHotLoad.Net45;
using Plugin.Abstractions.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPlugin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IAssemblyLoadManager loadManager = new AssemblyLoadManager(new AssemblyLoadConfig {
                Path = "Plugins", Filter = "Plugin.*.dll", AutoDelete = false
            });
            loadManager.StartWatcher();

            string assemblyName = "Plugin.Test1.Framework";
            string fullClassName = assemblyName + ".PluginCommand";
            
            while (true)
            {
                Assembly assembly = loadManager.GetAssembly(assemblyName);
                IPluginCommand command = assembly.CreateInstance(fullClassName) as IPluginCommand;
                command.Hello("use PluginCommand Hello, but not Reference Plugin.Test1.Framework");
            }

            Console.ReadLine();

        }
    }
}
