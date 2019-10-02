using Ds3App.Constant;
using Ds3App.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App.EmailerService
{
    public class EmailSender
    {
        private async Task SendEmail(string HTMLTemplate, MailAddress emailAddress, string Subject)
        {
            var apiKey = ConfigurationManager.AppSettings["apikey"];
            var fromName = ConfigurationManager.AppSettings["sendGridUser"];
            var fromEmail = ConfigurationManager.AppSettings["sendGridFrom"];

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var subject = Subject;
            var to = new EmailAddress(emailAddress.Address, "");
            var plainTextContent = "";
            var htmlContent = HTMLTemplate;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            try
            {
                await client.SendEmailAsync(msg);
            } catch (Exception e)
            {

            }
        }

        public async Task ConfirmAccount(string email, string Password, string callbackUrl)
        {
            string Body = string.Empty;

            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/ConfirmAccount.html")))
            {
                Body = reader.ReadToEnd();
            }
            Body = Body.Replace("{Password}", Password);
            Body = Body.Replace("{LoginLink}", callbackUrl);

            await SendEmail(Body, new MailAddress(email), EmailSubject.ConfirmAccount);

        }
        public async Task SendQuote(string email, Quotation quotation)
        {
            string Body = string.Empty;

            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/Quote.html")))
            {
                Body = reader.ReadToEnd();
            }
            Body = Body.Replace("{ReferenceNumber}", quotation.ReferenceNumber);
            Body = Body.Replace("{EstimatedCost}", quotation.EstimatedCost.ToString("C"));
            Body = Body.Replace("{ProjectName}", quotation.ProjectName);
            Body = Body.Replace("{ProjectDescription}", quotation.ProjectDescription);
            Body = Body.Replace("{Foreman}", quotation.Foreman.ToString());
            Body = Body.Replace("{SkilledWorkers}", quotation.SkilledWorkers.ToString());
            Body = Body.Replace("{SemiSkilledWorkers}", quotation.SemiSkilledWorkers.ToString());
            Body = Body.Replace("{GeneralWorkers}", quotation.GeneralWorkers.ToString());

            Body = Body.Replace("{SkilledWorkerTasks}", quotation.SWTaskDescription);
            Body = Body.Replace("{GeneralWorkerTasks}", quotation.GWTaskDescription);
            Body = Body.Replace("{SemiSkilledWorkerTasks}", quotation.SSWTaskDescription);

            Body = Body.Replace("{StartDate}", quotation.StartDate.ToLongDateString());
            Body = Body.Replace("{EndDate}", quotation.EndDate.ToLongDateString());
            Body = Body.Replace("{Shifts}", quotation.Shifts.ToString());
            Body = Body.Replace("{ShiftHours}", quotation.ShiftHours.ToString());
            Body = Body.Replace("{WorkWeekends}", quotation.WorkWeekends ? "Yes" : "No");

            await SendEmail(Body, new MailAddress(email), EmailSubject.NewQuote);

        }
    }
}