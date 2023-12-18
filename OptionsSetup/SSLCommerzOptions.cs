namespace SSLCommerz.NetCore.OptionsSetup;

public class SSLCommerzOptions
{
    public const string SSLCommerz = nameof(SSLCommerz);
    public required string StoreId { get; set; }
    public required string StorePass { get; set; }
    public required bool IsLive { get; set; } = false;
    public required string SSLCommerzLiveUrl { get; set; }
    public required string SSLCommerzTestUrl { get; set; }
    public required string SubmitUrl { get; set; }
    public required string ValidationUrl { get; set; }
    public required string CheckingUrl { get; set; }
    public required string SuccessUrl { get; set; }
    public required string FailUrl { get; set; }
    public required string CancelUrl { get; set; }
    public string IPNUrl { get; set; }
}
