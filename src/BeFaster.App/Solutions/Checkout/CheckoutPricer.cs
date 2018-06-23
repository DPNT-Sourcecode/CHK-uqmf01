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
                return CalculatePriceInternal(skus);
            }
            catch (SkuInvalidException)
            {
                // todo Log what happened here
                return -1;
            }
        }

        private int CalculatePriceInternal(string skus)
        {
            var total = 0;
            var items = CreateItemsCountDictionary(skus);
            foreach (var skuAndQuantity in items)
            {
                var sku = skuAndQuantity.Key;
                var quantity = skuAndQuantity.Value;
                total += CalculatePriceFor(sku, quantity);
            }
            return total;
        }

        private int CalculatePriceFor(char sku, int quantity)
        {
            var multiPrice = priceDatabase.GetMultiPriceOfferFor(sku);
            var individualPrice = priceDatabase.GetIndividualPriceFor(sku);
            var normalPrice = individualPrice * quantity;
            if (multiPrice == null)
            {
                return normalPrice;
            }
            var offerPrice = CalculateMultiPriceFor(multiPrice, individualPrice, quantity);
            return Math.Min(normalPrice, offerPrice);
        }

        private int CalculateMultiPriceFor(MultiPrice multiPrice, int individualPrice, int quantity)
        {
            var numberOfMultiGroups = quantity / multiPrice.Quantity;
            var numberLeftOver = quantity % multiPrice.Quantity;
            var offerPrice = numberOfMultiGroups * multiPrice.Price
                + numberLeftOver * individualPrice;
            return offerPrice;
        }

        private Dictionary<char, int> CreateItemsCountDictionary(string skus)
        {
            return skus.GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
        }
    }
}
