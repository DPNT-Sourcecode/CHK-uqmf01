﻿namespace BeFaster.App.Solutions.Checkout
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
    }
}
