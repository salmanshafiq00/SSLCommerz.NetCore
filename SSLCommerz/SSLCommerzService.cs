﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SSLCommerz.NetCore.OptionsSetup;
using System.Net;
using System.Text;

namespace SSLCommerz.NetCore.SSLCommerz;

public class SSLCommerzService(
    IOptionsSnapshot<SSLCommerzOptions> sslCommerz, 
    IHttpClientFactory httpClientFactory) 
    : ISSLCommerzService
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


    public async Task<string> InitializeTransectionAsync(
        SSLInitialRequest reqData, 
        CancellationToken cancellationToken)
    {
        try
        {
            var postData = GetKeyValuePairs(reqData);

            var appBaseUrl = _sslCommerz.IsDevelopment ? _sslCommerz.AppDevBaseUrl : _sslCommerz.AppBaseUrl;

            postData.Add(STORE_ID, _sslCommerz.StoreId);
            postData.Add(STORE_PASSWD, _sslCommerz.StorePass);
            postData.Add(SUCCESS_URL, $"{appBaseUrl}{_sslCommerz.SuccessUrl}");
            postData.Add(FAIL_URL, $"{appBaseUrl}{_sslCommerz.FailUrl}");
            postData.Add(CANCEL_URL, $"{appBaseUrl}{_sslCommerz.CancelUrl}");
            postData.Add(IPN_URL, $"{appBaseUrl}{_sslCommerz.IPNUrl}");

            var sslBaseUrl = GetSSLCommerzUrl(_sslCommerz.IsSandbox);

            var submitUrl = sslBaseUrl + _sslCommerz.SubmitUrl;

            var content = new FormUrlEncodedContent(postData);

            var client = _httpClientFactory.CreateClient("SSLCommerz");

            var response = await client.PostAsync(submitUrl, content, cancellationToken);

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
        , SSLCallbackResponse callbackResponse
        , CancellationToken cancellationToken)
    {
        bool hashVerified = VerifyIPNHash(callbackResponse);
        if (!hashVerified)
        {
            return (false, "Unable to verify hash");
        }

        string encodedValID = WebUtility.UrlEncode(callbackResponse.ValidationId);
        string encodedStoreID = WebUtility.UrlEncode(_sslCommerz.StoreId);
        string encodedStorePassword = WebUtility.UrlEncode(_sslCommerz.StorePass);

        var sslBaseUrl = GetSSLCommerzUrl(_sslCommerz.IsSandbox);

        string validateUrl = $"{sslBaseUrl}{_sslCommerz.ValidationUrl}?{VAL_ID}={encodedValID}&{STORE_ID}={encodedStoreID}&{STORE_PASSWD}={encodedStorePassword}&v=1&format=json";

        var client = _httpClientFactory.CreateClient("SSLCommerz");

        var response = await client.GetAsync(validateUrl, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return (false, "Unable to get transection status");
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        var validatorResponse = JsonConvert
            .DeserializeObject<SSLCommerzValidatorResponse>(responseContent);

        if (validatorResponse is not {Status: "VALID" or "VALIDATED" })
        {
            return (false, "This transaction is either expired or fails");
        }

        if (!IsTranxIdAndAmountValid(validatorResponse, tranxId, tranxAmount, tranxCurrency))
        {
            return (false, "Amount not matching");
        }
        return (true, "Transaction Validated");
    }


    public async Task<SSLTransactionQueryResponse> GetTransactionDetail(
        string tranxId, 
        CancellationToken cancellationToken)
    {
        var sslBaseUrl = GetSSLCommerzUrl(_sslCommerz.IsSandbox);

        string encodedTranxID = WebUtility.UrlEncode(tranxId);
        string encodedStoreID = WebUtility.UrlEncode(_sslCommerz.StoreId);
        string encodedStorePassword = WebUtility.UrlEncode(_sslCommerz.StorePass);

        var checkingUrl = $"{sslBaseUrl}{_sslCommerz.CheckingUrl}?{TRANX_ID}={encodedTranxID}&{STORE_ID}={encodedStoreID}&{STORE_PASSWD}={encodedStorePassword}&v=1&format=json";

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


    private bool VerifyIPNHash(SSLCallbackResponse callbackResponse)
    {
        var verifySign = callbackResponse.VerifySign;
        var verifyKey = callbackResponse.VerifyKey;

        // Check For verify_sign and verify_key parameters
        if (!string.IsNullOrEmpty(verifySign) && !string.IsNullOrEmpty(verifyKey))
        {

            // Split key string by comma to make a list array
            var keyList = verifyKey.Split(',').ToList();

            // Initiate a key value pair list array
            var dataArray = keyList
                .Select(key => new KeyValuePair<string, string>(key, GetPropertyValue(callbackResponse, key)))
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

    private string GetSSLCommerzUrl(bool isSandbox)
        => isSandbox
        ? _sslCommerz.SSLCommerzTestUrl
        : _sslCommerz.SSLCommerzUrl;

    private static bool IsTranxIdAndAmountValid(SSLCommerzValidatorResponse validatorResponse
        , string tranxId
        , decimal tranxAmount
        , string tranxCurrency)
    {
        if (validatorResponse.Currency.Equals(tranxCurrency, StringComparison.OrdinalIgnoreCase)
            && tranxId == validatorResponse.TranxId
            && (tranxAmount - validatorResponse.Amount < 1))
        {
            return true;
        }
        return false;
    }

    private static Dictionary<string, string?> GetKeyValuePairs(object obj)
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

    private static string GetPropertyValue(object obj, string propertyName)
    {
        var propertyInfo = obj.GetType().GetProperties()
            .FirstOrDefault(p => p.GetCustomAttributes(true)
                .OfType<JsonPropertyAttribute>()
                .Any(attr => attr.PropertyName == propertyName));

        if (propertyInfo != null)
        {
            // Get the value of the property
            var propertyValue = propertyInfo.GetValue(obj);
            return propertyValue?.ToString() ?? string.Empty;
        }
        return string.Empty;
    }
}




