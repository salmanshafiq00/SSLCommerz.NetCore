using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSLCommerz.NetCore.SSLCommerz;
using SSLCommerz.NetCore.Utilities;

namespace SSLCommerz.NetCore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController(ISSLCommerzService sSLCommerzService) : ControllerBase
    {
        private readonly ISSLCommerzService _sSLCommerzService = sSLCommerzService;

        [HttpPost]
        public async Task<ActionResult> Checkout(Guid productId)
        {
            //Todo: Get Product Information from DB using CQRS/Repository
            // Here is used a private method for simplicity.

            SSLInitialRequest reqData = GetPaymentData(productId);

            // payment initialize. It will return gateway return url.
            string response = await _sSLCommerzService
                .InitializeTransectionAsync(reqData, new CancellationToken());

            //Todo Here we can save invoice with payment status false. after success payment we will do it true;
            // return Redirect(response);    // if mvc application.
            return Ok(response);    //return Gateway api
        }

        [EnableCors("SSLCommerzOrigins")]
        [HttpPost]
        public async Task<ActionResult> CheckoutSuccess([FromForm][ModelBinder(typeof(SnakeCaseModelBinder))] SSLCallbackResponse callBackResponse)
        {
            var allowedOrigins = new HashSet<string>()
            {
                "https://securepay.sslcommerz.com",
                "https://sandbox.sslcommerz.com"
            };

            // Check if the request has an "Origin" header
            if (
                Request.Headers.TryGetValue("Origin", out var origin) 
                && !allowedOrigins.Contains(origin))
            {
                return StatusCode(403, "Forbidden: Invalid Origin");
            }

            if (!(!String.IsNullOrEmpty(Request.Form["status"]) && Request.Form["status"] == "VALID"))
            {
                return BadRequest("There some error while processing your payment. Please try again.");
            }

            // Cross Check here with post data and with ur saved invoice data

            // Get invoice data from DB by tranxId. if data not found then return BadRequest()
            decimal amount = 55000;
            string currency = "BDT";

            var (status, message) = await _sSLCommerzService
                .ValidatePaymentAsync(callBackResponse.TranxId, amount, currency, callBackResponse, new CancellationToken());
            
            if (status)
            {
                //Todo: if respose is true then update invoice payment status true
            }

            var successInfo = $"Validation Response: {status}\nYour TranxID: {callBackResponse.TranxId}";

            return Ok(successInfo);

            // use it for ur web api application to direct to success page. 
            //return Redirect($"http://localhost:4200/payment/success/{callBackResponse.TranxId}"); 
        }

        [EnableCors("SSLCommerzOrigins")]
        [HttpPost]
        public IActionResult CheckoutFail([FromForm][ModelBinder(typeof(SnakeCaseModelBinder))] SSLCallbackResponse callBackResponse)
        {
            var jsonData = GetJsonCollection(Request.Form);

            string tranxId = Request.Form["tran_id"].ToString();

            // Todo: if transection fail then we will delete the invoice or keep false of payment status.

            return BadRequest("There some error while processing your payment. Please try again.");

            // use it for ur web api application to direct to fail page. 
            //return Redirect($"http://localhost:4200/payment/fail/{tranxId}"); 
        }

        [EnableCors("SSLCommerzOrigins")]
        [HttpPost]
        public IActionResult CheckoutCancel()
        {
            string tranxID = Request.Form["tran_id"].ToString();

            return BadRequest("Your payment has been cancel");

            // use it for ur web api application to direct to fail page. 
            //return Redirect($"http://localhost:4200/payment/cancel/{tranxId}"); 
        }

        [EnableCors("SSLCommerzOrigins")]
        [HttpPost]
        public IActionResult CheckoutIPN()
        {
            var req = Request.Form;

            string tranxID = Request.Form["tran_id"].ToString();

            return BadRequest("Your payment has been cancel");

            // use it for ur web api application to direct to fail page. 
            //return Redirect($"http://localhost:4200/payment/cancel/{tranxId}"); 
        }

        [HttpGet]
        public async Task<ActionResult<SSLTransactionQueryResponse>> TransactionDetail(string tranxId)
        {
            return await _sSLCommerzService.GetTransactionDetail(tranxId, new CancellationToken());
        }

        private static SSLInitialRequest GetPaymentData(Guid packageId)
        {
            var userId = Guid.NewGuid().ToString();
            var transectionId = packageId.ToString();
            var userName = "TestUser";
            var emailAddress = "sample@localhost.com";
            var cusAddress = "Dhaka";
            var cusCity = "Dhaka";
            var cusCountry = "Bangladesh";
            var packageName = "Package One";
            var price = 55000;
            var mobile = "01119343493";
            var productCategory = "ERP Service";
            var productProfile = "Non-physical-goods";

            SSLInitialRequest initRequestInfo = new(
                transectionId,
                userName,
                mobile,
                emailAddress,
                cusAddress,
                cusCity,
                cusCountry,
                packageName,
                price,
                productCategory,
                productProfile);

            return initRequestInfo;
        }
        private static string GetJsonCollection(IFormCollection formCollection)
        {
            // Extract form data
            var formData = formCollection
                .ToDictionary(k => k.Key, v => v.Value.ToString());

            // Serialize to JSON using Newtonsoft.Json
            var jsonString = JsonConvert.SerializeObject(formData);

            // Your logic here

            return jsonString;
        }
    }
}
