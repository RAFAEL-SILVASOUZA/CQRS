using Product.Api.Domain.Entities;


namespace Product.Api.Infrastructure.Data.ReadOnly;

public interface IProductMongoContext
{
    Task<List<BsonProduct>> GetAsync();
    Task<BsonProduct?> GetAsync(Guid id);
    Task<List<BsonProduct>> GetAsync(string description);
    Task CreateAsync(BsonProduct newProduct);
    Task UpdateAsync(Guid id, BsonProduct updatedProduct);
    Task DeleteAsync(Guid id);
}
