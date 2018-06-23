using System;
using System.Collections.Generic;

namespace BeFaster.App.Solutions.Checkout
{
    public class CheckoutPricer : ICheckoutPricer
    {
        private readonly IPriceDatabase priceDatabase;

        public CheckoutPricer(IPriceDatabase priceDatabase)
        {
            this.priceDatabase = priceDatabase;
        }

        public int CalculatePrice(string skus)
        {
            try
            {
                var items = CreateItemsCountDictionary(skus);
                var total = 0;
                foreach (var sku in skus)
                {
                    total += priceDatabase.GetIndividualPriceFor(sku);
                }
                return total;
            }
            catch (SkuInvalidException)
            {
                // todo Log what happened here
                return -1;
            }
        }

        private Dictionary<char, int> CreateItemsCountDictionary(string skus)
        {
            var itemCounts = new Dictionary<char, int>();
            foreach(var sku in skus)
            {
                if (itemCounts.ContainsKey(sku))
                {
                    itemCounts[sku]++;
                }
                else
                {
                    itemCounts[sku] = 1;
                }
            }
            return itemCounts;
        }
    }
}
