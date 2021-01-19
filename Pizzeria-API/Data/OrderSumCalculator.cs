using Pizzeria_API.Models;

namespace Pizzeria_API.Data
{
    public class OrderSumCalculator
    {
        public double CalculateOrderSum(Order order)
        {
            double sum = 0;
            foreach (var pizza in order.Pizzas)
            {
                sum += GetPizzaPrice(pizza);
            }
            foreach (var soda in order.Sodas)
            {
                sum += GetSodaPrice(soda);
            }
            foreach (var ingredient in order.Ingredients)
            {
                sum += GetIngredientPrice(ingredient);
            }

            return sum;
        }

        private double GetIngredientPrice(Ingredient ingredient)
        {
            return ingredient.Price;
        }

        private double GetPizzaPrice(Product pizza)
        {
            return pizza.Price;
        }
        private double GetSodaPrice(Soda soda)
        {
            return soda.Price;
        }
    }
}
