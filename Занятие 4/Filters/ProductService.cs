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

        public static Func<Product, bool> Not(Func<Product, bool> filter)
        {
            return p => !filter(p);
        }

        public static Func<Product, bool> And(params Func<Product, bool>[] filters)
        {
            return p => filters.All(filter => filter(p));
        }

        public static Func<Product, bool> Or(params Func<Product, bool>[] filters)
        {
            return p => filters.Any(filter => filter(p));
        }

        public static Func<Product, bool> Xor(params Func<Product, bool>[] filters)
        {
            return p => filters.Count(filter => filter(p)) == 1;
        }
    }
}
