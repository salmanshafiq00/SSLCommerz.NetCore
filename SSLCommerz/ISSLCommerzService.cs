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

    /// <summary>
    /// Validate transaction with Transaction ID
    /// </summary>
    /// <param name="tranxId"></param>
    /// <param name="tranxAmount"></param>
    /// <param name="tranxCurrency"></param>
    /// <param name="cancellationToken"></param>
    /// <returns> tuple -> (bool status, string? message)</returns>
    Task<(bool status, string? message)> ValidatePaymentAsync(string tranxId, decimal tranxAmount, string tranxCurrency, SSLCallbackResponse callbackResponse, CancellationToken cancellationToken);

    /// <summary>
    /// Get transaction detail by Transaction ID
    /// </summary>
    /// <param name="tranxId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>SSLTransactionQueryResponse</returns>
    Task<SSLTransactionQueryResponse> GetTransactionDetail(string tranxId, CancellationToken cancellationToken);
}
