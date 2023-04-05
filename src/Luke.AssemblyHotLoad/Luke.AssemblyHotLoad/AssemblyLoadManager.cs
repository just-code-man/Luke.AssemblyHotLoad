using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Luke.AssemblyHotLoad
{
    internal class AssemblyLoadManager : IAssemblyLoadManager
    {

        private ConcurrentDictionary<string, Assembly> _assemblyList;

        private AssemblyLoadConfig _config;

        FileSystemWatcher _fileSystemWatcher;

        string _path;

        public AssemblyLoadManager(AssemblyLoadConfig config)
        {
            _config = config;
            _assemblyList = new ConcurrentDictionary<string, Assembly>();
            _path = GetPath();
        }

        public Assembly GetAssembly(string assemblyName)
        {
            Assembly assembly = _assemblyList.GetValueOrDefault(assemblyName);
            return assembly;
        }

        private string GetPath()
        {
            bool isPathFullyQualified = Path.IsPathFullyQualified(_config.Path);
            string path = isPathFullyQualified ? _config.Path : (AppDomain.CurrentDomain.BaseDirectory + _config.Path);
            return path;
        }

        public void StartWatcher()
        {
            LoadAssemblyFromConfig();

            _fileSystemWatcher = new FileSystemWatcher(_path);
            _fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Created += OnCreated;
            _fileSystemWatcher.Deleted += OnDeleted;
            _fileSystemWatcher.Renamed += OnRenamed;
            _fileSystemWatcher.Error += OnError;

            _fileSystemWatcher.Filter = _config.Filter;
            _fileSystemWatcher.IncludeSubdirectories = false;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void LoadAssemblyFromConfig()
        {
            DirectoryInfo directory = new DirectoryInfo(_path);

            FileInfo[] fileInfos = directory.GetFiles(_config.Filter);
            foreach (FileInfo fileInfo in fileInfos)
            {
                ReLoadAssembly(fileInfo.FullName);
            }

        }

        private void ReLoadAssembly(string path)
        {
            try
            {
                byte[] bs = File.ReadAllBytes(path);
                Assembly assembly = Assembly.Load(bs);
                if (assembly != null)
                {
                    _assemblyList.AddOrUpdate(assembly.GetName().Name, assembly, (s, a) => assembly);
                }
            }
            catch (Exception)
            {

            }
        }

        public void Add(string assemblyName)
        {
            string filePath = Path.Combine(_path, assemblyName);
            if (File.Exists(filePath))
            {
                ReLoadAssembly(filePath);
            }
        }

        public void Remove(string assemblyName)
        {
            _assemblyList.Remove(assemblyName, out _);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            ReLoadAssembly(e.FullPath);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            ReLoadAssembly(e.FullPath);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (_config.AutoDelete)
            {
                string assemblyName = e.Name;
                string fileExt = ".dll";
                if (assemblyName.EndsWith(fileExt))
                {
                    assemblyName = assemblyName.Substring(0, assemblyName.Length - fileExt.Length);
                    Remove(assemblyName);
                }
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            ReLoadAssembly(e.FullPath);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {

        }

        public void Dispose()
        {
            _fileSystemWatcher.Dispose();
            _assemblyList.Clear();
        }
    }
}
