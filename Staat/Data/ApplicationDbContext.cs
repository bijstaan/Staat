/*
 * Staat - Staat
 * Copyright (C) 2021 Matthew Kilgore (tankerkiller125)
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Staat.Helpers;
using Staat.Models;
using Staat.Models.Users;

namespace Staat.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<File> File { get; set; }
        public DbSet<Incident> Incident { get; set; }
        public DbSet<IncidentMessage> IncidentMessage { get; set; }
        public DbSet<Maintenance> Maintenance { get; set; }
        public DbSet<MaintenanceMessage> MaintenanceMessage { get; set; }
        public DbSet<Models.Monitor> Monitor { get; set; }
        public DbSet<MonitorData> MonitorData { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceGroup> ServiceGroup { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Status> Status { get; set; }
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

            var incidentEntities = this.ChangeTracker.Entries()
                .Where(x => (x.State == EntityState.Modified || x.State == EntityState.Added) && x.Entity != null && x.Entity as Incident != null)
                .Select(x => x.Entity as Incident);

            foreach (var entity in incidentEntities)
            {
                if (entity != null) entity.DescriptionHtml = MarkdownHelper.ToHtml(entity.Description);
            }
            
            var incidentMessageEntities = this.ChangeTracker.Entries()
                .Where(x => (x.State == EntityState.Modified || x.State == EntityState.Added) && x.Entity != null && x.Entity as IncidentMessage != null)
                .Select(x => x.Entity as IncidentMessage);

            foreach (var entity in incidentMessageEntities)
            {
                if (entity != null) entity.MessageHtml = MarkdownHelper.ToHtml(entity.Message);
            }

            var maintenanceEntities = this.ChangeTracker.Entries()
                .Where(x => (x.State == EntityState.Modified || x.State == EntityState.Added) && x.Entity != null &&
                            x.Entity as Maintenance != null)
                .Select(x => x.Entity as Maintenance);

            foreach (var entity in maintenanceEntities)
            {
                if (entity != null) entity.DescriptionHtml = MarkdownHelper.ToHtml(entity.Description);
            }
            
            var maintenanceMessageEntities = this.ChangeTracker.Entries()
                .Where(x => (x.State == EntityState.Modified || x.State == EntityState.Added) && x.Entity != null &&
                            x.Entity as MaintenanceMessage != null)
                .Select(x => x.Entity as MaintenanceMessage);

            foreach (var entity in maintenanceMessageEntities)
            {
                if (entity != null) entity.MessageHtml = MarkdownHelper.ToHtml(entity.Message);
            }

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