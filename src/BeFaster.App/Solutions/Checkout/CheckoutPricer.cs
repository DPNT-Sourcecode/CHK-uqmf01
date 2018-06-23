using System;
using System.Collections.Generic;
using System.Linq;

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
                var total = 0;
                var items = CreateItemsCountDictionary(skus);
                foreach(var skuAndQuantity in items)
                {
                    var sku = skuAndQuantity.Key;
                    var quantity = skuAndQuantity.Value;
                    total += CalculatePriceFor(sku, quantity);
                }
            }
            catch (SkuInvalidException)
            {
                // todo Log what happened here
                return -1;
            }
        }

        private int CalculatePriceFor(char sku, int quantity)
        {
            var multiPrice = priceDatabase.GetMultiPriceOfferFor(sku);
            var individualPrice = priceDatabase.GetIndividualPriceFor(sku);
            if(multiPrice == null)
            {
                return individualPrice * quantity;
            }
            return CalculatePrice
        }

        private Dictionary<char, int> CreateItemsCountDictionary(string skus)
        {
            return skus.GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
        }
    }
}
