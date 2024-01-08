using Newtonsoft.Json;

namespace SSLCommerz.NetCore.SSLCommerz;

public class SSLCallbackResponse : SSLCommonResponse
{
    [JsonProperty("bank_tran_id")]
    public string BankTranxId { get; set; } = string.Empty;

    [JsonProperty("error")]
    public string Error { get; set; } = string.Empty;

    [JsonProperty("store_id")]
    public string StoreId { get; set; } = string.Empty;

    [JsonProperty("subscription_id")]
    public string SubscriptionId { get; set; } = string.Empty;

    [JsonProperty("verify_key")]
    public string VerifyKey { get; set; } = string.Empty;

    [JsonProperty("verify_sign")]
    public string VerifySign { get; set; } = string.Empty;

    [JsonProperty("verify_sign_sha2")]
    public string VerifySignSha2 { get; set; } = string.Empty;
}

