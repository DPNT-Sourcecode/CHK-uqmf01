using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public class Basket
        {
            private Basket(Dictionary<char, int> itemsLeftToCost, int priceSoFar)
            {
                ItemsLeftToCostDictionary = itemsLeftToCost;
                PriceSoFar = priceSoFar;
            }

            public Basket(string itemsLeftToCost, int priceSoFar)
            {
                ItemsLeftToCostDictionary = itemsLeftToCost.GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count()); ;
                PriceSoFar = priceSoFar;
            }

            private Dictionary<char, int> ItemsLeftToCostDictionary { get; }
            public int PriceSoFar { get; }

            public string ItemsLeftToCost()
            {
                var builder = new StringBuilder();
                foreach(var kvp in ItemsLeftToCostDictionary)
                {
                    for(var i = 0; i < kvp.Value; i++)
                    {
                        builder.Append(kvp.Key);
                    }
                }
                return builder.ToString();
            }

            internal Basket Processed(IList<char> costedItems, int atPrice)
            {
                var newItemsLeftToCost = ItemsLeftToCostDictionary
                    .ToDictionary(x => x.Key, x => x.Value - costedItems.Count(c => c == x.Value));
                return new Basket(newItemsLeftToCost, PriceSoFar + atPrice);
            }
        }

        private int CalculatePriceInternal(string skus)
        {
            var basket = new Basket(skus, priceSoFar: 0);

            var groupOffers = priceDatabase.GetGroupOffers();
            if (groupOffers != null)
            {
                foreach (var groupOffer in groupOffers)
                {
                    basket = ApplyGroupOffer(basket, groupOffer);
                }
            }
            var items = CreateItemsCountDictionary(basket.ItemsLeftToCost());
            var total = basket.PriceSoFar;
            var potentialFreeItems = CalulatePotentialFreeItemCounts(items);
            foreach (var skuAndQuantity in items)
            {
                var sku = skuAndQuantity.Key;
                var quantity = skuAndQuantity.Value;
                var freeItems = potentialFreeItems.ContainsKey(sku) ? potentialFreeItems[sku] : 0;
                // Note: We are asuming here that it's always better to give the free items,
                // this might not be best for the customer (eg in situations where it is cheaper to have
                // an extra item in the basket)
                // Although this will work for all the offers we currently support
                total += CalculatePriceFor(sku, quantity - freeItems);
            }
            return total;
        }

        private Basket ApplyGroupOffer(Basket basket, GroupOffer groupOffer)
        {
            var relaventItems = basket.ItemsLeftToCost()
                .Where(x => groupOffer.AppliesTo(x))
                .ToList();
            if(relaventItems.Count < groupOffer.Quantity)
            {
                // Cannot apply offer
                return basket;
            }
            var itemsToApplyTo = relaventItems
                .OrderByDescending(x => priceDatabase.GetIndividualPriceFor(x))
                .Take(groupOffer.Quantity)
                .ToList();
            var normalPrice = itemsToApplyTo.Sum(x => priceDatabase.GetIndividualPriceFor(x));
            if(normalPrice <= groupOffer.Price)
            {
                // No point applying offer
                return basket;
            }
            var leftOverBasket = basket.Processed(itemsToApplyTo, atPrice: groupOffer.Price);
            return leftOverBasket;
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
            var multiPriceOffers = priceDatabase.GetMultiPriceOfferFor(sku) ?? new MultiPrice[0]; ;
            var individualPrice = priceDatabase.GetIndividualPriceFor(sku);
            var normalPrice = individualPrice * quantity;
            if (!multiPriceOffers.Any())
            {
                return normalPrice;
            }
            var offerPrice = CalculateMultiPriceFor(multiPriceOffers, individualPrice, quantity);
            return Math.Min(normalPrice, offerPrice);
        }

        private int CalculateMultiPriceFor(IList<MultiPrice> multiPriceOffers, int individualPrice, int quantity)
        {
            // This may not give the customer the best price, if the offers are not well balanced
            var offersOrderedByAveragePrice = multiPriceOffers
                .OrderBy(x => decimal.Divide(x.Price, x.Quantity));
            var priceSoFar = 0;
            var quantityLeft = quantity;
            foreach (var offer in offersOrderedByAveragePrice)
            {
                var numberOfTimesToApplyOffer = quantityLeft / offer.Quantity;
                quantityLeft = quantityLeft % offer.Quantity;
                priceSoFar += numberOfTimesToApplyOffer * offer.Price;
            }
            return priceSoFar + quantityLeft * individualPrice;
        }

        private Dictionary<char, int> CreateItemsCountDictionary(string skus)
        {
            return skus.GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
        }
    }
}
