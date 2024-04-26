using DotNetCore.CAP;
using Product.Worker.Constants;
using Product.Worker.Infrastructure.Data;

namespace Product.Worker.Consumer;

public interface IProductDeleteConsumer
{
    Task Handle(string id);
}

public sealed class ProductDeleteConsumer(IProductMongoContext productMongoContext) : IProductDeleteConsumer, ICapSubscribe
{
    [CapSubscribe(Queues.PRODUCT_DELETED)]
    public Task Handle(string id)
    => productMongoContext.DeleteAsync(new Guid(id));
}
