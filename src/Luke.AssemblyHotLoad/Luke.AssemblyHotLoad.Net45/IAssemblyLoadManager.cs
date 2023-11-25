using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Luke.AssemblyHotLoad.Net45
{
    public interface IAssemblyLoadManager : IDisposable
    {
        Assembly GetAssembly(string assemblyName);

        void StartWatcher();

        void Add(string assemblyName);

        void Remove(string assemblyName);
    }
}
