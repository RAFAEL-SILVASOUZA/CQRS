using FluentValidation;
using MediatR;
using Product.Api.Domain.Responses;

namespace Product.Api.Domain.Command;

public sealed record ProductCreateCommand(string Description, double Price) : IRequest<ProductCreatedResponse>;


public class ProductCreateCommandValidatior : AbstractValidator<ProductCreateCommand>
{
    public ProductCreateCommandValidatior()
    {
        RuleFor(_ => _.Description)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(_ => _.Price)
            .NotNull()
            .GreaterThan(0);
    }
}

public static class ProductCreateCommandExtensions
{
    public static Entities.Product ToProduct(this ProductCreateCommand productCreateCommand)
    => new Entities.Product(Guid.NewGuid(), productCreateCommand.Description, productCreateCommand.Price);
}

