namespace BeFaster.App.Solutions.Checkout
{
    public interface ICheckoutPricer
    {
        int CalculatePrice(string skus);
    }
}
