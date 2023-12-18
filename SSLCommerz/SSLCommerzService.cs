using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SSLCommerz.NetCore.OptionsSetup;
using System.Net;
using System.Text;

namespace SSLCommerz.NetCore.SSLCommerz;

public class SSLCommerzService : ISSLCommerzService
{
    private readonly SSLCommerzOptions _sslCommerz;
    private readonly IHttpClientFactory _httpClientFactory;

    public SSLCommerzService(IOptionsSnapshot<SSLCommerzOptions> sslCommerz, IHttpClientFactory httpClientFactory)
    {
        _sslCommerz = sslCommerz.Value;
        _httpClientFactory = httpClientFactory;
    }
    public async Task<string> InitializeTransectionAsync(SSLInitialRequest reqData, CancellationToken cancellationToken)
    {
        try
        {
            var postData = reqData.GetType()
                                       .GetProperties()
                                       .Where(p => p.GetValue(reqData) != null && !string.IsNullOrEmpty(p.GetValue(reqData)?.ToString()))
                                       .ToDictionary(
                                           prop => ((JsonPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(JsonPropertyAttribute)))?.PropertyName ?? prop.Name,
                                           prop => prop.GetValue(reqData)?.ToString()
                                       );

            postData.Add("store_id", _sslCommerz.StoreId);
            postData.Add("store_passwd", _sslCommerz.StorePass);
            postData.Add("success_url", _sslCommerz.SuccessUrl);
            postData.Add("fail_url", _sslCommerz.FailUrl);
            postData.Add("cancel_url", _sslCommerz.CancelUrl);
            postData.Add("ipn_url", _sslCommerz.IPNUrl);

            var baseUrl = GetSSLCommerzUrl(_sslCommerz.IsLive);

            var submitUrl = baseUrl + _sslCommerz.SubmitUrl;

            var content = new FormUrlEncodedContent(postData);

            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsync(submitUrl, content, cancellationToken);

            //response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                var initResponse = JsonConvert.DeserializeObject<SSLInitialResponse>(responseContent);

                return initResponse?.Status == "SUCCESS"
                    ? initResponse?.GatewayPageURL
                    : throw new Exception("Unable to get data from SSLCommerz");
            }
            else
            {
                // Handle error scenarios here
                throw new HttpRequestException($"\"Unable to get data from SSLCommerz. Please contact your manager! {response.StatusCode}");
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

        string encodedValID = WebUtility.UrlEncode(httpRequest.Form["val_id"]);
        string encodedStoreID = WebUtility.UrlEncode(_sslCommerz.StoreId);
        string encodedStorePassword = WebUtility.UrlEncode(_sslCommerz.StorePass);

        string baseUrl = GetSSLCommerzUrl(_sslCommerz.IsLive);

        string validateUrl = $"{baseUrl}{_sslCommerz.ValidationUrl}?val_id={encodedValID}&store_id={encodedStoreID}&store_passwd={encodedStorePassword}&v=1&format=json";

        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync(validateUrl, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return (false, "Unable to get transection status");
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        var validatorResponse = JsonConvert.DeserializeObject<SSLCommerzValidatorResponse>(responseContent);

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

    private bool VerifyIPNHash(HttpRequest httpRequest)
    {
        var verifySign = httpRequest.Form["verify_sign"].ToString();
        var verifyKey = httpRequest.Form["verify_key"].ToString();

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

            dataArray.Add(new KeyValuePair<string, string>("store_passwd", hashedPass));

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
        => isLive ? _sslCommerz.SSLCommerzLiveUrl : _sslCommerz.SSLCommerzTestUrl;

    private bool GetValidationStatus(string status) => status == "VALID" || status == "VALIDATED";

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
}




