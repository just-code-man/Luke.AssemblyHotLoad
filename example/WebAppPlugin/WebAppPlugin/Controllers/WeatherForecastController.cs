using Luke.AssemblyHotLoad;
using Microsoft.AspNetCore.Mvc;
using Plugin.Abstractions;
using System.Reflection;

namespace WebAppPlugin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IAssemblyLoadManager _assemblyLoadManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAssemblyLoadManager assemblyLoadManager)
        {
            _logger = logger;
            _assemblyLoadManager = assemblyLoadManager;
        }

        [HttpGet]
        public void Get()
        {
            string assemblyName = "Plugin.Test1";
            string fullClassName = assemblyName + ".PluginCommand";
            Assembly assembly = _assemblyLoadManager.GetAssembly(assemblyName);
            IPluginCommand command = assembly.CreateInstance(fullClassName) as IPluginCommand;
            command.Hello("use PluginCommand Hello, but not Reference Plugin.Test1");
        }
    }
}