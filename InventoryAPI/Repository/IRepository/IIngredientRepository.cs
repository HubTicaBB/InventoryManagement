using InventoryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace InventoryAPI.Repository.IRepository
{
    public interface IIngredientRepository
    {
        IEnumerable<Ingredient> GetAll();

        IActionResult PlaceManualOrder(IngredientDto ingredient);

        IActionResult PlaceBulkOrder();

        IActionResult ConsumeIngredients(IEnumerable<OrderItem> orderItems);
    }
}
