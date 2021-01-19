namespace InventoryAPI.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public double QuantityOnStock { get; set; }
    }
}
