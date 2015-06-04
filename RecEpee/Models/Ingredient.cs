
namespace RecEpee.Models
{
    public class Ingredient
    {
        public string Name { get; set; }
        public double? Quantity { get; set; }
        public string Unit { get; set; }

        public Ingredient GetWithDifferentQuantity(double newQuantity)
        {
            return new Ingredient { Name = Name, Unit = Unit, Quantity = newQuantity };
        }
    }
}
