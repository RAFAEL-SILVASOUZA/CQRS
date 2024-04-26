using MongoDB.Driver;

namespace Product.Worker.Infrastructure.Data;

public class ProductMongoContext : IProductMongoContext
{
    private readonly IMongoCollection<Domain.Entities.Product> _productsCollection;

    public ProductMongoContext(IConfiguration configuration)
    {
        var connectionString = configuration["Mongo:ConnectionString"];
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(configuration["Mongo:DatabaseName"]);
        _productsCollection = mongoDatabase.GetCollection<Domain.Entities.Product>(configuration["Mongo:ProductCollectionName"]);
    }

    public async Task CreateAsync(Domain.Entities.Product newProduct) =>
        await _productsCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(Guid id, Domain.Entities.Product updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(x => x.Id == id.ToString(), updatedProduct);

    public async Task DeleteAsync(Guid id) =>
        await _productsCollection.DeleteOneAsync(x => x.Id == id.ToString());
}
