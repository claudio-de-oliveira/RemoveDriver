using Application.Interfaces.Driver;

using Infrastructure.Services.Driver;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager configuration,
            IWebHostEnvironment environment
            )
        {
            InstallSerilog(configuration);

            // Ver mapeamento de volume co docker-compose.yml
            string root = configuration["Root"] ?? Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Driver");

            var rootPath = Path.Combine(environment.ContentRootPath, root);

            IFileProvider physicalProvider = new PhysicalFileProvider(rootPath);
            services.AddSingleton(physicalProvider);

            services.AddScoped<IDriver, Driver>();

            return services;
        }

        private static void InstallSerilog(ConfigurationManager _)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: SystemConsoleTheme.Colored
                    )
                .CreateLogger();
        }
    }
}
