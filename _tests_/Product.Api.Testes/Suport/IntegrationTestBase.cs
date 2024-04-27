using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Product.Api.Infrastructure.Data;
using Product.Api.Infrastructure.Data.ReadOnly;

namespace Product.Api.Testes.Suport
{
    public class IntegrationTestBase : WebApplicationFactory<Program>
    {

        protected readonly DbContainerTest dbContainerTest;
        protected readonly HttpClient client;

        public IntegrationTestBase()
        {
            dbContainerTest = new DbContainerTest();
            dbContainerTest.InitializeAsync().Wait();
            client = CreateClient();

        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Teste");
            var assembly = typeof(ProductDbContext).Assembly;

            builder.ConfigureAppConfiguration((ctx, config) =>
            {
                config.Sources.Clear();
                config.AddJsonFile("appsettings.Test.json")
                       .AddEnvironmentVariables()
                       .Build();
            });

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IProductMongoContext>();
                services.RemoveAll<ProductDbContext>();

                services.AddDbContext<ProductDbContext>(config => config
                        .UseNpgsql(dbContainerTest._postgreSqlContainer.GetConnectionString(), opts => opts
                        .MigrationsAssembly(assembly.GetName().Name)
                        .EnableRetryOnFailure()));

                services.AddScoped<IProductMongoContext>(x => new ProductMongoContext(dbContainerTest._mongoDbContainer.GetConnectionString(), "Produto", "Produtos"));

                var _serviceProvider = services.BuildServiceProvider();

                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<ProductDbContext>();
                    context.Database.SetConnectionString(dbContainerTest._postgreSqlContainer.GetConnectionString());
                    if (context.Database.IsRelational())
                        context.Database.Migrate();
                }

            });

            base.ConfigureWebHost(builder);
        }
    }
}
