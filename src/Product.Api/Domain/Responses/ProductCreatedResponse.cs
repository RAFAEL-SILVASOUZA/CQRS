namespace Product.Api.Domain.Responses;

public sealed record ProductCreatedResponse(Guid Id, string Description, double Price) : IResponse;

public static class ProductCreatedResponseFactoryExtension
{
    public static ProductCreatedResponse ToProductCreatedResponse(this Entities.Product product)
    => new ProductCreatedResponse(product.Id, product.Description, product.Price);
}