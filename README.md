# Luke.AssemblyHotLoad

A .Net Assembly Hotload Library

you can use this to Hotload your Library,and it will not occupy files



һ��.Netƽ̨���ȼ��ؿ�

�����ʹ�����ȼ��ؿ⣬���Ҳ���ռ���ļ��������������

��dll�ļ����������ʱ���Զ����ص��ڴ�



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

**Path**: ����·�� �����Ǿ���·�������·��

**Filter**:�ļ�ƥ����� Plugin.*.dll  ƥ��  Plugin.XXXX.dll

**AutoDelete**:���ļ���ɾ��ʱ�Ƿ�ͬʱж�س���



Add a new  Library  **Plugin.Abstractions**

���һ�������  **Plugin.Abstractions**

Add a new interface **IPluginCommand**

���һ���½ӿ���  **IPluginCommand**

```c#
public interface IPluginCommand
    {

        void Hello(string message);

    }
```



Add a new  Library  **Plugin.Test1**

���һ�������  **Plugin.Test1**

Add a new class   **PluginCommand**

���һ������  **PluginCommand**

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

�� **Plugin.Test1.dll**  ���Ƶ�  **Path**  ·��

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







