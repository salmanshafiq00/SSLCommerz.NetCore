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

public class SSLCommerzTransactionElement : SSLCommonResponse
{
    [JsonProperty("validated_on")]
    public DateTime? ValidatedOn { get; set; }

    [JsonProperty("bank_tran_id")]
    public string BankTranxId { get; set; } = string.Empty;

    [JsonProperty("bank_gw")]
    public string BankGateway { get; set; } = string.Empty;

    [JsonProperty("gw_version")]
    public string GatewayVersion { get; set; } = string.Empty;

    [JsonProperty("emi_instalment")]
    public int EmiInstalment { get; set; }

    [JsonProperty("emi_amount")]
    public decimal? EmiAmount { get; set; }

    [JsonProperty("emi_description")]
    public string EmiDescription { get; set; } = string.Empty;

    [JsonProperty("emi_issuer")]
    public string EmiIssuer { get; set; } = string.Empty;

    [JsonProperty("error")]
    public string Error { get; set; } = string.Empty;
}
