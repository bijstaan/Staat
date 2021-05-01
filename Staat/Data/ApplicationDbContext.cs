using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Staat.Models;
using Staat.Models.Users;
using Staat.Services;

namespace Staat.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceGroup> ServiceGroup { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Incident> Incident { get; set; }
        public DbSet<IncidentMessage> IncidentMessage { get; set; }
        public DbSet<Models.Monitor> Monitor { get; set; }
        public DbSet<MonitorData> MonitorData { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<User> User { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            var newEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Added &&
                         x.Entity != null &&
                         x.Entity as ITimeStampedModel != null
                )
                .Select(x => x.Entity as ITimeStampedModel);

            var modifiedEntities = this.ChangeTracker.Entries() 
                .Where(
                    x => x.State == EntityState.Modified &&
                         x.Entity != null &&
                         x.Entity as ITimeStampedModel != null
                )
                .Select(x => x.Entity as ITimeStampedModel);

            foreach (var newEntity in newEntities)
            {
                if (newEntity != null)
                {
                    newEntity.CreatedAt = DateTime.UtcNow;
                    newEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

            foreach (var modifiedEntity in modifiedEntities)
            {
                modifiedEntity.UpdatedAt = DateTime.UtcNow;
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            var newEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Added &&
                         x.Entity != null &&
                         x.Entity as ITimeStampedModel != null
                )
                .Select(x => x.Entity as ITimeStampedModel);

            var modifiedEntities = this.ChangeTracker.Entries() 
                .Where(
                    x => x.State == EntityState.Modified &&
                         x.Entity != null &&
                         x.Entity as ITimeStampedModel != null
                )
                .Select(x => x.Entity as ITimeStampedModel);

            foreach (var newEntity in newEntities)
            {
                if (newEntity != null)
                {
                    newEntity.CreatedAt = DateTime.UtcNow;
                    newEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

            foreach (var modifiedEntity in modifiedEntities)
            {
                if (modifiedEntity != null) modifiedEntity.UpdatedAt = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}