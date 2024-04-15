namespace Product.Api.Domain.Responses;

public sealed record ProductUpdatedResponse(Guid Id, string Description, double Price) : IResponse;

public static class ProductUpdatedResponseFactoryExtension
{
    public static ProductUpdatedResponse ToProductUpdatedResponse(this Entities.Product product)
    => new ProductUpdatedResponse(product.Id, product.Description, product.Price);
}
