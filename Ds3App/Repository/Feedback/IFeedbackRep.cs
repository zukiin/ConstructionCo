using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App.Repository.Feedback
{
    public interface IFeedbackRep
    {
        Task<IEnumerable<Models.Feedback>> GetFeedback();
        Task CreateFeedback(Models.Feedback feedback);
        Task DeleteFeedback(Guid id);
    }
}
