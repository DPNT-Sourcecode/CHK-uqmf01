namespace BeFaster.App.Solutions.Checkout
{
    public interface IPriceDatabase
    {
        int GetIndividualPriceFor(char sku);
    }
}