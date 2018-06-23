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
            var potentialFreeItems = CalulatePotentialFreeItemCounts(items);
            foreach (var skuAndQuantity in items)
            {
                var sku = skuAndQuantity.Key;
                var quantity = skuAndQuantity.Value;
                var freeItems = potentialFreeItems?[sku] ?? 0;
                // Note: We are asuming here that it's always better to give the free items, this might not be best
                total += CalculatePriceFor(sku, quantity - freeItems);
            }
            return total;
        }

        private Dictionary<char, int> CalulatePotentialFreeItemCounts(Dictionary<char, int> items)
        {
            var potentialFreeCounts = new Dictionary<char, int>();
            foreach (var skuAndQuantity in items)
            {
                var sku = skuAndQuantity.Key;
                var quantity = skuAndQuantity.Value;
                var getFreeItemOffer = priceDatabase.GetGetOneFreeOfferFor(sku);
                if(getFreeItemOffer != null)
                {
                    var canBeAppliedCount = quantity / getFreeItemOffer.Quantity;
                    if (potentialFreeCounts.ContainsKey(getFreeItemOffer.FreeSku))
                    {
                        potentialFreeCounts[getFreeItemOffer.FreeSku] += canBeAppliedCount;
                    }
                    else
                    {
                        potentialFreeCounts[getFreeItemOffer.FreeSku] = canBeAppliedCount;
                    }
                }
            }
            return potentialFreeCounts;
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