using Newtonsoft.Json;

namespace SSLCommerz.NetCore.SSLCommerz;

public class SSLCommerzValidatorResponse : SSLCommonResponse
{

    [JsonProperty("bank_tran_id")]
    public string BankTranxId { get; set; }

    [JsonProperty("emi_instalment")]
    public int EMIInstalment { get; set; }

    [JsonProperty("emi_amount")]
    public decimal? EMIAmount { get; set; }

    [JsonProperty("emi_description")]
    public string EMIDescription { get; set; }

    [JsonProperty("emi_issuer")]
    public string EMIIssuer { get; set; }

    [JsonProperty("account_details")]
    public string AccountDetails { get; set; }

    [JsonProperty("APIConnect")]
    public string APIConnect { get; set; }

    [JsonProperty("validated_on")]
    public DateTime ValidatedOn { get; set; }

    [JsonProperty("gw_version")]
    public string GatewayVersion { get; set; }

    [JsonProperty("offer_avail")]
    public int OfferAvailable { get; set; }

    [JsonProperty("card_ref_id")]
    public string CardReferenceId { get; set; }

    [JsonProperty("isTokeizeSuccess")]
    public int IsTokenizeSuccess { get; set; }

    [JsonProperty("campaign_code")]
    public string CampaignCode { get; set; }
}
