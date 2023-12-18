using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SSLCommerz.NetCore.SSLCommerz;

namespace SSLCommerz.NetCore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ISSLCommerzService _sSLCommerzService;

        public PaymentController(ISSLCommerzService sSLCommerzService)
        {
            _sSLCommerzService = sSLCommerzService;
        }
        [HttpPost]
        public async Task<ActionResult> Checkout(Guid packageId)
        {
            //Todo: Get Package Information from DB using CQRS

            SSLInitialRequest reqData = GetPaymentData(packageId);

            // payment initialize
            string response = await _sSLCommerzService.InitializeTransectionAsync(reqData, new CancellationToken());

            //Todo Here we can save invoice with payment status false. after success payment we will do it true;

            return Ok(response);    //returnre Gateway api
        }

        [EnableCors("SSLCommerzOrigins")]
        [HttpPost]
        public async Task<ActionResult> CheckoutSuccess()
        {
            var allowedOrigins = new HashSet<string>() { "https://securepay.sslcommerz.com", "https://sandbox.sslcommerz.com" };

            // Check if the request has an "Origin" header
            if (Request.Headers.TryGetValue("Origin", out var origin) && !allowedOrigins.Contains(origin))
            {
                return StatusCode(403, "Forbidden: Invalid Origin");
            }

            if (!(!String.IsNullOrEmpty(Request.Form["status"]) && Request.Form["status"] == "VALID"))
            {
                return BadRequest("There some error while processing your payment. Please try again.");
            }

            string tranxId = Request.Form["tran_id"].ToString();

            // Cross Check here with post data and with ur saved invoice data

            // Get invoice data from DB using tranxId
            decimal amount = 55000;
            string currency = "BDT";

            var resonse = await _sSLCommerzService.ValidatePaymentAsync(tranxId, amount, currency, Request, new CancellationToken());
            if (resonse.status)
            {
                //Todo: if respose is true then update invoice payment status true
            }
            var successInfo = $"Validation Response: {resonse.status}";

            return Ok(successInfo);

            //return Redirect("http://localhost:4200/payment/success/${TrxID}"); // it should to payment success component
        }

        [EnableCors("SSLCommerzOrigins")]
        [HttpPost]
        public IActionResult CheckoutFail()
        {
            string tranxID = Request.Form["tran_id"].ToString();

            // Todo: if transection fail then we will delete the invoice or keep false of payment status.

            return BadRequest("There some error while processing your payment. Please try again.");

            //return Redirect("http://localhost:4200/payment/fail/${TrxID}"); // it should to payment success component
        }

        [EnableCors("SSLCommerzOrigins")]
        [HttpPost]
        public IActionResult CheckoutCancel()
        {
            string tranxID = Request.Form["tran_id"].ToString();

            return BadRequest("Your payment has been cancel");
        }

        private static SSLInitialRequest GetPaymentData(Guid packageId)
        {
            var businessProfileId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            var transectionId = Guid.NewGuid().ToString();
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
    }
}
