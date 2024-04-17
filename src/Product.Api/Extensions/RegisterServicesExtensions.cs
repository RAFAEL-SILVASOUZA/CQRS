using FluentValidation;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Product.Api.Domain.Notifications;
using Product.Api.Infrastructure.Behavior;
using Product.Api.Infrastructure.Data;
using Product.Api.Infrastructure.Data.ReadOnly;
using Serilog;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;
using Serilog.Sinks.GraylogGelf;

namespace Product.Api.Extensions;

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


        builder.ConfigureSerilogLogger();

        return builder;
    }

    private static void ConfigureSerilogLogger(this WebApplicationBuilder builder)
    {
        var loggerConfig  = new LoggerConfiguration()
                                 .ReadFrom.Configuration(builder.Configuration).CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(loggerConfig);
    }
}
