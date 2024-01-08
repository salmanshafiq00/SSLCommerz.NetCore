namespace SSLCommerz.NetCore.OptionsSetup;

public class SSLCommerzOptions
{
    public const string SSLCommerz = nameof(SSLCommerz);
    public required string StoreId { get; set; }
    public required string StorePass { get; set; }
    public required bool IsDevelopment { get; set; } = true;
    public required bool IsSandbox { get; set; } = true;
    public required string SSLCommerzUrl { get; set; }
    public required string SSLCommerzTestUrl { get; set; }
    public required string SubmitUrl { get; set; }
    public required string ValidationUrl { get; set; }
    public required string CheckingUrl { get; set; }
    public required string SuccessUrl { get; set; }
    public required string FailUrl { get; set; }
    public required string CancelUrl { get; set; }
    public required string AppBaseUrl { get; set; }
    public required string AppDevBaseUrl { get; set; }
    public string IPNUrl { get; set; }
}
