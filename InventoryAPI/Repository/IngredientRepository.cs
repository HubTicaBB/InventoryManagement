using InventoryAPI.Models;
using InventoryAPI.Persistence;
using InventoryAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAPI.Repository
{                                 
    public class IngredientRepository : IIngredientRepository
    {                                                                          
        private readonly InventoryDbContext _db;

        public IngredientRepository(InventoryDbContext db)                
        {
            _db = db;
        }

        public IEnumerable<Ingredient> GetAll() => _db.Ingredients.ToList();

        public void PlaceManualOrder(IngredientDto ingredient)
        {
            var existingIngredient = _db.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
            if (existingIngredient is not null)
            {
                existingIngredient.QuantityOnStock += ingredient.ReorderQuantity;
                _db.SaveChanges();
            }
        }

        public void PlaceBulkOrder(IEnumerable<Ingredient> ingredients)
        {
            ingredients.ToList().ForEach(ingredient => 
            {
                PlaceManualOrder(new IngredientDto() { Id = ingredient.Id, ReorderQuantity = 10 });
            });
        }

        public int GetId(string name) => _db.Ingredients
            .Where(i => i.Name == name)
            .Select(p => p.Id).FirstOrDefault();

        public bool CheckIfAllOnStock(IEnumerable<OrderItem> orderItems)
        {
            foreach (var item in orderItems)
            {
                var ingredientId = GetId(item.Name);
                var ingredient = new IngredientDto { Id = ingredientId, ReorderQuantity = item.Quantity };
                if (IsOutOfStock(ingredient))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsOutOfStock(IngredientDto ingredient)
        {
            var existingIngredient = GetIngredient(ingredient.Id);
            return (existingIngredient.QuantityOnStock < ingredient.ReorderQuantity);
        }

        public void ReduceStockUnits(IngredientDto ingredient)
        {
            var existingIngredient = GetIngredient(ingredient.Id);
            if (existingIngredient is not null)
            {
                existingIngredient.QuantityOnStock -= ingredient.ReorderQuantity;
                _db.SaveChanges();
            }
        }

        private Ingredient GetIngredient(int id) => _db.Ingredients.FirstOrDefault(i => i.Id == id);
    }
}
