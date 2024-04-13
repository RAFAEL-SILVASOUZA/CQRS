using MongoDB.Driver;
using Product.Api.Domain.Entities;
using Product.Api.Domain.Responses;

namespace Product.Api.Infrastructure.ReadOnly;

public class ProductMongoContext : IProductMongoContext
{
    private readonly IMongoCollection<BsonProduct> _productsCollection;

    public ProductMongoContext(IConfiguration configuration)
    {
        var connectionString = configuration["Mongo:ConnectionString"];
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(configuration["Mongo:DatabaseName"]);
        _productsCollection = mongoDatabase.GetCollection<BsonProduct>(configuration["Mongo:ProductCollectionName"]);
    }

    public async Task<List<BsonProduct>> GetAsync() =>
    await _productsCollection.Find(_ => true).ToListAsync();

    public async Task<BsonProduct?> GetAsync(Guid id) =>
        await _productsCollection.Find(x => x.Id == id.ToString()).FirstOrDefaultAsync();

    public async Task<List<BsonProduct>> GetAsync(string description) =>
        await _productsCollection.Find(x => x.Description.ToUpper().Contains(description.ToUpper())).ToListAsync();

    public async Task CreateAsync(BsonProduct newProduct) =>
        await _productsCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(Guid id, BsonProduct updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(x => x.Id == id.ToString(), updatedProduct);

    public async Task DeleteAsync(Guid id) =>
        await _productsCollection.DeleteOneAsync(x => x.Id == id.ToString());
}
