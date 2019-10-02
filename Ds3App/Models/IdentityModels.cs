using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ds3App.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public bool IsDeleted { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ConstructionTask> ConstructionTask { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<QuotationRequest> QuotationRequest { get; set; }
        public virtual DbSet<Quotation> Quotation { get; set; }
        public virtual DbSet<ProjectTask> ProjectTask { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<WorkerType> WorkerType { get; set; }
        public virtual DbSet<ForemanAssignedToProject> ForemanAssignedToProject { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<QuoteEquipment> QuoteEquipment { get; set; }
        public virtual DbSet<QuoteMaterial> QuoteMaterial { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}