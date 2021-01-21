using InventoryAPI.Models;
using InventoryAPI.Persistence;
using InventoryAPI.Repository.IRepository;
using System.Linq;

namespace InventoryAPI.Repository
{                                 
    public class IngredientRepository : Repository<Ingredient>, IIngredientRepository                                      
    {                                                                          
        private readonly InventoryDbContext _db;

        public IngredientRepository(InventoryDbContext db) : base(db)                      
        {
            _db = db;
        }
                                                                                                                                                                                
        public void Update(IngredientDto ingredient)
        {
            var existingIngredient = _db.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
            if (existingIngredient is not null)
            {
                existingIngredient.QuantityOnStock += ingredient.ReorderQuantity;
                _db.SaveChanges();  
            }
        }
    }
}
