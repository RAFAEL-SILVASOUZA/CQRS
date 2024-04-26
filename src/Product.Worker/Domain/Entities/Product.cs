using MongoDB.Bson.Serialization.Attributes;

namespace Product.Worker.Domain.Entities;

public class Product(string id, string description, double price)
{
    [BsonId]
    public string Id { get; init; } = id;
    public string Description { get; init; } = description;
    public double Price { get; init; } = price;
}