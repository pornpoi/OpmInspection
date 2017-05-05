using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using OpmInspection.Shared.Helpers;
using System.Threading.Tasks;
using System.Threading;
using OpmInspection.Shared.Models;

namespace OpmInspection.Shared
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, UserLogin, UserRole, UserClaim>
    {
        public ApplicationDbContext() : base("MySqlConnectionString")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        //Identity and Authorization
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<VisitorStatistic> VisitorStatistics { get; set; }
        public DbSet<OfficerStatistic> OfficerStatistics { get; set; }
        public DbSet<Background> Backgrounds { get; set; }
        public DbSet<Skin> Skins { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modify Users table in database
            modelBuilder.Entity<ApplicationUser>()
                .Ignore(m => m.PhoneNumber)
                .Ignore(m => m.PhoneNumberConfirmed);
        }

        public override int SaveChanges()
        {
            TimeStamps();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            TimeStamps();

            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            TimeStamps();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void TimeStamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is ITrackable && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((ITrackable)entity.Entity).CreatedAt = DateTime.UtcNow;
                    ((ITrackable)entity.Entity).UpdatedAt = DateTime.UtcNow;
                }

                if (entity.State == EntityState.Modified)
                {
                    ((ITrackable)entity.Entity).UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}