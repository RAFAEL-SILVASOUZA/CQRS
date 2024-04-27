using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;

namespace Product.Api.Testes.Suport;

public class DbContainerTest : IAsyncLifetime
{
    public readonly MongoDbContainer _mongoDbContainer =
        new MongoDbBuilder().Build();

    public readonly PostgreSqlContainer _postgreSqlContainer =
        new PostgreSqlBuilder().Build();

    public async Task SetValuesMongo<T>(IConfiguration configuration, List<T> values) where T : class
    {
        var client = new MongoClient(_mongoDbContainer.GetConnectionString());
        var database = client.GetDatabase(configuration["Mongo:DatabaseName"]);
        var productsCollection = database.GetCollection<T>(configuration["Mongo:ProductCollectionName"]);
        await productsCollection.InsertManyAsync(values);
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        await _postgreSqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync().AsTask();
        await _postgreSqlContainer.DisposeAsync().AsTask();
    }
}
