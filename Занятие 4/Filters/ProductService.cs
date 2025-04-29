using System;
using System.Collections.Generic;

namespace Filters
{
    public static class ProductService
    {
        public static List<Product> FilterProducts(List<Product> products, Func<Product, bool> filter)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));
            
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var filteredProducts = new List<Product>(products.Count); // Оптимизация под размер

            foreach (var product in products)
            {
                if (filter(product))
                    filteredProducts.Add(product);
            }

            return filteredProducts;
        }

        // Метод Not (логическое отрицание)
        public static Func<Product, bool> Not(Func<Product, bool> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return product => !filter(product);
        }

        // Метод And (логическое И)
        public static Func<Product, bool> And(params Func<Product, bool>[] filters)
        {
            if (filters == null || filters.Length == 0)
                throw new ArgumentException("Filters cannot be null or empty", nameof(filters));

            return product =>
            {
                foreach (var filter in filters)
                {
                    if (filter == null)
                        throw new InvalidOperationException("One of the filters is null");

                    if (!filter(product))
                        return false;
                }
                return true;
            };
        }

        // Метод Or (логическое ИЛИ)
        public static Func<Product, bool> Or(params Func<Product, bool>[] filters)
        {
            if (filters == null || filters.Length == 0)
                throw new ArgumentException("Filters cannot be null or empty", nameof(filters));

            return product =>
            {
                foreach (var filter in filters)
                {
                    if (filter == null)
                        throw new InvalidOperationException("One of the filters is null");

                    if (filter(product))
                        return true;
                }
                return false;
            };
        }

        // Метод Xor (исключающее ИЛИ)
        public static Func<Product, bool> Xor(params Func<Product, bool>[] filters)
        {
            if (filters == null || filters.Length == 0)
                throw new ArgumentException("Filters cannot be null or empty", nameof(filters));

            return product =>
            {
                int trueCount = 0;

                foreach (var filter in filters)
                {
                    if (filter == null)
                        throw new InvalidOperationException("One of the filters is null");

                    if (filter(product))
                        trueCount++;

                    if (trueCount > 1) // Оптимизация: выход, если уже больше одного
                        return false;
                }

                return trueCount == 1;
            };
        }
    }
}
