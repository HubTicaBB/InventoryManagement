using Pizzeria_API.Data.Factory;
using System.Collections.Generic;

namespace Pizzeria_API.Models
{
    public sealed class Menu
    {
        private static Menu _instance;

        public List<Product> Pizzas { get; set; }
        public List<Soda> Sodas { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        private Menu()
        {
        }

        public static Menu GetMenu()
        {
            if (_instance is null)
            {
                ProductFactory pizzaFactory = new PizzaFactory();
                ItemFactory<Soda> sodaFactory = new SodaFactory();
                ItemFactory<Ingredient> ingredientFactory = new IngredientFactory();

                _instance = new Menu()
                {
                    Pizzas = new List<Product>
                    {
                        pizzaFactory.GetProduct(nameof(Margherita)),
                        pizzaFactory.GetProduct(nameof(Hawaii)),
                        pizzaFactory.GetProduct(nameof(Kebabpizza)),
                        pizzaFactory.GetProduct(nameof(QuatroStagioni))
                    },
                    Sodas = new List<Soda>
                    {
                        sodaFactory.GetItem(nameof(Fanta)),
                        sodaFactory.GetItem(nameof(CocaCola)),
                        sodaFactory.GetItem(nameof(Sprite))
                    },
                    Ingredients = new List<Ingredient>
                    {
                        ingredientFactory.GetItem(nameof(Ham)),
                        ingredientFactory.GetItem(nameof(Pineapple)),
                        ingredientFactory.GetItem(nameof(Mushrooms)),
                        ingredientFactory.GetItem(nameof(Onion)),
                        ingredientFactory.GetItem(nameof(KebabSauce)),
                        ingredientFactory.GetItem(nameof(Shrimps)),
                        ingredientFactory.GetItem(nameof(Mussels)),
                        ingredientFactory.GetItem(nameof(Artichoke)),
                        ingredientFactory.GetItem(nameof(Kebab)),
                        ingredientFactory.GetItem(nameof(Coriander)),
                    }
                };
            }

            return _instance;
        }
    }
}
