using Product.Worker.Consumer;
using Product.Worker.Infrastructure.Data;
using Serilog;

namespace Product.Worker.Extensions;

public static class RegisterServicesExtensions
{
    public static HostApplicationBuilder RegisterServices(this HostApplicationBuilder builder)
    {
        builder.RegisterConsumers();
        builder.RegisterMongoContext();
        builder.RegisterCap();
        builder.ConfigureSerilogLogger();
        return builder;
    }
    private static void RegisterConsumers(this HostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IProductCreateConsumer, ProductCreateConsumer>();
        builder.Services.AddTransient<IProductUpdateConsumer, ProductUpdateConsumer>();
        builder.Services.AddTransient<IProductDeleteConsumer, ProductDeleteConsumer>();
    }
    private static void RegisterCap(this HostApplicationBuilder builder)
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
        });
    }
    private static void RegisterMongoContext(this HostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IProductMongoContext, ProductMongoContext>();
    }
    private static void ConfigureSerilogLogger(this HostApplicationBuilder builder)
    {
        var loggerConfig = new LoggerConfiguration()
                                 .ReadFrom.Configuration(builder.Configuration).CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(loggerConfig);
    }
}
