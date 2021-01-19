using Pizzeria_API.Models;

namespace Pizzeria_API.Data.Factory
{
    public class IngredientFactory : ItemFactory<Ingredient>
    {

        protected override Ingredient CreateItem(string name)
        {
            switch (name)
            {
                case (nameof(Ham)):
                    {
                        return new Ham();
                    }
                case (nameof(Pineapple)):
                    {
                        return new Pineapple();
                    }
                case (nameof(Mushrooms)):
                    {
                        return new Mushrooms();
                    }
                case (nameof(Onion)):
                    {
                        return new Onion();
                    }
                case (nameof(KebabSauce)):
                    {
                        return new KebabSauce();
                    }
                case (nameof(Shrimps)):
                    {
                        return new Shrimps();
                    }
                case (nameof(Mussels)):
                    {
                        return new Mussels();
                    }
                case (nameof(Artichoke)):
                    {
                        return new Artichoke();
                    }
                case (nameof(Kebab)):
                    {
                        return new Kebab();
                    }
                case (nameof(Coriander)):
                    {
                        return new Coriander();
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }

    public class Ham : Ingredient
    {
        public Ham()
        {
            Name = "Ham";
            Price = 10;
        }
    }
    public class Pineapple : Ingredient
    {
        public Pineapple()
        {
            Name = "Pineapple";
            Price = 10;
        }
    }
    public class Mushrooms : Ingredient
    {
        public Mushrooms()
        {
            Name = "Mushrooms";
            Price = 10;
        }
    }
    public class Onion : Ingredient
    {
        public Onion()
        {
            Name = "Onion";
            Price = 10;
        }
    }
    public class KebabSauce : Ingredient
    {
        public KebabSauce()
        {
            Name = "Kebab sauce";
            Price = 10;
        }
    }
    public class Shrimps : Ingredient
    {
        public Shrimps()
        {
            Name = "Shrimps";
            Price = 15;
        }
    }
    public class Mussels : Ingredient
    {
        public Mussels()
        {
            Name = "Mussels";
            Price = 15;
        }
    }
    public class Artichoke : Ingredient
    {
        public Artichoke()
        {
            Name = "Artichoke";
            Price = 15;
        }
    }
    public class Kebab : Ingredient
    {
        public Kebab()
        {
            Name = "Kebab";
            Price = 20;
        }
    }
    public class Coriander : Ingredient
    {
        public Coriander()
        {
            Name = "Coriander";
            Price = 20;
        }
    }
}
