using Pizzeria_API.Models;
using System.Collections.Generic;

namespace Pizzeria_API.Data.Factory
{
     class PizzaFactory : ProductFactory
     {
       
        protected override Product CreateProduct(string name)
        {
			switch (name)
			{
				case (nameof(Margherita)):
					{
                        return new Margherita();
					}
				case (nameof(Hawaii)):
					{
                        return new Hawaii();
					}
                case (nameof(Kebabpizza)):
                    {
                        return new Kebabpizza();
                    }
                case (nameof(QuatroStagioni)):
                    {
                        return new QuatroStagioni();
                    }
                default:
                    {
						return null;
                    }
			}
		}
     }

    public class Margherita : Product
    {        
        public Margherita()
        {
            Name = "Margherita";
            Price = 85;
            Ingredients = new List<Ingredient>() 
            { 
                new Ingredient{Name = "cheese", Price = 0},
                new Ingredient{Name = "tomato sauce", Price = 0}
            };
        }
    }
    public class Hawaii : Product
    {
        public Hawaii()
        {
            Name = "Hawaii";
            Price = 95;
            Ingredients = new List<Ingredient>()
            {
                new Ingredient{Name = "cheese"},
                new Ingredient{Name = "tomato sauce"},
                new Ingredient{Name = "ham"},
                new Ingredient{Name = "pineapple"}
            };
        }
    }
    public class Kebabpizza : Product
    {
        public Kebabpizza()
        {
            Name = "Kebabpizza";
            Price = 105;
            Ingredients = new List<Ingredient>()
            {
                new Ingredient{Name = "cheese"},
                new Ingredient{Name = "tomato sauce"},
                new Ingredient{Name = "kebab"},
                new Ingredient{Name = "mushrooms"},
                new Ingredient{Name = "onion"},
                new Ingredient{Name = "iceberg salad"},
                new Ingredient{Name = "tomato kebab sauce"}
            };
        }
    }
    public class QuatroStagioni : Product
    {
        public QuatroStagioni()
        {
            Name = "Quatro Stagioni";
            Price = 115;
            Ingredients = new List<Ingredient>()
            {
                new Ingredient{Name = "cheese"},
                new Ingredient{Name = "tomato sauce"},
                new Ingredient{Name = "ham"},
                new Ingredient{Name = "shrimps"},
                new Ingredient{Name = "mussels"},
                new Ingredient{Name = "mushrooms"},
                new Ingredient{Name = "artichoke"}
            };
        }
    }
}
