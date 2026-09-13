// Layer: language. Run: dotnet run --project examples/csharp/01-language-basics

var products = new List<Product>
{
    new(1, "Apple", 3.50m, true),
    new(2, "Bread", 4.20m, true),
    new(3, "Hidden demo item", 99m, false),
};

var visible = products.Where(product => product.IsActive).ToList();
var subtotal = CalculateTotal(visible);
IDiscountPolicy policy = new ThresholdDiscountPolicy(7m, 0.10m);
var total = policy.Apply(subtotal);

Console.WriteLine($"Active products: {string.Join(", ", visible.Select(product => product.Name))}");
Console.WriteLine($"Subtotal={subtotal:C}; total after policy={total:C}");
Console.WriteLine($"Generic Last<T>: {Last(visible).Name}");

try
{
    _ = Last(Array.Empty<int>());
}
catch (InvalidOperationException exception)
{
    Console.WriteLine($"Expected exception: {exception.Message}");
}

static decimal CalculateTotal(IEnumerable<Product> products) =>
    products.Sum(product => product.Price);

static T Last<T>(IReadOnlyList<T> values)
{
    if (values.Count == 0) throw new InvalidOperationException("The sequence is empty.");
    return values[^1];
}

internal sealed record Product(int Id, string Name, decimal Price, bool IsActive);

internal interface IDiscountPolicy
{
    decimal Apply(decimal subtotal);
}

internal sealed class ThresholdDiscountPolicy(decimal threshold, decimal rate) : IDiscountPolicy
{
    public decimal Apply(decimal subtotal) => subtotal >= threshold ? subtotal * (1 - rate) : subtotal;
}

