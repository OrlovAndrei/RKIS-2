namespace Filters
{
    public static class ProductService
    {
        // Метод для фильтрации продуктов
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

        // Метод Not: логическое отрицание фильтра
        public static Func<Product, bool> Not(Func<Product, bool> filter)
        {
            return product => !filter(product);
        }

        // Метод And: логическое И для нескольких фильтров
        public static Func<Product, bool> And(params Func<Product, bool>[] filters)
        {
            return product => filters.All(filter => filter(product));
        }

        // Метод Or: логическое ИЛИ для нескольких фильтров
        public static Func<Product, bool> Or(params Func<Product, bool>[] filters)
        {
            return product => filters.Any(filter => filter(product));
        }

        // Метод Xor: исключающее ИЛИ для нескольких фильтров
        public static Func<Product, bool> Xor(params Func<Product, bool>[] filters)
        {
            return product => filters.Count(filter => filter(product)) == 1;
        }
    }
}