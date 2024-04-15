using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Api.Domain.Notifications;
using Product.Api.Infrastructure.Behavior;
using Product.Api.Infrastructure.Data;
using Product.Api.Infrastructure.Data.ReadOnly;

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

            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationRequestBehavior<,>));
            builder.Services.AddScoped<IDomainNotificationContext, DomainNotificationContext>();

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => builder.Services.AddScoped(result.InterfaceType, result.ValidatorType));

            builder.Services.AddCap(x =>
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
