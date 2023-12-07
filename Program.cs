using Stripe;

class Program
{
    static void Main(string[] args)
    {
        StripeConfiguration.ApiKey = args[4];
       
        var options = new ProductCreateOptions
        {
            Description = args[1],
            Name = args[0],
            Active = true,
            Shippable = false,
            StatementDescriptor = args[1],
            TaxCode = "txcd_10000000",
            DefaultPriceData = new ProductDefaultPriceDataOptions
            {
                Currency = args[2],
                TaxBehavior = "inclusive",
                UnitAmount = long.Parse(args[3]),
            },
        };
        var service = new ProductService();
        service.Create(options);
    }
}
