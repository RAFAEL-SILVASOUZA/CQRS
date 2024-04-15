using FluentValidation;
using Product.Api.Domain.Command;
using Product.Api.Domain.Constants;
using Product.Api.Infrastructure.Data;

namespace Product.Api.Domain.Commands.Validators;

public class ProductUpdateCommandValidatior : AbstractValidator<ProductUpdateCommand>
{
    public ProductUpdateCommandValidatior(ProductDbContext productDbContext)
    {
        RuleFor(x => x.Id).Custom((id, ctx) =>
        {
            var product = productDbContext.Products.Find(id);

            if (product is null)
                ctx.AddFailure(new FluentValidation.Results.ValidationFailure("Id", Errors.PRODUCT_NOT_FOUND));
        });

        RuleFor(_ => _.Description)
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(_ => _.Price)
            .NotNull()
            .GreaterThan(0);
    }
}
