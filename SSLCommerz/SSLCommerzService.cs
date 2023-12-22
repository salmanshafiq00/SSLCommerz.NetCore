using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SSLCommerz.NetCore.OptionsSetup;
using System.Net;
using System.Text;

namespace SSLCommerz.NetCore.SSLCommerz;

public class SSLCommerzService(IOptionsSnapshot<SSLCommerzOptions> sslCommerz, IHttpClientFactory httpClientFactory) : ISSLCommerzService
{
    private readonly SSLCommerzOptions _sslCommerz = sslCommerz.Value;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public const string STORE_ID = "store_id";
    public const string STORE_PASSWD = "store_passwd";
    public const string SUCCESS_URL = "success_url";
    public const string FAIL_URL = "fail_url";
    public const string CANCEL_URL = "cancel_url";
    public const string IPN_URL = "ipn_url";
    public const string VAL_ID = "val_id";
    public const string VERIFY_SIGN = "verify_sign";
    public const string VERIFY_KEY = "verify_key";
    public const string TRANX_ID = "tran_id";


    public async Task<string> InitializeTransectionAsync(SSLInitialRequest reqData, CancellationToken cancellationToken)
    {
        try
        {
            var postData = GetKeyValuePairs(reqData);

            postData.Add(STORE_ID, _sslCommerz.StoreId);
            postData.Add(STORE_PASSWD, _sslCommerz.StorePass);
            postData.Add(SUCCESS_URL, $"{_sslCommerz.AppLiveBaseUrl}{_sslCommerz.SuccessUrl}");
            postData.Add(FAIL_URL, $"{_sslCommerz.AppLiveBaseUrl}{_sslCommerz.FailUrl}");
            postData.Add(CANCEL_URL, $"{_sslCommerz.AppLiveBaseUrl}{_sslCommerz.CancelUrl}");
            postData.Add(IPN_URL, $"{_sslCommerz.AppLiveBaseUrl}{_sslCommerz.IPNUrl}");

            var baseUrl = GetSSLCommerzUrl(_sslCommerz.IsLive);

            var submitUrl = baseUrl + _sslCommerz.SubmitUrl;

            var content = new FormUrlEncodedContent(postData);

            var client = _httpClientFactory.CreateClient("SSLCommerz");

            var response = await client.PostAsync(submitUrl, content, cancellationToken);

            //response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                ArgumentException.ThrowIfNullOrEmpty(responseContent, nameof(responseContent));

                var initResponse = JsonConvert.DeserializeObject<SSLInitialResponse>(responseContent);

                ArgumentNullException.ThrowIfNull(initResponse, nameof(initResponse));

                return initResponse?.Status == "SUCCESS"
                    ? initResponse?.GatewayPageURL
                    : throw new Exception("Unable to get data from SSLCommerz");
            }
            else
            {
                // Handle error scenarios here
                throw new HttpRequestException($"Unable to get data from SSLCommerz. Please contact your manager! {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<(bool status, string? message)> ValidatePaymentAsync(
          string tranxId
        , decimal tranxAmount
        , string tranxCurrency
        , HttpRequest httpRequest
        , CancellationToken cancellationToken)
    {
        bool hashVerified = VerifyIPNHash(httpRequest);

        if (!hashVerified)
        {
            return (false, "Unable to verify hash");
        }

        string encodedValID = WebUtility.UrlEncode(httpRequest.Form[VAL_ID]);
        string encodedStoreID = WebUtility.UrlEncode(_sslCommerz.StoreId);
        string encodedStorePassword = WebUtility.UrlEncode(_sslCommerz.StorePass);

        string baseUrl = GetSSLCommerzUrl(_sslCommerz.IsLive);

        string validateUrl = $"{baseUrl}{_sslCommerz.ValidationUrl}?{VAL_ID}={encodedValID}&{STORE_ID}={encodedStoreID}&{STORE_PASSWD}={encodedStorePassword}&v=1&format=json";

        var client = _httpClientFactory.CreateClient("SSLCommerz");

        var response = await client.GetAsync(validateUrl, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return (false, "Unable to get transection status");
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        var validatorResponse = JsonConvert
            .DeserializeObject<SSLCommerzValidatorResponse>(responseContent);

        if (validatorResponse is null || !GetValidationStatus(validatorResponse.Status))
        {
            return (false, "This transaction is either expired or fails");
        }

        if (!IsTranxIdAndAmountValid(validatorResponse, tranxId, tranxAmount, tranxCurrency))
        {
            return (false, "Amount not matching");
        }
        return (true, "Transaction Validated");
    }


    public async Task<SSLTransactionQueryResponse> GetTransactionDetail(string tranxId, CancellationToken cancellationToken)
    {
        var baseUrl = GetSSLCommerzUrl(_sslCommerz.IsLive);

        string encodedTranxID = WebUtility.UrlEncode(tranxId);
        string encodedStoreID = WebUtility.UrlEncode(_sslCommerz.StoreId);
        string encodedStorePassword = WebUtility.UrlEncode(_sslCommerz.StorePass);

        var checkingUrl = $"{baseUrl}{_sslCommerz.CheckingUrl}?{TRANX_ID}={encodedTranxID}&{STORE_ID}={encodedStoreID}&{STORE_PASSWD}={encodedStorePassword}&v=1&format=json";

        // var content = new FormUrlEncodedContent(requestData);

        var client = _httpClientFactory.CreateClient("SSLCommerz");

        var response = await client.GetAsync(checkingUrl, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Unable to get detail of TanxID: {tranxId}");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var transectionDetail = JsonConvert
            .DeserializeObject<SSLTransactionQueryResponse>(content);

        ArgumentNullException.ThrowIfNull(transectionDetail, nameof(transectionDetail));

        return transectionDetail;
    }

    private bool VerifyIPNHash(HttpRequest httpRequest)
    {
        var verifySign = httpRequest.Form[VERIFY_SIGN].ToString();
        var verifyKey = httpRequest.Form[VERIFY_KEY].ToString();

        // Check For verify_sign and verify_key parameters
        if (!string.IsNullOrEmpty(verifySign) && !string.IsNullOrEmpty(verifyKey))
        {

            // Split key string by comma to make a list array
            var keyList = verifyKey.Split(',').ToList();

            // Initiate a key value pair list array
            var dataArray = keyList
                .Select(key => new KeyValuePair<string, string>(key, httpRequest.Form[key].ToString()))
                .ToList();

            // Store Hashed Password in list
            string hashedPass = MD5(_sslCommerz.StorePass);

            dataArray.Add(new KeyValuePair<string, string>(STORE_PASSWD, hashedPass));

            // Sort Array
            dataArray.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            // Concatenate and make String from array
            string hashString = dataArray
                .Aggregate(new StringBuilder(),
                    (sb, kv) => sb.Append($"{kv.Key}={kv.Value}&"), sb => sb.ToString())
                .TrimEnd('&');

            // Make hash by hashString and store
            string generatedHash = MD5(hashString);

            // Check if generated hash and verify_sign match or not
            if (generatedHash == verifySign)
            {
                return true; // Matched
            }
        }
        return false;
    }

    private static string MD5(string input)
    {
        using var md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        return BitConverter
            .ToString(hashBytes)
            .Replace("-", "")
            .ToLower();
    }

    private string GetSSLCommerzUrl(bool isLive)
        => isLive
        ? _sslCommerz.SSLCommerzLiveUrl
        : _sslCommerz.SSLCommerzTestUrl;

    private static bool GetValidationStatus(string status)
        => status == "VALID" || status == "VALIDATED";

    private static bool IsTranxIdAndAmountValid(SSLCommerzValidatorResponse validatorResponse
        , string tranxId
        , decimal tranxAmount
        , string tranxCurrency)
    {
        if (validatorResponse.Currency.Equals(tranxCurrency, StringComparison.OrdinalIgnoreCase)
            && tranxId == validatorResponse.TransactionId
            && (tranxAmount - validatorResponse.Amount < 1))
        {
            return true;
        }
        return false;
    }

    private Dictionary<string, string?> GetKeyValuePairs(object obj)
    {
        return obj
         .GetType()
         .GetProperties()
         .Where(p => p.GetValue(obj) != null && !string.IsNullOrEmpty(p.GetValue(obj)?.ToString()))
         .ToDictionary(
             prop => ((JsonPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(JsonPropertyAttribute)))?.PropertyName ?? prop.Name,
             prop => prop.GetValue(obj)?.ToString()
         );
    }
}




