using InventoryAPI.Persistence;
using InventoryAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventoryDbContext _db;
        

        public UnitOfWork(InventoryDbContext db)
        {
            _db = db;
            Ingredient = new IngredientRepository(_db);
            // Rest
        }

        public IIngredientRepository Ingredient { get; private set; }
        // rest

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
