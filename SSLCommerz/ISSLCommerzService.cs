namespace SSLCommerz.NetCore.SSLCommerz;

public interface ISSLCommerzService
{
    /// <summary>
    /// Initialize with payment gateway
    /// </summary>
    /// <param name="reqData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>GatewayPageURL</returns>
    Task<string> InitializeTransectionAsync(SSLInitialRequest reqData, CancellationToken cancellationToken);
    Task<(bool status, string? message)> ValidatePaymentAsync(string tranxId, decimal tranxAmount, string tranxCurrency, HttpRequest httpRequest, CancellationToken cancellationToken);
}
