using InventoryAPI.Models;
using System.Collections.Generic;

namespace InventoryAPI.Repository.IRepository
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        void PlaceManualOrder(IngredientDto ingredient);

        void PlaceBulkOrder(IEnumerable<Ingredient> ingredients);

        void ReduceStockUnits(IngredientDto ingredient);

        int GetId(string name);
    }
}
