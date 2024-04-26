using DotNetCore.CAP;
using Product.Worker.Constants;
using Product.Worker.Infrastructure.Data;

namespace Product.Worker.Consumer;

public interface IProductCreateConsumer
{
    Task Handle(Domain.Entities.Product product);
}

public sealed class ProductCreateConsumer(IProductMongoContext productMongoContext) : IProductCreateConsumer, ICapSubscribe
{
    [CapSubscribe(Queues.PRODUCT_CREATED)]
    public Task Handle(Domain.Entities.Product product)
    => productMongoContext.CreateAsync(product);
}
