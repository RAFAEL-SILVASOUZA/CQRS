using Product.Api.Domain.Command;
using Product.Api.Domain.Commands.Validators;
using Product.Api.Domain.Notifications;
using Product.Api.Infrastructure.Data;

namespace Product.Api.Filters
{
    public class ProductDeleteEndpointFilter(ProductDbContext productDbContext, IDomainNotificationContext domainNotificationContext) : IEndpointFilter
    {

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<Guid>(2);
            var command = new ProductDeleteCommand(id);
            var validator = new ProductDeleteCommandValidatior(productDbContext, domainNotificationContext);
            _ = await validator.ValidateAsync(command);
            return await next(context);
        }
    }
}
