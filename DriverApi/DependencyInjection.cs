using Microsoft.AspNetCore.Mvc.Infrastructure;

using DriverApi.Common;
using DriverApi.Common.Errors;

namespace DriverApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<ProblemDetailsFactory, DriverProblemDetailsFactory>();
            services.AddMappings();
            return services;
        }
    }
}
