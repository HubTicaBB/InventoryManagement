using InventoryAPI.Models;

namespace InventoryAPI.Repository.IRepository
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        void Update(IngredientDto ingredient);
    }
}
