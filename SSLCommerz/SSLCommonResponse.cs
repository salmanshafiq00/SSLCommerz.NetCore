using Newtonsoft.Json;

namespace SSLCommerz.NetCore.SSLCommerz;

public abstract class SSLCommonResponse
{
    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("tran_date")]
    public string TranxDate { get; set; }

    [JsonProperty("tran_id")]
    public string TranxId { get; set; }

    [JsonProperty("val_id")]
    public string ValidationId { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("store_amount")]
    public decimal StoreAmount { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }
    [JsonProperty("card_type")]
    public string CardType { get; set; }

    [JsonProperty("card_no")]
    public string CardNumber { get; set; }

    [JsonProperty("card_issuer")]
    public string CardIssuer { get; set; }

    [JsonProperty("card_brand")]
    public string CardBrand { get; set; }

    [JsonProperty("card_category")]
    public string CardCategory { get; set; }

    [JsonProperty("card_sub_brand")]
    public string CardSubBrand { get; set; }

    [JsonProperty("card_issuer_country")]
    public string CardIssuerCountry { get; set; }

    [JsonProperty("card_issuer_country_code")]
    public string CardIssuerCountryCode { get; set; }

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
    [JsonProperty("risk_title")]
    public string RiskTitle { get; set; }

    [JsonProperty("risk_level")]
    public int? RiskLevel { get; set; }

    [JsonProperty("discount_percentage")]
    public decimal? DiscountPercentage { get; set; }

    [JsonProperty("discount_amount")]
    public decimal? DiscountAmount { get; set; }

    [JsonProperty("discount_remarks")]
    public string DiscountRemarks { get; set; }


// additional than valresponse

    [JsonProperty("error")]
    public string? Error { get; set; } 

    [JsonProperty("store_id")]
    public string StoreId { get; set; } 

    [JsonProperty("verify_sign")]
    public string VerifySign { get; set; } 

    [JsonProperty("verify_key")]
    public string VerifyKey { get; set; } 

    [JsonProperty("verify_sign_sha2")]
    public string VerifySignSha2 { get; set; } 




}
