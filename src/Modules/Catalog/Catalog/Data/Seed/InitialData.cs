namespace Catalog.Data.Seed;

internal static class InitialData
{
    internal static IEnumerable<Product> Products => new List<Product>
    {
         Product.Create("IPhone X", ["category1"], "Long description", "imagefile", 500),
         Product.Create("Samsung 10", ["category1"], "Long description", "imagefile", 400),
         Product.Create("Huawei Plus", ["category2"], "Long description", "imagefile", 650),
         Product.Create("Xiaomi Mi", ["category2"], "Long description", "imagefile", 450)
    };
}
