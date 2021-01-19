﻿using Pizzeria.Models;

namespace Pizzeria.Data.Factory
{
	public abstract class ProductFactory
	{
		public Product GetProduct(string name)
		{
			var product = CreateProduct(name);
			return product;
		}

		protected abstract Product CreateProduct(string name);
	}
}
