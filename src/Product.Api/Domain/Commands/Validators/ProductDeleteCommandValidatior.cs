using FluentValidation;
using Product.Api.Domain.Command;
using Product.Api.Domain.Constants;
using Product.Api.Domain.Notifications;
using Product.Api.Infrastructure.Data;
using Product.Api.Infrastructure.Data.ReadOnly;

namespace Product.Api.Domain.Commands.Validators;

public class ProductDeleteCommandValidatior : AbstractValidator<ProductDeleteCommand>
{
    public ProductDeleteCommandValidatior(ProductDbContext productDbContext,
                                          IProductMongoContext productMongoContext,
                                          IDomainNotificationContext domainNotificationContext)
    {
        RuleFor(x => x.Id).Custom((id, ctx) =>
        {
            var product = productDbContext.Products.Find(id);
            var productReadOnly = productMongoContext.Get(id);

            if (product is null && productReadOnly is null)
                domainNotificationContext.NotifyNotFound(Errors.PRODUCT_NOT_FOUND);
        });
    }
}
