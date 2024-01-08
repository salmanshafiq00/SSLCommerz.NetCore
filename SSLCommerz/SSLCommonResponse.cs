using Newtonsoft.Json;

namespace SSLCommerz.NetCore.SSLCommerz;

public abstract class SSLCommonResponse
{
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("base_fair")]
    public decimal? BaseFair { get; set; }

    [JsonProperty("card_brand")]
    public string CardBrand { get; set; } = string.Empty;

    [JsonProperty("card_category")]
    public string CardCategory { get; set; } = string.Empty;

    [JsonProperty("card_issuer")]
    public string CardIssuer { get; set; } = string.Empty;

    [JsonProperty("card_issuer_country")]
    public string CardIssuerCountry { get; set; } = string.Empty;

    [JsonProperty("card_issuer_country_code")]
    public string CardIssuerCountryCode { get; set; } = string.Empty;

    [JsonProperty("card_no")]
    public string CardNumber { get; set; } = string.Empty;

    [JsonProperty("card_sub_brand")]
    public string CardSubBrand { get; set; } = string.Empty;

    [JsonProperty("card_type")]
    public string CardType { get; set; } = string.Empty;

    [JsonProperty("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonProperty("currency_amount")]
    public decimal? CurrencyAmount { get; set; }

    [JsonProperty("currency_rate")]
    public decimal? CurrencyRate { get; set; }

    [JsonProperty("currency_type")]
    public string CurrencyType { get; set; } = string.Empty;

    [JsonProperty("discount_amount")]
    public decimal? DiscountAmount { get; set; }

    [JsonProperty("discount_percentage")]
    public decimal? DiscountPercentage { get; set; }

    [JsonProperty("discount_remarks")]
    public string DiscountRemarks { get; set; } = string.Empty;

    [JsonProperty("risk_level")]
    public int? RiskLevel { get; set; }

    [JsonProperty("risk_title")]
    public string RiskTitle { get; set; } = string.Empty;

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    [JsonProperty("store_amount")]
    public decimal StoreAmount { get; set; }

    [JsonProperty("tran_date")]
    public string TranxDate { get; set; } = string.Empty;

    [JsonProperty("tran_id")]
    public string TranxId { get; set; } = string.Empty;

    [JsonProperty("val_id")]
    public string ValidationId { get; set; } = string.Empty;

    [JsonProperty("value_a")]
    public string ValueA { get; set; } = string.Empty;

    [JsonProperty("value_b")]
    public string ValueB { get; set; } = string.Empty;

    [JsonProperty("value_c")]
    public string ValueC { get; set; } = string.Empty;

    [JsonProperty("value_d")]
    public string ValueD { get; set; } = string.Empty;
}
