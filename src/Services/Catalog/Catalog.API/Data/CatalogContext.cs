using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
    
    public CatalogContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        var client = new MongoClient(connectionString);
        var databaseName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
        var database = client.GetDatabase(databaseName);
        var collectionName = configuration.GetValue<string>("DatabaseSettings:ConnectionName");
        ProductsCollection = database.GetCollection<Product>(collectionName); 
        CatalogContextSeed.SeedData(ProductsCollection);
    }

    public IMongoCollection<Product> ProductsCollection { get; set; }
}