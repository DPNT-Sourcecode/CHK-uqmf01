using System;

namespace BeFaster.App.Solutions.Checkout
{
    public class SkuInvalidException : Exception
    {
        public SkuInvalidException(char sku) 
            : base($"The SKU '{sku}' is invalid") { }
    }
}
