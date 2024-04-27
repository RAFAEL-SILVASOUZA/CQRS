using Bogus;
using System.Runtime.Serialization;

namespace Product.Api.Testes.Suport
{
    public static class ExtensionsForFaker
    {
        public static Faker<T> SkipConstructor<T>(this Faker<T> fakerOfT) where T : class
        {
            return fakerOfT.CustomInstantiator(_ =>
            {
                return FormatterServices.GetUninitializedObject(typeof(T)) as T;
            });
        }
    }
}
