using MongoDB.Bson.Serialization.Attributes;

namespace Product.Api.Domain.Entities;

public class BsonProduct(string id, string description, double price)
{
    [BsonId]
    public string Id { get; init; } = id;
    public string Description { get; init; } = description;
    public double Price { get; init; } = price;
}


public static class BsonProductFactoryExtension
{
    public static BsonProduct ToBsonProduct(this Product product)
    => new BsonProduct(product.Id.ToString(), product.Description, product.Price);

    public static BsonProduct SetId(this BsonProduct product, Guid Id)
    => new BsonProduct(Id.ToString(), product.Description, product.Price);
}