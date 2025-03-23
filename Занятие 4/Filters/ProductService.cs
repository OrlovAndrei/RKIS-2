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

		// Метод Not: инвертирует результат фильтра
        public static Func<Product, bool> Not(Func<Product, bool> filter)
        {
            return product => !filter(product);
        }

        // Метод And: возвращает true, если все фильтры возвращают true
        public static Func<Product, bool> And(params Func<Product, bool>[] filters)
        {
            return product =>
            {
                foreach (var filter in filters)
                {
                    if (!filter(product))
                        return false;
                }
                return true;
            };
        }

        // Метод Or: возвращает true, если хотя бы один фильтр возвращает true
        public static Func<Product, bool> Or(params Func<Product, bool>[] filters)
        {
            return product =>
            {
                foreach (var filter in filters)
                {
                    if (filter(product))
                        return true;
                }
                return false;
            };
        }

        // Метод Xor: возвращает true, если только один фильтр возвращает true
        public static Func<Product, bool> Xor(params Func<Product, bool>[] filters)
        {
            return product =>
            {
                int trueCount = 0;
                foreach (var filter in filters)
                {
                    if (filter(product))
                        trueCount++;
                }
                return trueCount == 1;
            };
        }
	}
}
