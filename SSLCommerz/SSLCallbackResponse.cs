using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SSLCommerz.NetCore.SSLCommerz;

public class SSLCallbackResponse : SSLCommonResponse
{

    [JsonProperty("subscription_id")]
    public string SubscriptionId { get; set; } = string.Empty;

}

