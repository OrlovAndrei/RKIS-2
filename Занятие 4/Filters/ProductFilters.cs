namespace Filters
{
	public static class ProductFilters
	{
		public static Func<Product, bool> FilterByCategory(Category category)
		{
			return product => product.Category == category;
		}

		public static Func<Product, bool> FilterByPrice(decimal minPrice, decimal maxPrice)
		{
			return product => product.Price >= minPrice && product.Price <= maxPrice;
		}

		public static Func<Product, bool> FilterByStock(int minStock)
		{
			return product => product.StockCount >= minStock;
		}

		public static Func<Product, bool> FilterByExpirationDate(DateTime date)
		{
			return product =>
    		{
       	 		// Если срок годности не указан, товар подходит
        		if (product.ExpirationDate == null)
            	return true;

        		// Вычисляем дату истечения срока годности
       			 DateTime expirationDate = product.ProductionDate + product.ExpirationDate.Value;

        		// Проверяем, что срок годности не истек
        		return expirationDate >= date;
    		};
		}

		public static Func<Product, bool> FilterByNameContains(string namePart)
		{
			return product => product.Name.Contains(namePart, StringComparison.OrdinalIgnoreCase);
		}
	}
}
