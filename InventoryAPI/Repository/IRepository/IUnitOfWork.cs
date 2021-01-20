using System;

namespace InventoryAPI.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IIngredientRepository Ingredient { get; }
        // Add rest
    }
}
