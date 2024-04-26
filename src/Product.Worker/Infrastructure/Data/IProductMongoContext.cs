namespace Product.Worker.Infrastructure.Data;

public interface IProductMongoContext
{
    Task CreateAsync(Domain.Entities.Product newProduct);
    Task UpdateAsync(Guid id, Domain.Entities.Product updatedProduct);
    Task DeleteAsync(Guid id);
}
