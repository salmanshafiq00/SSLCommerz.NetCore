namespace SSLCommerz.NetCore.SSLCommerz;

public class SSLInitialResponse
{
    public string Status { get; set; }
    public string FailedReason { get; set; }
    public string SessionKey { get; set; }
    public GatewayInfo Gw { get; set; }
    public string RedirectGatewayURL { get; set; }
    public string DirectPaymentURLBank { get; set; }
    public string DirectPaymentURLCard { get; set; }
    public string DirectPaymentURL { get; set; }
    public string RedirectGatewayURLFailed { get; set; }
    public string GatewayPageURL { get; set; }
    public string StoreBanner { get; set; }
    public string StoreLogo { get; set; }
    public string StoreName { get; set; }
    public List<Description> Desc { get; set; }
    public string IsDirectPayEnable { get; set; }
}
