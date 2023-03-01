using Domain.Entity;

using Infrastructure.Data;

using Microsoft.Extensions.DependencyInjection;

namespace AdministrativeApi.Test.Mock
{
    public class HoldingMockData
    {
        public static async Task<Guid> CreateHolding(AdministrativeApiApplication application)
        {
            using var scope = application.Services.CreateScope();
            var provider = scope.ServiceProvider;

            using var praeceptorDbContext = provider.GetRequiredService<PraeceptorDbContext>();
            await praeceptorDbContext.Database.EnsureCreatedAsync();

            var holding = Holding.Create(
                    "Main",
                    "Minha Holding Principal",
                    "Curitiba/SE",
                    DateTime.UtcNow,
                    "systemadmin");

            return holding.Id;
        }
    }
}
