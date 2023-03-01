using Infrastructure.Data;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace AdministrativeApi.Test
{
    public class AdministrativeApiApplication : WebApplicationFactory<Program>
    {
        public AdministrativeApiApplication(Uri baseAddress)
        {
            ClientOptions.BaseAddress = baseAddress;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();
            
            builder.ConfigureServices(services => {
                services.RemoveAll(typeof(DbContextOptions<PraeceptorDbContext>));
                services.AddDbContext<PraeceptorDbContext>(options =>
                    options.UseInMemoryDatabase("PraeceptorDatabase", root)
                );
            });

            return base.CreateHost(builder);
        }
    }
}
