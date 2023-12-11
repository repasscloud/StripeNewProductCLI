using Stripe;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using LunaVpnApi.Models.StripeCustom;
using System.Text.Json;
using System.Text;

namespace StripeNewProdCLI;
class Program
{
    private static readonly HttpClient httpClient = new HttpClient();

    static async Task Main(string[] args)
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
        else if (args[1].ToLower() == "--csv")
        {
            string csvFilePath = args[2];

            var cultureInfo = CultureInfo.InvariantCulture;

            if (!System.IO.File.Exists(csvFilePath))
            {
                Console.WriteLine($"File {csvFilePath} not exists");
                Environment.Exit(1);
            }    
            else
            {
                Console.WriteLine($"Using csv: {csvFilePath}");
            }
                
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(cultureInfo)
            {
                HasHeaderRecord = true // to indicate that the CSV file has a header
            }))
            {
                var records = csv.GetRecords<Models.StripeProd>();

                foreach (Models.StripeProd stProd in records)
                {
                    var options = new ProductCreateOptions
                    {
                        Description = string.IsNullOrEmpty(stProd.Description) ? stProd.Name : stProd.Description,
                        Name = stProd.Name,
                        Active = true,
                        Shippable = false,
                        StatementDescriptor = string.IsNullOrEmpty(stProd.Description) ? stProd.Name : stProd.Description,
                        TaxCode = stProd.TaxCode,
                        DefaultPriceData = new ProductDefaultPriceDataOptions
                        {
                            Currency = stProd.CurrencyCode,
                            TaxBehavior = "inclusive",
                            UnitAmount = long.Parse(stProd.UnitAmount),
                        },
                        Images = new List<string>
                        {
                            stProd.ImageUrl,
                        },
                    };

                    var service = new ProductService();
                    var stripeResponse = service.Create(options);
                    
                    var dataToSend = new StripeLunaProduct
                    {
                        ProductId = stripeResponse.Id,
                        ExpirationDays = string.IsNullOrEmpty(stProd.ExpiryDays) ? 0 : int.Parse(stProd.ExpiryDays),
                    };

                    string jsonData = JsonSerializer.Serialize(dataToSend);
                    string apiUrl = args[3];
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response: " + responseBody);
                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
            }
        }
        else if (args.Length == 5)
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
        else
        {
            // Handle the case where too many arguments are provided
            Console.WriteLine("FAIL");
            Console.WriteLine("Usage:");
            Console.WriteLine("  >> app [api_key] --create-links [currency]");
            Console.WriteLine("  >> app [api_key] --csv [csv_file_path] [api_endpoint]");
            Console.WriteLine("  >> app [api_key] [product_name] [description] [currency_code] [amount]\n\n");
            Console.WriteLine("The best way to use it, is with the CSV file");
        }
    }
}
