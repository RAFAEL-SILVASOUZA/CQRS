using DotNetCore.CAP;
using Product.Worker.Constants;
using Product.Worker.Infrastructure.Data;

namespace Product.Worker.Consumer;

public interface IProductUpdateConsumer
{
    Task Handle(Domain.Entities.Product product);
}

public sealed class ProductUpdateConsumer(IProductMongoContext productMongoContext) : IProductUpdateConsumer, ICapSubscribe
{
    [CapSubscribe(Queues.PRODUCT_UPDATED)]
    public Task Handle(Domain.Entities.Product product)
    => productMongoContext.UpdateAsync(new Guid(product.Id), product);
}
