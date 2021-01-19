using System.Collections.Generic;

namespace Pizzeria.Models
{
    public abstract class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
