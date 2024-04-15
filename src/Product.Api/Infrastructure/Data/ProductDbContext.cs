using Microsoft.EntityFrameworkCore;
using ProductModel = Product.Api.Domain.Entities.Product;

namespace Product.Api.Infrastructure.Data;

public class ProductDbContext : DbContext
{
    public DbSet<ProductModel> Products { get; set; }

    public ProductDbContext(DbContextOptions<ProductDbContext> dbContextOptions)
        : base(dbContextOptions) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
