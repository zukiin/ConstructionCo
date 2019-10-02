using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App.Models;

namespace Ds3App.Repository.Client
{
    public class ClientRep : IClientRep
    {
        public async Task CreateClient(Models.Client client)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Client.Add(new Models.Client()
                {
                    Id = Guid.NewGuid(),
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Email = client.Email,
                    Contact = client.Contact,
                    UserId = client.UserId
                });
                await context.SaveChangesAsync();
            }
        }
    }
}