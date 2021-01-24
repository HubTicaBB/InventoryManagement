using InventoryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryAPI.Persistence
{
    public class DbInitializer
    {
        public static void Seed(InventoryDbContext db)
        {
            db.Database.EnsureCreated();

            if (db.Ingredients.Any())
            {
                return;
            }

            var ingredients = new List<Ingredient>
            {
                new Ingredient
                {
                    Name = "Ham",
                    UnitPrice = 10,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Pineapple",
                    UnitPrice = 10,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Mashrooms",
                    UnitPrice = 10,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Onions",
                    UnitPrice = 10,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Kebab Sauce",
                    UnitPrice = 10,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Shrimps",
                    UnitPrice = 15,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Mussels",
                    UnitPrice = 15,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Artichoke",
                    UnitPrice = 15,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Kebab",
                    UnitPrice = 20,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Coriander",
                    UnitPrice = 20,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Cheese",
                    UnitPrice = 20,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Tomato Sauce",
                    UnitPrice = 10,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Chili",
                    UnitPrice = 15,
                    QuantityOnStock = new Random().Next(0, 200)
                },
                new Ingredient
                {
                    Name = "Iceberg",
                    UnitPrice = 10,
                    QuantityOnStock = new Random().Next(0, 200)
                }
            };

            db.Ingredients.AddRange(ingredients);
            db.SaveChanges();
        }
    }
}
