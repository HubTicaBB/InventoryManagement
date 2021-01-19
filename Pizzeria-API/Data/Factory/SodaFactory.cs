using Pizzeria_API.Models;

namespace Pizzeria_API.Data.Factory
{
    public class SodaFactory : ItemFactory<Soda>
    {
        protected override Soda CreateItem(string name)
        {
            switch (name)
            {
                case (nameof(Fanta)):
                    {
                        return new Fanta();
                    }
                case (nameof(CocaCola)):
                    {
                        return new CocaCola();
                    }
                case (nameof(Sprite)):
                    {
                        return new Sprite();
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }

    public class Fanta : Soda
    {
        public Fanta()
        {
            Name = "Fanta";
            Price = 20;
        }
    }
    public class CocaCola : Soda
    {
        public CocaCola()
        {
            Name = "Coca Cola";
            Price = 20;
        }
    }
    public class Sprite : Soda
    {
        public Sprite()
        {
            Name = "Sprite";
            Price = 25;
        }
    }

}
