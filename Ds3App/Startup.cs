using Ds3App.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Configuration;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(Ds3App.Startup))]
namespace Ds3App
{
    public partial class Startup
    {
        private readonly string email = "admin@app.com";
        private readonly string password = "Password01";
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AddRolesAndAdmin();
            AddWorkerType();
            AddRoles();
        }

        private void AddRoles()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                if (!roleManger.RoleExists("Foreman"))
                {
                    var role = new IdentityRole("Foreman");
                    roleManger.Create(role);
                }
                if (!roleManger.RoleExists("Worker"))
                {
                    var role = new IdentityRole("Worker");
                    roleManger.Create(role);
                }
            }
        }

        private void AddWorkerType()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                if (!context.WorkerType.Any())
                {
                    WorkerType worker1 = new WorkerType();
                    WorkerType worker2 = new WorkerType();
                    WorkerType worker3 = new WorkerType();
                    WorkerType worker4 = new WorkerType();

                    worker1.Id = Guid.NewGuid();
                    worker2.Id = Guid.NewGuid();
                    worker3.Id = Guid.NewGuid();
                    worker4.Id = Guid.NewGuid();

                    worker1.Type = "Foreman";
                    worker2.Type = "General Worker";
                    worker3.Type = "Semi-skilled Worker";
                    worker4.Type = "Skilled Worker";

                    worker1.Slug = "foreman";
                    worker2.Slug = "generalworker";
                    worker3.Slug = "semiskilledworker";
                    worker4.Slug = "skilledworker";

                    context.WorkerType.Add(worker1);
                    context.WorkerType.Add(worker2);
                    context.WorkerType.Add(worker3);
                    context.WorkerType.Add(worker4);

                    context.SaveChanges();
                }
            }
        }

        private void AddRolesAndAdmin()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                if (!roleManger.RoleExists("Administrator"))
                {
                    var role = new IdentityRole("Administrator");
                    roleManger.Create(role);

                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                    };

                    var newUser = userManager.Create(user, password);
                    if (newUser.Succeeded)
                    {
                        userManager.AddToRole(user.Id, "Administrator");
                    }
                }
                if (!roleManger.RoleExists("Client"))
                {
                    var role = new IdentityRole("Client");
                    roleManger.Create(role);
                }
            }
        }
    }
}
