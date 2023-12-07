using Stripe;

class Program
{
    static void Main(string[] args)
    {
        StripeConfiguration.ApiKey = args[0];
        
        if (args[1].ToLower() == "--create-links")
        {
            string currency = args[2].ToLower();

            var options = new PriceListOptions { Currency = currency, Active = true };
            var service = new PriceService();
            StripeList<Price> prices = service.List(options);

            foreach (var p in prices)
            {
                var paymentLinkOptions = new PaymentLinkCreateOptions
                {
                    Currency = currency,
                    AllowPromotionCodes = true,
                    BillingAddressCollection = "auto",
                    AutomaticTax = new PaymentLinkAutomaticTaxOptions { Enabled = false },
                    InvoiceCreation = new PaymentLinkInvoiceCreationOptions { Enabled = true },
                    PaymentMethodTypes = new List<string> { "card" },
                    PhoneNumberCollection = new PaymentLinkPhoneNumberCollectionOptions
                    {
                        Enabled = false,
                    },
                    LineItems = new List<PaymentLinkLineItemOptions>
                    {
                        new PaymentLinkLineItemOptions
                        {
                            AdjustableQuantity = new PaymentLinkLineItemAdjustableQuantityOptions
                            {
                                Enabled = false,
                            },
                            Price = p.Id.ToString(),
                            Quantity = 1,
                        },
                    },
                    AfterCompletion = new PaymentLinkAfterCompletionOptions
                    {
                        Type = "hosted_confirmation",
                    },
                };
                var paymentLinkService= new PaymentLinkService();
                paymentLinkService.Create(paymentLinkOptions);
            }
        }
        else
        {
            var options = new ProductCreateOptions
            {
                Description = args[2],
                Name = args[1],
                Active = true,
                Shippable = false,
                StatementDescriptor = args[2],
                TaxCode = "txcd_10000000",
                DefaultPriceData = new ProductDefaultPriceDataOptions
                {
                    Currency = args[3],
                    TaxBehavior = "inclusive",
                    UnitAmount = long.Parse(args[4]),
                },
                Images = new List<string>
                {
                    "https://raw.githubusercontent.com/repasscloud/StripeNewProductCLI/main/27314.jpg",
                },
            };
            var service = new ProductService();
            service.Create(options);
        }

        
    }
}
