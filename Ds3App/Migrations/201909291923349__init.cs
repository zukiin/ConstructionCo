namespace Ds3App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Contact = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstructionTasks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TaskName = c.String(nullable: false),
                        TaskDescription = c.String(nullable: false),
                        RatePerHour = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WorkerTypeId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ClientName = c.String(),
                        ClientSurname = c.String(),
                        Email = c.String(),
                        Contact = c.String(),
                        QuotationReference = c.String(),
                        ProjectName = c.String(),
                        ProjectCost = c.String(),
                        ContractContent = c.String(),
                        DateTimeStamp = c.String(),
                        IsSigned = c.Boolean(nullable: false),
                        IsNotAccepted = c.Boolean(nullable: false),
                        ClientId = c.String(),
                        ContractIssuedBy = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EquipmentName = c.String(nullable: false),
                        Model = c.String(nullable: false),
                        LastMaintenance = c.DateTime(nullable: false),
                        Brand = c.String(nullable: false),
                        RatePerHour = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        EquipmentImage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        Client = c.String(),
                        Project = c.String(),
                        Comment = c.String(),
                        DateTimeStamp = c.DateTime(nullable: false),
                        ClientId = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ForemanAssignedToProjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        ForemanId = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateTimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MaterialName = c.String(nullable: false),
                        MaterialDescription = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockQuantity = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Supplier = c.Guid(nullable: false),
                        SupplierName = c.String(),
                        MaterialImage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        QuotationReference = c.String(),
                        ProjectName = c.String(),
                        AmountDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        ContractId = c.Guid(nullable: false),
                        ProofOfPayment = c.String(),
                        IsOnlinePayment = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ClientId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProjectName = c.String(),
                        ProjectDescription = c.String(),
                        ProjectCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Progress = c.String(),
                        Status = c.String(),
                        IsCompleted = c.Boolean(nullable: false),
                        ClientId = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CompleteTasks = c.String(),
                        ForemanTasks = c.Int(nullable: false),
                        SkilledWorkerTasks = c.Int(nullable: false),
                        SemiSkilledWorkerTasks = c.Int(nullable: false),
                        GeneralWorkerTasks = c.Int(nullable: false),
                        QuotationReference = c.String(),
                        ProjectReport = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectTasks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        ConstructionTask = c.Guid(nullable: false),
                        DateAssigned = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        WorkerId = c.String(),
                        IsCompleted = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Quotations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ReferenceNumber = c.String(nullable: false),
                        Foreman = c.Int(nullable: false),
                        SkilledWorkers = c.Int(nullable: false),
                        SemiSkilledWorkers = c.Int(nullable: false),
                        GeneralWorkers = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        WorkWeekends = c.Boolean(nullable: false),
                        Shifts = c.Int(nullable: false),
                        ShiftHours = c.Int(nullable: false),
                        EstimatedCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        ProjectName = c.String(nullable: false),
                        ProjectDescription = c.String(nullable: false),
                        ClientId = c.String(),
                        Documents = c.String(),
                        ForemanTasks = c.Int(nullable: false),
                        SkilledWorkerTasks = c.Int(nullable: false),
                        GeneralWorkerTasks = c.Int(nullable: false),
                        SemiSkilledWorkerTasks = c.Int(nullable: false),
                        FTaskDescription = c.String(),
                        SWTaskDescription = c.String(),
                        SSWTaskDescription = c.String(),
                        GWTaskDescription = c.String(),
                        Approved = c.Boolean(nullable: false),
                        Declined = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuotationRequests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ClientId = c.String(),
                        ReferenceNumber = c.String(nullable: false),
                        ProjectName = c.String(nullable: false),
                        ProjectDescription = c.String(nullable: false),
                        ProjectDocuments = c.String(),
                        DateTimeStamp = c.DateTime(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuoteEquipments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        QuotationRequestId = c.Guid(nullable: false),
                        EquipmentId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        IsAdded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuoteMaterials",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        QuotationRequestId = c.Guid(nullable: false),
                        MaterialId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        IsAdded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SupplierName = c.String(nullable: false),
                        SupplierContact = c.String(nullable: false, maxLength: 10),
                        SupplierEmail = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TRANSACTION_ID = c.Guid(nullable: false, identity: true),
                        DATE = c.DateTime(),
                        PAY_REQUEST_ID = c.String(),
                        AMOUNT = c.Int(nullable: false),
                        REFERENCE = c.String(),
                        TRANSACTION_STATUS = c.String(),
                        RESULT_CODE = c.Int(nullable: false),
                        RESULT_DESC = c.String(),
                        CUSTOMER_EMAIL_ADDRESS = c.String(),
                    })
                .PrimaryKey(t => t.TRANSACTION_ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Contact = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WorkerTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(nullable: false),
                        Slug = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.WorkerTypes");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Transactions");
            DropTable("dbo.Suppliers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.QuoteMaterials");
            DropTable("dbo.QuoteEquipments");
            DropTable("dbo.QuotationRequests");
            DropTable("dbo.Quotations");
            DropTable("dbo.ProjectTasks");
            DropTable("dbo.Projects");
            DropTable("dbo.Payments");
            DropTable("dbo.Materials");
            DropTable("dbo.ForemanAssignedToProjects");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Equipments");
            DropTable("dbo.Contracts");
            DropTable("dbo.ConstructionTasks");
            DropTable("dbo.Clients");
        }
    }
}
