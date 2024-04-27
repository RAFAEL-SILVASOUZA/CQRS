using MongoDB.Bson.Serialization.Attributes;

namespace Product.Api.Domain.Entities;

public class BsonProduct
{
    public BsonProduct(string id, string description, double price)
    {
        Id = id;
        Description = description;
        Price = price;
    }

    public BsonProduct Create(string id, string description, double price) 
        => new BsonProduct(id, description, price);

    [BsonId]
    public string Id { get; init; } 
    public string Description { get; init; } 
    public double Price { get; init; }
}


public static class BsonProductFactoryExtension
{
    public static BsonProduct ToBsonProduct(this Product product)
    => new BsonProduct(product.Id.ToString(), product.Description, product.Price);

    public static BsonProduct SetId(this BsonProduct product, Guid Id)
    => new BsonProduct(Id.ToString(), product.Description, product.Price);
}