using System;
using System.Collections.Generic;
using System.Linq;

namespace Filters
{
    public static class ProductService
    {
        public static List<Product> FilterProducts(List<Product> products, Func<Product, bool> filter)
        {
            var filteredProducts = new List<Product>();

            foreach (var product in products)
            {
                if (filter(product))
                    filteredProducts.Add(product);
            }

            return filteredProducts;
        }
        // Метод Not
        public static Func<Product, bool> Not(Func<Product, bool> filter)
        {
            return product => !filter(product);
        }
        // Метод And (логическое И)
        public static Func<Product, bool> And(params Func<Product, bool>[] filters)
        {
            return product =>
            {
                foreach (var filter in filters)
                {
                    if (!filter(product)) return false; // Если хотя бы один фильтр не проходит, возвращаем false
                }
                return true;
            };
        }
        // Метод Or (логическое ИЛИ)
        public static Func<Product, bool> Or(params Func<Product, bool>[] filters)
        {
            return product =>
            {
                foreach (var filter in filters)
                {
                    if (filter(product)) return true; // Если хотя бы один фильтр проходит, возвращаем true
                }
                return false;
            };
        }
        // Метод Xor (исключающее ИЛИ)
        public static Func<Product, bool> Xor(params Func<Product, bool>[] filters)
        {
            return product =>
            {
                int trueCount = 0;

                foreach (var filter in filters)
                {
                    if (filter(product)) trueCount++;
                }

                return trueCount == 1; // Должно быть ровно одно совпадение
            };
        }
    }
}

