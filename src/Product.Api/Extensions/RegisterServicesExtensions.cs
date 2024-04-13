using Microsoft.EntityFrameworkCore;
using Product.Api.Infrastructure;
using Product.Api.Infrastructure.ReadOnly;

namespace Product.Api.Extensions
{
    public static class RegisterServicesExtensions
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            var assembly = typeof(ProductDbContext).Assembly;

            builder.Services.AddDbContext<ProductDbContext>(config => config
            .UseNpgsql(builder.Configuration.GetConnectionString("ProductSqlConnection"), opts => opts
            .MigrationsAssembly(assembly.GetName().Name)
            .EnableRetryOnFailure()));

            builder.Services.AddScoped<IProductMongoContext, ProductMongoContext>();
            builder.Services.AddMediatR(_ => _.RegisterServicesFromAssembly(assembly));

            _ = builder.Services.AddCap(x =>
            {
                var configuration = builder.Configuration;
                x.UsePostgreSql(configuration.GetConnectionString("ProductSqlConnection") ?? "");
                x.UseRabbitMQ(o =>
                {
                    o.HostName = configuration["RabbitMQ:Host"] ?? "";
                    o.Password = configuration["RabbitMQ:UserName"] ?? "";
                    o.UserName = configuration["RabbitMQ:Password"] ?? "";
                    o.Port = 5672;
                });
                x.UseDashboard();
            });

            return builder;
        }
    }
}
