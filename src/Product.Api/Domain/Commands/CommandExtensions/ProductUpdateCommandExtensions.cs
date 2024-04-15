using Product.Api.Domain.Command;

namespace Product.Api.Domain.Commands.CommandExtensions;

public static class ProductUpdateCommandExtensions
{
    public static Entities.Product ChangeProduct(this ProductUpdateCommand productUpdateCommand, Entities.Product product)
    {
        product.SetDescription(productUpdateCommand.Description);
        product.SetPrice(productUpdateCommand.Price);
        return product;
    }
}