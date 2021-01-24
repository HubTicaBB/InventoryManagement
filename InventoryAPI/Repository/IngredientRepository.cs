using InventoryAPI.Models;
using InventoryAPI.Persistence;
using InventoryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAPI.Repository
{                                 
    public class IngredientRepository : ControllerBase, IIngredientRepository
    {                                                                          
        private readonly InventoryDbContext _db;

        public IngredientRepository(InventoryDbContext db)                
        {
            _db = db;
        }

        public IEnumerable<Ingredient> GetAll() => _db.Ingredients.ToList();

        public IActionResult PlaceManualOrder(IngredientDto ingredient)
        {
            var existingIngredient = _db.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
            if (existingIngredient is null)
            {
                return NotFound();
            }
            existingIngredient.QuantityOnStock += ingredient.ReorderQuantity;
            _db.SaveChanges();
            return Ok();
        }

        public IActionResult PlaceBulkOrder()
        {
            var allIngredients = GetAll();
            allIngredients.ToList().ForEach(ingredient => 
            {
                ingredient.QuantityOnStock += 10;
                _db.SaveChanges();                
            });
            return Ok();
        }

        private int GetId(string name) => _db.Ingredients
            .Where(i => i.Name == name)
            .Select(p => p.Id).FirstOrDefault();

        private bool CheckIfAllOnStock(IEnumerable<OrderItem> orderItems)
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

        private bool IsOutOfStock(IngredientDto ingredient)
        {
            var existingIngredient = GetIngredient(ingredient.Id);
            return (existingIngredient.QuantityOnStock < ingredient.ReorderQuantity);
        }

        public IActionResult ConsumeIngredients(IEnumerable<OrderItem> orderItems)
        {
            var allOnStock = CheckIfAllOnStock(orderItems);

            if (allOnStock)
            {
                orderItems.ToList().ForEach(item =>
                {
                    var ingredientId = GetId(item.Name);
                    var ingredient = new IngredientDto { Id = ingredientId, ReorderQuantity = item.Quantity };

                    ReduceStockUnits(new IngredientDto { Id = ingredientId, ReorderQuantity = item.Quantity });
                });
                return Ok(orderItems);
            }
            return BadRequest("The order cannot be processed as some ingredients are out of stock.");
        }

        private void ReduceStockUnits(IngredientDto ingredient)
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
