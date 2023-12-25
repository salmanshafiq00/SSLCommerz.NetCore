using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SSLCommerz.NetCore.Utilities;

namespace SSLCommerz.NetCore.SSLCommerz;

public class SSLCallbackResponse
{

    [JsonProperty("tran_id")]
    public string TranxId { get; set; }

    [JsonProperty("val_id")]
    public string? ValidationId { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("card_type")]
    public string CardType { get; set; }

    [JsonProperty("store_amount")]
    public decimal StoreAmount { get; set; }

    [JsonProperty("card_no")]
    public string CardNumber { get; set; }

    [JsonProperty("bank_tran_id")]
    public string BankTranxId { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("tran_date")]
    //[JsonConverter(typeof(CustomDateTimeConverter))]
    public string TranxDate { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("card_issuer")]
    public string CardIssuer { get; set; }

    [JsonProperty("card_brand")]
    public string CardBrand { get; set; }

    [JsonProperty("card_sub_brand")]
    public string CardSubBrand { get; set; }

    [JsonProperty("card_issuer_country")]
    public string CardIssuerCountry { get; set; }

    [JsonProperty("card_issuer_country_code")]
    public string CardIssuerCountryCode { get; set; }

    [JsonProperty("store_id")]
    public string StoreId { get; set; }

    [JsonProperty("verify_sign")]
    public string VerifySign { get; set; }

    [JsonProperty("verify_key")]
    public string VerifyKey { get; set; }

    [JsonProperty("verify_sign_sha2")]
    public string VerifySignSha2 { get; set; }

    [JsonProperty("currency_type")]
    public string CurrencyType { get; set; }

    [JsonProperty("currency_amount")]
    public decimal? CurrencyAmount { get; set; }

    [JsonProperty("currency_rate")]
    public decimal? CurrencyRate { get; set; }

    [JsonProperty("base_fair")]
    public decimal? BaseFair { get; set; }

    [JsonProperty("value_a")]
    public string ValueA { get; set; }

    [JsonProperty("value_b")]
    public string ValueB { get; set; }

    [JsonProperty("value_c")]
    public string ValueC { get; set; }

    [JsonProperty("value_d")]
    public string ValueD { get; set; }

    [JsonProperty("subscription_id")]
    public string SubscriptionId { get; set; }

    [JsonProperty("risk_level")]
    public int RiskLevel { get; set; }

    [JsonProperty("risk_title")]
    public string? RiskTitle { get; set; }
}

