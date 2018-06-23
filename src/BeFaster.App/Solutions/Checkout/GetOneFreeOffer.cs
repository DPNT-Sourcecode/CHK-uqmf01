namespace BeFaster.App.Solutions.Checkout
{
    public class GetOneFreeOffer
    {
        public GetOneFreeOffer(int quantity, string freeSkus)
        {
            Quantity = quantity;
            FreeSkus = freeSkus;
        }

        public int Quantity { get; }
        public string FreeSkus { get; }
    }
}
