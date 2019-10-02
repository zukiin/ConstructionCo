using Ds3App.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Ds3App.Extension
{
    public static class Helper
    {
        private static bool res = false;
        private static Random random = new Random();
        public static string GetReference()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool IsImage(string ext)
        {
            string[] formats = ConfigurationManager.AppSettings["ImageFormats"].Split(',');
            for(int i = 0; i < formats.Length; i++)
            {
                if (ext.Remove(0,1).ToUpper() == formats[i])
                {
                    res = true;
                    break;
                }  
            }
            return res;
        }
        public static bool IsRespondedTo(Guid contractId)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                try
                {
                    string @ref = context.Contract.Find(contractId).QuotationReference;
                    return context.Payment.Any(p => p.QuotationReference == @ref);
                }
                catch 
                {
                    return res;
                }
            }
        }

        public static bool ProjectComplete(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                try
                {
                    if(context.ProjectTask.Any(p => !p.IsCompleted && !p.IsDeleted && p.ProjectId == id))
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        public static bool Commented(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Feedback.Any(p => p.ProjectId == id);
            }
        }
    }
}