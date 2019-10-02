using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App.Models;

namespace Ds3App.Repository.Feedback
{
    public class FeedbackRep : IFeedbackRep
    {
        public async Task CreateFeedback(Models.Feedback feedback)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                feedback.id = Guid.NewGuid();
                context.Feedback.Add(feedback);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteFeedback(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Feedback.Find(id).IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Models.Feedback>> GetFeedback()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Feedback.Where(s => !s.IsDeleted)
                    .OrderByDescending(s => s.DateTimeStamp)
                    .ToListAsync();
            }
        }
    }
}