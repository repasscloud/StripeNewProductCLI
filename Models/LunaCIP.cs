using System.Text.Json.Serialization;

namespace LunaVpnApi.Models.CIP.LunaVpnCustom
{
    public class LunaCIP
    {
        [JsonPropertyName("application")]
        public string App { get; set; } = null!;

        [JsonPropertyName("region")]
        public string Region { get; set; } = null!;

        [JsonPropertyName("country")]
        public string Country { get; set; } = null!;

        [JsonPropertyName("city")]
        public string City { get; set; } = null!;

        [JsonPropertyName("cip")]
        public string CIP { get; set; } = null!;

        [JsonPropertyName("flag_code")]
        public string FlagCode { get; set; } = null!;

        [JsonPropertyName("api_payload")]
        public string ApiPayload { get; set; } = null!;
    }
}

