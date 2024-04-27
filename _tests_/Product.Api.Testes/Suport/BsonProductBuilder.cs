using Bogus;
using FizzWare.NBuilder;
using Product.Api.Domain.Entities;

namespace Product.Api.Testes.Suport
{
    public sealed class BsonProductBuilder
    {
        public static IList<BsonProduct> Build(int length)
        => Builder<BsonProduct>
                .CreateListOfSize(length)
                .All()
                .WithFactory(() =>
                {
                    return new Faker<BsonProduct>("pt_BR")
                    .SkipConstructor()
                    .RuleFor(x => x.Id, f => f.Random.Guid().ToString())
                    .RuleFor(x => x.Description, f => f.Commerce.Product())
                    .RuleFor(x => x.Price, f => double.Parse(f.Commerce.Price()));
                })
                .Build();
    }
}
