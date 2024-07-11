using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products { get; }
    public IMongoCollection<ProductBrand> Brands { get; }
    public IMongoCollection<ProductType> Types { get; }

    public CatalogContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("DatabaseSettings:ConnectionString").Value;
        var databaseName = configuration.GetSection("DatabaseSettings:DatabaseName").Value;
        var brandsCollection = configuration.GetSection("DatabaseSettings:BrandsCollection").Value;
        var typesCollection = configuration.GetSection("DatabaseSettings:TypesCollection").Value;
        var collectionName = configuration.GetSection("DatabaseSettings:CollectionName").Value;

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        Brands = database.GetCollection<ProductBrand>(brandsCollection);
        Types = database.GetCollection<ProductType>(typesCollection);
        Products = database.GetCollection<Product>(collectionName);

        BrandContextSeed.SeedData(Brands);
        TypeContextSeed.SeedData(Types);
        CatalogContextSeed.SeedData(Products);
    } 
}