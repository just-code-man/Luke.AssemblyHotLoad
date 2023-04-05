using Luke.Plugin.Core;
using Luke.AssemblyHotLoad;
using System.Reflection;
using Plugin.Abstractions;

namespace WebAppPlugin
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            IConfiguration configuration = builder.Configuration;
            builder.Services.AddAssemblyLoadServices(configuration.GetSection("Test"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            

            app.UseAuthorization();

            app.StartAssemblyLoad();

            app.MapControllers();

            app.Run();
        }
    }
}