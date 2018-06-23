using System.Collections.Generic;

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
        /// Returns an IList of MultiPrice defining the offers for this sku.
        /// Returns an empty IList or null if there is no offers for this sku.
        /// If the sku is invalid it will throw an <see cref="SkuInvalidException"/>.
        /// </summary>
        IList<MultiPrice> GetMultiPriceOfferFor(char sku);

        /// <summary>
        /// Returns a GetOneFreeOffer defining an offer for this sku.
        /// Returns null if there is no GetOneFreeOffer for this sku.
        /// If the sku is invalid it will throw an <see cref="SkuInvalidException"/>.
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        GetOneFreeOffer GetGetOneFreeOfferFor(char sku);
    }
}