using Pizzeria_API.Data;
using System.Collections.Generic;

namespace Pizzeria_API.Models
{
    public enum Status { InProgress, Submitted, Delivered, Cancelled};

    public class Order
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public List<Product> Pizzas { get; set; }
        public List<Soda> Sodas { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public double Total { get; set; }

        public Order()
        {
            Id = GetId();
            Pizzas = new List<Product>();
            Sodas = new List<Soda>();
            Ingredients = new List<Ingredient>();
        }

        private int GetId()
        {
            var orders = Orders.GetOrders();
            return orders.Queue.Count + 1;
        }
    }
}
