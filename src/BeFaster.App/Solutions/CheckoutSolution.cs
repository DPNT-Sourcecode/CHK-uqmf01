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
            private readonly Dictionary<char, List<MultiPrice>> multiPrices;
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
                    { 'F', 10 },
                    { 'G', 20 },
                    { 'H', 10 },
                    { 'I', 35 },
                    { 'J', 60 },
                    { 'K', 70 },
                    { 'L', 90 },
                    { 'M', 15 },
                    { 'N', 40 },
                    { 'O', 10 },
                    { 'P', 50 },
                    { 'Q', 30 },
                    { 'R', 50 },
                    { 'S', 20 },
                    { 'T', 20 },
                    { 'U', 40 },
                    { 'V', 50 },
                    { 'W', 20 },
                    { 'X', 17 },
                    { 'Y', 20 },
                    { 'Z', 21 },
                };

                multiPrices = new Dictionary<char, List<MultiPrice>>
                {
                    {'A', new List<MultiPrice>
                    {
                        new MultiPrice(quantity: 3, price: 130),
                        new MultiPrice(quantity: 5, price: 200),
                    }},
                    {'B', new List<MultiPrice>{new MultiPrice(quantity: 2, price: 45) } },
                    {'F', new List<MultiPrice>{new MultiPrice(quantity: 3, price: 20) } },// Buy 2 get 1 free
                    {'H', new List<MultiPrice>
                    {
                        new MultiPrice(quantity: 5, price: 45),
                        new MultiPrice(quantity: 10, price: 80),
                    }},
                    {'K', new List<MultiPrice>{new MultiPrice(quantity: 2, price: 120) } },
                    {'P', new List<MultiPrice>{new MultiPrice(quantity: 5, price: 200) } },
                    {'Q', new List<MultiPrice>{new MultiPrice(quantity: 3, price: 80) } },
                    {'U', new List<MultiPrice>{new MultiPrice(quantity: 4, price: 120) } },// Buy 3 get 1 free
                    {'V', new List<MultiPrice>
                    {
                        new MultiPrice(quantity: 2, price: 90),
                        new MultiPrice(quantity: 3, price: 130),
                    }},
                };

                getOneFreeOffers = new Dictionary<char, GetOneFreeOffer>
                {
                    {'E', new GetOneFreeOffer(quantity: 2, freeSku: 'B') },
                    {'N', new GetOneFreeOffer(quantity: 3, freeSku: 'M') },
                    {'R', new GetOneFreeOffer(quantity: 3, freeSku: 'Q') },
                };
            }

            public int GetIndividualPriceFor(char sku)
            {
                AssertSkuExists(sku);
                return individualPrices[sku];
            }

            public IList<MultiPrice> GetMultiPriceOfferFor(char sku)
            {
                AssertSkuExists(sku);
                return multiPrices.ContainsKey(sku) ? multiPrices[sku] : new List<MultiPrice>(0);
            }

            public GetOneFreeOffer GetGetOneFreeOfferFor(char sku)
            {
                AssertSkuExists(sku);
                return getOneFreeOffers.ContainsKey(sku) ? getOneFreeOffers[sku] : null;
            }

            public IList<GroupOffer> GetGroupOffers()
            {
                return new GroupOffer[] { new GroupOffer(skus: "STXYZ", quantity: 3, price: 45) };
            }

            private void AssertSkuExists(char sku)
            {
                if (!individualPrices.ContainsKey(sku))
                {
                    throw new SkuInvalidException(sku);
                }
            }
        }
    }
}
