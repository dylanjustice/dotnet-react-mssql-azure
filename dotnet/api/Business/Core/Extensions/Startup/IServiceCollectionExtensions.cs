using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AndcultureCode.GB.Business.Core.Providers.Logging;
using Serilog;
using Microsoft.Extensions.Hosting;

namespace AndcultureCode.GB.Business.Core.Extensions.Startup
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilogServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            Log.Logger = new LoggingProvider(environment, services, configuration).GetLogger();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            return services.AddSingleton(Log.Logger);
        }
    }
}
