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
                return product => !filter(product);
            }

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
