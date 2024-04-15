using Product.Api.Domain.Command;

namespace Product.Api.Domain.Commands.CommandExtensions;

public static class ProductCreateCommandExtensions
{
    public static Entities.Product ToProduct(this ProductCreateCommand productCreateCommand)
    => new Entities.Product(Guid.NewGuid(), productCreateCommand.Description, productCreateCommand.Price);
}

