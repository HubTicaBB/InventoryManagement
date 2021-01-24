using System.Collections.Generic;

namespace InventoryAPI.Models.DTO
{
    public class OrderDto
    {
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
