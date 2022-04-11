using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{

    private readonly ICatalogContext _catalogContext;
    
    public ProductRepository(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
    }
    
    public async Task<IEnumerable<Product>> GetProducts()
    {
        using var productsCursor = await _catalogContext
            .ProductsCollection
            .FindAsync(p => true);
        
        return await productsCursor.ToListAsync();
    }

    public async Task<Product> GetProduct(string id)
    {
        using var productsCursor = await _catalogContext
            .ProductsCollection
            .FindAsync(p => p.Id.Equals(id));
        return await productsCursor.FirstAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        var filter = Builders<Product>.Filter.Eq(product => product.Name, name);

        using var productsCursor = await _catalogContext
            .ProductsCollection
            .FindAsync(filter);
        return await productsCursor.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        
        var filter = Builders<Product>.Filter.Eq(product => product.Category, categoryName);

        using var productsCursor = await _catalogContext
            .ProductsCollection
            .FindAsync(filter);
        return await productsCursor.ToListAsync();
    }

    public async Task CreateProduct(Product product)
    { 
        await _catalogContext
           .ProductsCollection
           .InsertOneAsync(product);
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _catalogContext
            .ProductsCollection
            .ReplaceOneAsync(p => p.Id.Equals(product.Id), product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var filter = Builders<Product>.Filter.Eq(product => product.Id, id);

        var deleteResult = await _catalogContext
            .ProductsCollection
            .DeleteOneAsync(filter);
        
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}