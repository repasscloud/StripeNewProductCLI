using System.Text.Json.Serialization;

namespace LunaVpnApi.Models.StripeCustom
{
	public class StripeLunaProduct
	{
        public Guid StripeLunaProductId { get; set; } = Guid.NewGuid();

        [JsonPropertyName("product_id")]  //productId in stripe
        public string? ProductId { get; set; }

        [JsonPropertyName("expiration_days")]
        public int ExpirationDays { get; set; }
    }
}

