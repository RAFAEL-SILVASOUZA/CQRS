using FluentValidation;
using Product.Api.Domain.Command;
using Product.Api.Domain.Constants;
using Product.Api.Domain.Notifications;
using Product.Api.Infrastructure.Data;

namespace Product.Api.Domain.Commands.Validators;

public class ProductDeleteCommandValidatior : AbstractValidator<ProductDeleteCommand>
{
    public ProductDeleteCommandValidatior(ProductDbContext productDbContext, IDomainNotificationContext domainNotificationContext)
    {
        RuleFor(x => x.Id).Custom((id, ctx) =>
        {
            var product = productDbContext.Products.Find(id);

            if (product is null)
                domainNotificationContext.NotifyNotFound(Errors.PRODUCT_NOT_FOUND);
        });
    }
}
