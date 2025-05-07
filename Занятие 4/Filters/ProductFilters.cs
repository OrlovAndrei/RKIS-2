using System;
using System.Collections.Generic;
using System.Linq;

namespace Filters
{
    public enum Category
    {
        Electronics,
        Clothing
    }
    public class Product
    {
        public string Name { get; }
        public DateTime ProductionDate { get; }
        public TimeSpan WarrantyPeriod { get; }
        public Category Category { get; }
        public decimal Price { get; }
        public int Stock { get; }
        public Product(string name, DateTime productionDate, TimeSpan warrantyPeriod, Category category, decimal price, int stock)
        {
            Name = name;
            ProductionDate = productionDate;
            WarrantyPeriod = warrantyPeriod;
            Category = category;
            Price = price;
            Stock = stock;
        }
    }
    public static class ProductFilters
    {
        public static Func<Product, bool> FilterByCategory(Category category) =>
            p => p.Category == category;

        public static Func<Product, bool> FilterByPrice(decimal minPrice, decimal maxPrice) =>
            p => p.Price >= minPrice && p.Price <= maxPrice;

        public static Func<Product, bool> FilterByStock(int minStock) =>
         p => p.Stock >= minStock;
    }
    public static class ProductService
    {
        public static List<Product> FilterProducts(List<Product> products, Func<Product, bool> filter) =>
            products.Where(filter).ToList();

        public static Func<Product, bool> Not(Func<Product, bool> filter) =>
            p => !filter(p);
        public static Func<Product, bool> And(params Func<Product, bool>[] filters) =>
            p => filters.All(f => f(p));
        public static Func<Product, bool> Or(params Func<Product, bool>[] filters) =>
            p => filters.Any(f => f(p));
        public static Func<Product, bool> Xor(params Func<Product, bool>[] filters)

        {
            return p =>
            {
                int trueCount = 0;
                foreach (var filter in filters)
                {
                    if (filter(p))
                    {
                        trueCount++;
                    }
                }
                return trueCount == 1; // Xor - true только если один фильтр true
            };
        }
    }
}
