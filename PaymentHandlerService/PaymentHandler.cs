using Databases;

namespace PaymentHandlerService;

public class PaymentHandler(IDatabase database)
{
    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await database.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await database.Products.FindAsync(id);

    public async Task<Product> CreateAsync(Product product)
    {
        database.Products.Add(product);
        await database.SaveChangesAsync();
        return product;
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
