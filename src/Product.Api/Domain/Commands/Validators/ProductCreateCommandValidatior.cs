using FluentValidation;
using Product.Api.Domain.Command;

namespace Product.Api.Domain.Commands.Validators;

public class ProductCreateCommandValidatior : AbstractValidator<ProductCreateCommand>
{
    public ProductCreateCommandValidatior()
    {
        RuleFor(_ => _.Description)
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(_ => _.Price)
            .NotNull()
            .GreaterThan(0);
    }
}

