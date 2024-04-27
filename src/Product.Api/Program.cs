using Microsoft.EntityFrameworkCore;
using Product.Api.Extensions;
using Product.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.RegisterServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment() && app.Environment.IsProduction())
    using (var serviceScope = app.Services.CreateScope())
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<ProductDbContext>();
        if (context.Database.IsRelational())
            context.Database.Migrate();
    }

app.MapGroup("product")
   .MapProductApi();

app.UseHttpsRedirection();
app.Run();

public partial class Program { }