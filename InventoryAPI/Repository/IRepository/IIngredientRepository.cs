using InventoryAPI.Models;
using System.Collections.Generic;

namespace InventoryAPI.Repository.IRepository
{
    public interface IIngredientRepository
    {
        IEnumerable<Ingredient> GetAll();

        void PlaceManualOrder(IngredientDto ingredient);

        void PlaceBulkOrder(IEnumerable<Ingredient> ingredients);

        bool CheckIfAllOnStock(IEnumerable<OrderItem> orderItems);

        bool IsOutOfStock(IngredientDto ingredient);

        void ReduceStockUnits(IngredientDto ingredient);

        int GetId(string name);
    }
}
