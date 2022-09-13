using Core;
using Core.Database;
using Core.Database.Conventions;
using DocumentsManagement.Library;
using Membership.Core.Entities;
using Membership.Core.Entities.Base;
using Membership.Library.Entities;
using Membership.Library.Migrations;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Membership.Library.Data.Context
{
    public class MembershipContext : DbContext, ISelfRegisterDependency
    {
        public MembershipContext()
            : base("portalConnectionString")
        {
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null &&
                ConfigurationManager.AppSettings["__AutoMigrationEnabled__"].ToLower() == "true")
                Database.SetInitializer(
                    new MigrateDatabaseToLatestVersion<MembershipContext, MembershipContextConfiguration>());
            else
                Database.SetInitializer<MembershipContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // setup conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Add<ForeignKeyConvention>();

            // manual behavior overrides
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            // register all entities in context
            modelBuilder.RegisterAllEntities(
                new[] { typeof(BaseEntity).Assembly, typeof(Vendor).Assembly },
                new[] { typeof(BaseEntity) /*, typeof(PermissionEntity)*/},
                typeof(User).Namespace, typeof(Vendor).Namespace);


            modelBuilder.Entity<Role>().HasMany(r => r.Users).WithMany(u => u.Roles).Map(a => a.ToTable("RoleUser"));
            modelBuilder.Entity<User>().HasMany(r => r.Groups).WithMany(u => u.Users).Map(a => a.ToTable("GroupUser"));

            modelBuilder.Entity<AccessModule>()
                        .HasMany(r => r.AccessEntities)
                        .WithMany(u => u.AccessModules)
                        .Map(a => a.ToTable("AccessModuleAccessEntity"));

            // override entities mapping
            modelBuilder.Entity<Role>()
                        .HasMany(r => r.RoleAccessRights)
                        .WithRequired(ar => ar.Role)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<User>()
                        .HasMany(r => r.UserAccessRights)
                        .WithRequired(ar => ar.User)
                        .WillCascadeOnDelete();
            //modelBuilder.Entity<Folder>().HasMany(r => r.SubFolders).WithOptional(ar => ar.ParentFolder).WillCascadeOnDelete();
            modelBuilder.Entity<Company>().HasMany(r => r.Users).WithOptional(ar => ar.Company).WillCascadeOnDelete();
            modelBuilder.Entity<ApplicationPermission>();

            modelBuilder.Entity<User>()
                        .HasMany(x => x.FollowedUsers)
                        .WithMany(x => x.FollowedByUsers)
                        .Map(x => x.ToTable("FollowedUsers").MapLeftKey("UserId").MapRightKey("FollowedUserId"));

            modelBuilder.Entity<SiteUrl>().ToTable("SiteUrl");
            modelBuilder.Entity<Feature>().ToTable("Feature");

            modelBuilder.Entity<UserProfile>()
                        .HasRequired(x => x.User);

            modelBuilder.Entity<UserRoleProfileProperty>()
                        .ToTable("UserRoleProfileProperties")
                        .HasKey(x => new
                        {
                            x.UserRoleId,
                            x.PropertyId
                        });

            modelBuilder.Entity<Company>().HasOptional(x => x.MainContactUser);
            modelBuilder.Entity<Company>().HasOptional(x => x.MasterCompany)
                .WithMany(company => company.ClientCompanies)
                .HasForeignKey(company => company.MasterCompanyId);

            modelBuilder.Entity<User>().HasOptional(user => user.ClientCompany);

            DocumentsInitializer.InitializeContext<CompanyDocument>(modelBuilder);
            //modelBuilder.Entity<Company>().HasMany(p => p.CustomerDocuments).WithRequired(p => p.Master).WillCascadeOnDelete();
        }
    }
}