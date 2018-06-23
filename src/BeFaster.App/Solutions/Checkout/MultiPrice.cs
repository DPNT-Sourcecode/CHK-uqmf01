namespace BeFaster.App.Solutions.Checkout
{
    /// <summary>
    /// Represents an offer where if you buy this Quantity of items, the total price will be Price
    /// </summary>
    public class MultiPrice
    {
        public MultiPrice(int quantity, int price)
        {
            Quantity = quantity;
            Price = price;
        }

        public int Quantity { get; }
        public int Price { get; }
    }
}