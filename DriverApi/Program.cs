using Application;

using Infrastructure;

using Microsoft.IdentityModel.Logging;

using Serilog;

namespace DriverApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Console.WriteLine("*****************************************************************************");
            Console.WriteLine($"ApplicationName: {builder.Environment.ApplicationName}");
            Console.WriteLine($"EnvironmentName: {builder.Environment.EnvironmentName}");
            Console.WriteLine($"ContentRootPath: {builder.Environment.ContentRootPath}");
            Console.WriteLine($"WebRootPath: {builder.Environment.WebRootPath}");
            Console.WriteLine("*****************************************************************************");

            // Add services to the container.

            builder.Services
                .AddPresentation()
                .AddApplication()
                .AddInfrastructure(builder.Configuration, builder.Environment);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (builder.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            // Adds the authorization middleware to make sure, our API endpoint cannot be accessed
            // by anonymous clients.
            app.UseAuthorization();

            app.MapControllers();

            // R U N
            try
            {
                Log.Information("Starting host...");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
