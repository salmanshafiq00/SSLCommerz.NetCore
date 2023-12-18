using Newtonsoft.Json;

namespace SSLCommerz.NetCore.SSLCommerz;

public class SSLInitialRequest
{
    public SSLInitialRequest(
        string tranxId, 
        string cusName, 
        string cusPhone, 
        string cusEmail, 
        string cusAddress,
        string cusCity,
        string cusCountry,
        string productName, 
        decimal totalAmount, 
        string productCategory, 
        string productProfile, 
        string currency = "BDT",
        string shippingMethod = "NO")
    {
        TranId = tranxId;
        CusName = cusName;
        CusPhone = cusPhone;
        CusEmail = cusEmail;
        CusAdd1 = cusAddress;
        CusCity = cusCity;
        CusCountry = cusCountry;
        ProductName = productName;
        TotalAmount = totalAmount;
        ProductCategory = productCategory;
        ProductProfile = productProfile;
        ProductCategory = productCategory;
        Currency = currency;
        ShippingMethod = shippingMethod;
    }
    // System Information
    [JsonProperty("tran_id")]
    public string TranId { get; set; }
    [JsonProperty("total_amount")]
    public decimal TotalAmount { get; set; }
    [JsonProperty("currency")]
    public string Currency { get; set; }
    [JsonProperty("success_url")]
    public string SuccessUrl { get; set; }
    [JsonProperty("fail_url")]
    public string FailUrl { get; set; }
    [JsonProperty("cancel_url")]
    public string CancelUrl { get; set; }
    [JsonProperty("ipn_url")]
    public string IpnUrl { get; set; }

    // Customer Information
    [JsonProperty("cus_name")]
    public string CusName { get; set; }
    [JsonProperty("cus_email")]
    public string CusEmail { get; set; }
    [JsonProperty("cus_add1")]
    public string CusAdd1 { get; set; }
    [JsonProperty("cus_city")]
    public string CusCity { get; set; }
    [JsonProperty("cus_state")]
    public string CusState { get; set; }
    [JsonProperty("cus_postcode")]
    public string CusPostcode { get; set; }
    [JsonProperty("cus_country")]
    public string CusCountry { get; set; }
    [JsonProperty("cus_phone")]
    public string CusPhone { get; set; }

    // Product Information
    [JsonProperty("product_name")]
    public string ProductName { get; set; }
    [JsonProperty("product_category")]
    public string ProductCategory { get; set; }
    [JsonProperty("product_profile")]
    public string ProductProfile { get; set; }
    [JsonProperty("vat")]
    public string VAT { get; set; }
    [JsonProperty("discount_amount")]
    public string DiscountAmount { get; set; }



    // Shipment Information
    [JsonProperty("shipping_method")]
    public string ShippingMethod { get; set; }
    [JsonProperty("num_of_item")]
    public string NumOfItem { get; set; }
    [JsonProperty("weight_of_items")]
    public string WeightOfItems { get; set; }
    [JsonProperty("logistic_pickup_id")]
    public string LogisticPickupId { get; set; }
    [JsonProperty("logistic_delivery_type")]
    public string LogisticDeliveryType { get; set; }
    [JsonProperty("ship_name")]
    public string ShipName { get; set; }

    [JsonProperty("ship_add1")]
    public string ShippingAdd1 { get; set; }
    [JsonProperty("ship_area")]
    public string ShipArea { get; set; }
    [JsonProperty("ship_city")]
    public string ShipCity { get; set; }
    [JsonProperty("ship_state")]
    public string ShipState { get; set; }
    [JsonProperty("ship_postcode")]
    public string ShipPostCode { get; set; }
    [JsonProperty("ship_country")]
    public string ShipCountry { get; set; }

    // Customized or Additional Parameters
    [JsonProperty("value_a")]
    public string ValueA { get; set; }
    [JsonProperty("value_b")]
    public string ValueB { get; set; }
    [JsonProperty("value_c")]
    public string ValueC { get; set; }
    [JsonProperty("value_d")]
    public string ValueD { get; set; }


}
