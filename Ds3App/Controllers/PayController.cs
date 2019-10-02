using Ds3App.Models;
using Ds3App.Repository.Pay;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ds3App.Controllers
{
    public class PayController : Controller
    {
        // GET: Pay
        private ApplicationDbContext db = new ApplicationDbContext();
        private IPayment _payment = new Repository.Pay.Payment();

        // Get Paygate keys from webconfig
        readonly string PayGateID = ConfigurationManager.AppSettings["PAYGATEID"];
        readonly string PayGateKey = ConfigurationManager.AppSettings["PAYGATEKEY"];

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetRequest(Guid? id)
        {
            if (id == null) throw new NullReferenceException();
            var contract = db.Contract.Find(id);
            HttpClient http = new HttpClient();
            Dictionary<string, string> request = new Dictionary<string, string>();
            string paymentAmount = (double.Parse(contract.ProjectCost) * 100).ToString("00"); // amount int cents e.i 50 rands is 5000 cents

            request.Add("PAYGATE_ID", PayGateID);
            request.Add("REFERENCE", contract.QuotationReference); // Payment ref e.g ORDER NUMBER
            request.Add("AMOUNT", paymentAmount);
            request.Add("CURRENCY", "ZAR"); // South Africa
            request.Add("RETURN_URL", $"{Request.Url.Scheme}://{Request.Url.Authority}/pay/completepayment");
            request.Add("TRANSACTION_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            request.Add("LOCALE", "en-za");
            request.Add("COUNTRY", "ZAF");

            // get authenticated user's email
            // use a valid email, paygate will send a transaction confirmation to it
            var email = db.Client.Where(c => c.UserId == contract.ClientId).FirstOrDefault().Email;
            request.Add("EMAIL", email);

            request.Add("CHECKSUM", _payment.GetMd5Hash(request, PayGateKey));

            string requestString = _payment.ToUrlEncodedString(request);
            StringContent content = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = await http.PostAsync("https://secure.paygate.co.za/payweb3/initiate.trans", content);
            // if the request did not succeed, this line will make the program crash
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();

            Dictionary<string, string> results = _payment.ToDictionary(responseContent);

            if (results.Keys.Contains("ERROR"))
            {
                return Json(new
                {
                    success = false,
                    message = "An error occured while initiating your request"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!_payment.VerifyMd5Hash(results, PayGateKey, results["CHECKSUM"]))
            {
                return Json(new
                {
                    success = false,
                    message = "MD5 verification failed"
                }, JsonRequestBehavior.AllowGet);
            }

            bool IsRecorded = _payment.AddTransaction(request, results["PAY_REQUEST_ID"]);
            if (IsRecorded)
            {
                return Json(new
                {
                    success = true,
                    message = "Request completed successfully",
                    results
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = false,
                message = "Failed to record a transaction"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CompletePayment()
        {
            string responseContent = Request.Params.ToString();
            Dictionary<string, string> results = _payment.ToDictionary(responseContent);

            Transaction transaction = _payment.GetTransaction(results["PAY_REQUEST_ID"]);

            if (transaction == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            Dictionary<string, string> validationSet = new Dictionary<string, string>();
            validationSet.Add("PAYGATE_ID", PayGateID);
            validationSet.Add("PAY_REQUEST_ID", results["PAY_REQUEST_ID"]);
            validationSet.Add("TRANSACTION_STATUS", results["TRANSACTION_STATUS"]);
            validationSet.Add("REFERENCE", transaction.REFERENCE);

            if (!_payment.VerifyMd5Hash(validationSet, PayGateKey, results["CHECKSUM"]))
            {
                return RedirectToAction("BadRequest", "Home");
            }

            int paymentStatus = int.Parse(results["TRANSACTION_STATUS"]);
            if (paymentStatus == 1)
            {
                await VerifyTransaction(responseContent, transaction.REFERENCE);
                return RedirectToAction("Index","Payments");
            }
            return RedirectToAction("BadRequest", "Home");
        }

        private async Task VerifyTransaction(string responseContent, string Referrence)
        {
            HttpClient client = new HttpClient();
            Dictionary<string, string> response = _payment.ToDictionary(responseContent);
            Dictionary<string, string> request = new Dictionary<string, string>();

            request.Add("PAYGATE_ID", PayGateID);
            request.Add("PAY_REQUEST_ID", response["PAY_REQUEST_ID"]);
            request.Add("REFERENCE", Referrence);
            request.Add("CHECKSUM", _payment.GetMd5Hash(request, PayGateKey));

            string requestString = _payment.ToUrlEncodedString(request);

            StringContent content = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded");

            // ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpResponseMessage res = await client.PostAsync("https://secure.paygate.co.za/payweb3/query.trans", content);
            res.EnsureSuccessStatusCode();

            string _responseContent = await res.Content.ReadAsStringAsync();

            Dictionary<string, string> results = _payment.ToDictionary(_responseContent);
            if (!results.Keys.Contains("ERROR"))
            {
                _payment.UpdateTransaction(results, results["PAY_REQUEST_ID"]);
            }

        }

        public ViewResult Complete(int? id)
        {
            string status = "Unknown";
            switch (id.ToString())
            {
                case "-2":
                    status = "Unable to reconsile transaction";
                    break;
                case "-1":
                    status = "Checksum Error. The values have been altered";
                    break;
                case "0":
                    status = "Not Done";
                    break;
                case "1":
                    status = "Approved";
                    break;
                case "2":
                    status = "Declined";
                    break;
                case "3":
                    status = "Cancelled";
                    break;
                case "4":
                    status = "User Cancelled";
                    break;
                default:
                    status = $"Unknown Status({ id })";
                    break;
            }
            TempData["Status"] = status;

            return View();
        }
    }
}