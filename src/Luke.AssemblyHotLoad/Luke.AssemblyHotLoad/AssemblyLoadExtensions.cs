using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Luke.AssemblyHotLoad
{
    public static class AssemblyLoadExtensions
    {
        public static IServiceCollection AddAssemblyLoadServices(this IServiceCollection services, IConfigurationSection configuration)
        {
            AssemblyLoadConfig config = configuration.Get<AssemblyLoadConfig>();
            services.AddSingleton(config);
            services.AddSingleton<IAssemblyLoadManager, AssemblyLoadManager>();
            return services;
        }

        public static IServiceCollection AddAssemblyLoadServices(this IServiceCollection services, Action<AssemblyLoadConfig> action)
        {
            AssemblyLoadConfig config = new AssemblyLoadConfig()
            {
                Path = "",
                Filter = "*",
                AutoDelete = false
            };
            action(config);
            services.AddSingleton(config);
            services.AddSingleton<IAssemblyLoadManager, AssemblyLoadManager>();
            return services;
        }

        public static IApplicationBuilder StartAssemblyLoad(this IApplicationBuilder app)
        {
            IAssemblyLoadManager am = app.ApplicationServices.GetService<IAssemblyLoadManager>();
            am.StartWatcher();
            return app;
        }

    }
}
