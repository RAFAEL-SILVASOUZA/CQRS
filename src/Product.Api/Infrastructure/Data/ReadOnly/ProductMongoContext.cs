using MongoDB.Driver;
using Product.Api.Domain.Entities;

namespace Product.Api.Infrastructure.Data.ReadOnly;

public class ProductMongoContext : IProductMongoContext
{
    private readonly IMongoCollection<BsonProduct> _productsCollection;

    public ProductMongoContext(string connectionString, string databaseName, string productCollectionName)
    {
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseName);
        _productsCollection = mongoDatabase.GetCollection<BsonProduct>(productCollectionName);
    }

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

    public BsonProduct? Get(Guid id) =>
        _productsCollection.Find(x => x.Id == id.ToString()).FirstOrDefault();

    public async Task<List<BsonProduct>> GetAsync(string description) =>
        await _productsCollection.Find(x => x.Description.ToUpper().Contains(description.ToUpper())).ToListAsync();

}
