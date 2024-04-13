using FluentValidation;
using MediatR;
using Product.Api.Domain.Responses;

namespace Product.Api.Domain.Command;

public sealed record ProductUpdateCommand(Guid Id,string Description, double Price) : IRequest<ProductUpdatedResponse>;


public class ProductUpdateCommandValidatior : AbstractValidator<ProductUpdateCommand>
{
    public ProductUpdateCommandValidatior()
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

public static class ProductUpdateCommandExtensions
{
    public static Entities.Product ChangeProduct(this ProductUpdateCommand productUpdateCommand, Entities.Product product)
    {
        product.SetDescription(productUpdateCommand.Description);
        product.SetPrice(productUpdateCommand.Price);
        return product;
    }
}