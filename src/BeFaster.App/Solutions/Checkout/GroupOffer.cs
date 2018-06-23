using System;
using System.Collections.Generic;

namespace BeFaster.App.Solutions.Checkout
{
    public class GroupOffer
    {
        public GroupOffer(string skus, int quantity, int price)
        {
            Skus = new HashSet<char>(skus);
            Quantity = quantity;
            Price = price;
        }

        public HashSet<char> Skus { get; }
        public int Quantity { get; }
        public int Price { get; }

        public bool AppliesTo(char x)
        {
            return Skus.Contains(x);
        }
    }
}