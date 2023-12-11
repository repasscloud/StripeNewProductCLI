using System.ComponentModel.DataAnnotations;

namespace StripeNewProdCLI.Models;

public class StripeProd
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? StatementDescriptor { get; set; }
    public string? TaxCode { get; set; } = "txcd_10000000";
    [StringLength(3, MinimumLength = 3, ErrorMessage = "CurrencyCode must be exactly 3 characters.")]
    public string? CurrencyCode { get; set; }
    public string UnitAmount { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string? ExpiryDays { get; set; }
}

