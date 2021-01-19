using Pizzeria_API.Models;
using System.Collections.Generic;

namespace Pizzeria_API.Data
{
    public class Orders
    {
        private static Orders _instance;

        public List<Order> Queue { get; set; }

        private Orders() 
        {
            Queue = new List<Order>();
        }

        public static Orders GetOrders()
        {
            if (_instance is null)
            {
                _instance = new Orders();
                
            }

            return _instance;
        }
    }
}
