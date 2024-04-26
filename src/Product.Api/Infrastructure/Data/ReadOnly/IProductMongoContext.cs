using Product.Api.Domain.Entities;


namespace Product.Api.Infrastructure.Data.ReadOnly;

public interface IProductMongoContext
{
    Task<List<BsonProduct>> GetAsync();
    Task<BsonProduct?> GetAsync(Guid id);
    BsonProduct? Get(Guid id);
    Task<List<BsonProduct>> GetAsync(string description);
}
