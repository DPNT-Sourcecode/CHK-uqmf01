namespace BeFaster.App.Solutions.Checkout
{
    public class GetOneFreeOffer
    {
        public GetOneFreeOffer(int quantity, char freeSku)
        {
            Quantity = quantity;
            FreeSku = freeSku;
        }

        public int Quantity { get; }
        public char FreeSku { get; }
    }
}