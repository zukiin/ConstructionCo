using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Ds3App.Repository.User
{
    public class UserRep : IUserRep
    {
        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Users.ToListAsync();
            }
        }
    }
}