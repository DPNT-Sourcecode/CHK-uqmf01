namespace BeFaster.App.Solutions.Checkout
{
    public interface IPriceDatabase
    {
        /// <summary>
        /// Returns the cost of buying a single item with this sku.
        /// If the sku is invalid it will throw an <see cref="SkuInvalidException"/>.
        /// </summary>
        int GetIndividualPriceFor(char sku);

        /// <summary>
        /// Returns a MultiPrice defining an offer for this sku.
        /// Returns null if there is no offer for this sku.
        /// If the sku is invalid it will throw an <see cref="SkuInvalidException"/>.
        /// </summary>
        MultiPrice GetMultiPriceOfferFor(char sku);
    }
}