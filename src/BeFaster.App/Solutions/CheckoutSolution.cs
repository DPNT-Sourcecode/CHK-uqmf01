using BeFaster.App.Solutions.Checkout;
using System.Collections.Generic;

namespace BeFaster.App.Solutions
{
    public static class CheckoutSolution
    {
        public static int Checkout(string skus)
        {
            var checkoutPricer = new CheckoutPricer(
                new InMemoryPriceDatabase());
            return checkoutPricer.CalculatePrice(skus);
        }

        /// <summary>
        /// todo I would write tests over this, but I'll leave that for now,
        /// as this is basically a stub implementation
        /// </summary>
        private class InMemoryPriceDatabase : IPriceDatabase
        {
            private readonly Dictionary<char, int> individualPrices;
            private readonly Dictionary<char, MultiPrice> multiPrices;
            private readonly Dictionary<char, GetOneFreeOffer> getOneFreeOffers;

            public InMemoryPriceDatabase()
            {
                individualPrices = new Dictionary<char, int>
                {
                    { 'A', 50 },
                    { 'B', 30 },
                    { 'C', 20 },
                    { 'D', 15 },
                    { 'E', 40 },
                };

                multiPrices = new Dictionary<char, MultiPrice>
                {
                    {'A', new MultiPrice(quantity: 3, price: 130) },
                    {'B', new MultiPrice(quantity: 2, price: 45) },
                };

                getOneFreeOffers = new Dictionary<char, GetOneFreeOffer>();
            }

            public GetOneFreeOffer GetGetOneFreeOfferFor(char sku)
            {
                throw new System.NotImplementedException();
            }

            public int GetIndividualPriceFor(char sku)
            {
                if (individualPrices.ContainsKey(sku))
                {
                    return individualPrices[sku];
                }
                throw new SkuInvalidException(sku);
            }

            public MultiPrice GetMultiPriceOfferFor(char sku)
            {
                if (multiPrices.ContainsKey(sku))
                {
                    return multiPrices[sku];
                }
                if (individualPrices.ContainsKey(sku))
                {
                    // It's a valid sku, but there is no offer
                    return null;
                }
                throw new SkuInvalidException(sku);
            }
        }
    }
}
