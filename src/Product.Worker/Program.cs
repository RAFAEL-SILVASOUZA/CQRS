using Product.Worker;
using Product.Worker.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.RegisterServices();

var host = builder.Build();
host.Run();
