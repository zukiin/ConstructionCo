using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App.Models;

namespace Ds3App.Repository.User
{
    public interface IUserRep
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
    }
}