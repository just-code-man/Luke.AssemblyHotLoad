# Luke.AssemblyHotLoad

A .Net Assembly Hotload Library

you can use this to Hotload your Library,and it will not occupy files



一个.Net平台下热加载库

你可以使用它热加载库，而且不会占用文件，可以用作插件

在dll文件新增或更新时会自动加载到内存



## Using



```c#
public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

    
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyLoadServices(configuration.GetSection("Test"));
        }

    
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.StartAssemblyLoad();
        }
    }


```



appsettings.json

```json
{
  "Test": {
    "Path": "Plugins",
    "Filter": "Plugin.*.dll",
    "AutoDelete": false
  }
}
```

**Path**: 加载路径 可以是绝对路径或相对路径

**Filter**:文件匹配过滤 Plugin.*.dll  匹配  Plugin.XXXX.dll

**AutoDelete**:在文件被删除时是否同时卸载程序集



Add a new  Library  **Plugin.Abstractions**

添加一个新类库  **Plugin.Abstractions**

Add a new interface **IPluginCommand**

添加一个新接口类  **IPluginCommand**

```c#
public interface IPluginCommand
    {

        void Hello(string message);

    }
```



Add a new  Library  **Plugin.Test1**

添加一个新类库  **Plugin.Test1**

Add a new class   **PluginCommand**

添加一个新类  **PluginCommand**

```c#
public class PluginCommand : IPluginCommand
    {
        public void Hello(string message)
        {
            Console.WriteLine("Test:" +  message);
        }
	}
```



use

copy **Plugin.Test1.dll**  to **Path**

将 **Plugin.Test1.dll**  复制到  **Path**  路径

```c#

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
```







