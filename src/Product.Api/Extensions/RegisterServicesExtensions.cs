using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Api.Api.Handlers;
using Product.Api.Domain.Notifications;
using Product.Api.Infrastructure.Behavior;
using Product.Api.Infrastructure.Data;
using Product.Api.Infrastructure.Data.ReadOnly;
using Serilog;
using System.Reflection;

namespace Product.Api.Extensions;

public static class RegisterServicesExtensions
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        var assembly = typeof(ProductDbContext).Assembly;
        builder.RegisterCap();
        builder.RegisterDbContext(assembly);
        builder.RegisterApiHandlers();
        builder.RegisterMondoContext(assembly);
        builder.ConfigureSerilogLogger();
        builder.RegisterPipelineBehavior(assembly);
        return builder;
    }

    private static void RegisterCap(this WebApplicationBuilder builder)
    {
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
    }
    private static void RegisterDbContext(this WebApplicationBuilder builder, Assembly assembly)
    {
        builder.Services.AddDbContext<ProductDbContext>(config => config
        .UseNpgsql(builder.Configuration.GetConnectionString("ProductSqlConnection"), opts => opts
        .MigrationsAssembly(assembly.GetName().Name)
        .EnableRetryOnFailure()));
    }
    private static void RegisterApiHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ProductApiHandler>();
    }
    private static void RegisterMondoContext(this WebApplicationBuilder builder, Assembly assembly)
    {
        builder.Services.AddScoped<IProductMongoContext, ProductMongoContext>();
        builder.Services.AddMediatR(_ => _.RegisterServicesFromAssembly(assembly));
    }
    private static void ConfigureSerilogLogger(this WebApplicationBuilder builder)
    {
        var loggerConfig = new LoggerConfiguration()
                                 .ReadFrom.Configuration(builder.Configuration).CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(loggerConfig);
    }
    private static void RegisterPipelineBehavior(this WebApplicationBuilder builder, Assembly assembly)
    {
        builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationRequestBehavior<,>));
        builder.Services.AddScoped<IDomainNotificationContext, DomainNotificationContext>();

        AssemblyScanner
           .FindValidatorsInAssembly(assembly)
           .ForEach(result => builder.Services.AddScoped(result.InterfaceType, result.ValidatorType));
    }

}
