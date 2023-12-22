using System;
using Newtonsoft.Json;

namespace SSLCommerz.NetCore.SSLCommerz;

public class SSLTransactionQueryResponse
{
    [JsonProperty("APIConnect")]
    public string ApiConnect { get; set; }

    [JsonProperty("no_of_trans_found")]
    public int NumberOfTransactionsFound { get; set; }

    [JsonProperty("element")]
    public SSLCommerzTransactionElement[] Elements { get; set; }
}

public class SSLCommerzTransactionElement
{
    [JsonProperty("val_id")]
    public string ValId { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("validated_on")]
    public DateTime ValidatedOn { get; set; }

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

    [JsonProperty("discount_percentage")]
    public decimal? DiscountPercentage { get; set; }

    [JsonProperty("discount_remarks")]
    public string DiscountRemarks { get; set; }

    [JsonProperty("discount_amount")]
    public decimal? DiscountAmount { get; set; }

    [JsonProperty("tran_date")]
    public DateTime TranDate { get; set; }

    [JsonProperty("tran_id")]
    public Guid TranId { get; set; }

    [JsonProperty("amount")]
    public decimal? Amount { get; set; }

    [JsonProperty("store_amount")]
    public decimal? StoreAmount { get; set; }

    [JsonProperty("bank_tran_id")]
    public string BankTransactionId { get; set; }

    [JsonProperty("card_type")]
    public string CardType { get; set; }

    [JsonProperty("risk_title")]
    public string RiskTitle { get; set; }

    [JsonProperty("risk_level")]
    public int? RiskLevel { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("bank_gw")]
    public string BankGateway { get; set; }

    [JsonProperty("card_no")]
    public string CardNumber { get; set; }

    [JsonProperty("card_issuer")]
    public string CardIssuer { get; set; }

    [JsonProperty("card_brand")]
    public string CardBrand { get; set; }

    [JsonProperty("card_issuer_country")]
    public string CardIssuerCountry { get; set; }

    [JsonProperty("card_issuer_country_code")]
    public string CardIssuerCountryCode { get; set; }

    [JsonProperty("gw_version")]
    public string GatewayVersion { get; set; }

    [JsonProperty("emi_instalment")]
    public int? EmiInstalment { get; set; }

    [JsonProperty("emi_amount")]
    public decimal? EmiAmount { get; set; }

    [JsonProperty("emi_description")]
    public string EmiDescription { get; set; }

    [JsonProperty("emi_issuer")]
    public string EmiIssuer { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }
}
