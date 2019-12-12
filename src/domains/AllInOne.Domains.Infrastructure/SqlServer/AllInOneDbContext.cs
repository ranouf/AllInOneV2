using AllInOne.Common.Entities;
using AllInOne.Common.Events;
using AllInOne.Common.Extensions;
using AllInOne.Common.Session;
using AllInOne.Domains.Core.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllInOne.Domains.Infrastructure.SqlServer
{
    public class AllInOneDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        private readonly IDomainEvents _domainEvents;
        public IUserSession _session { get; private set; }
        protected Guid? UserId { get; set; }


        public AllInOneDbContext() { }

        public AllInOneDbContext(DbContextOptions<AllInOneDbContext> options) : base(options) { }

        public AllInOneDbContext(
            DbContextOptions<AllInOneDbContext> options,
            IUserSession session,
            IDomainEvents domainEvents
        ) : base(options)
        {
            _session = session;
            _domainEvents = domainEvents;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            PrepareIdentityModel(builder);
        }

        private void PrepareIdentityModel(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.CreatedByUser);
                entity.HasOne(u => u.UpdatedByUser);
                entity.HasOne(u => u.DeletedByUser);
                entity.HasIndex(u => u.Email);
                entity.HasIndex(u => u.NormalizedEmail);
                entity.HasIndex(u => u.IsDeleted);
                entity.Property(u => u.FullName)
                    .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");
                entity.HasIndex(u => u.IsDeleted);
                entity.HasQueryFilter(p => !p.IsDeleted);
            });

            builder.Entity<Role>(entity =>
            {
                entity.HasIndex(r => r.Name);
                entity.HasIndex(r => r.NormalizedName);
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(e => e.RoleId)
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired();
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_session.UserId.HasValue)
            {
                var user = Users.FirstOrDefault(u => u.Id == _session.UserId.Value);
                if (user != null)
                {
                    UserId = user.Id;
                }
            }

            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetCreationAuditProperties(entry);
                        break;
                    case EntityState.Modified:
                        SetModificationAuditProperties(entry);

                        if (entry.Entity.IsAssignableToGenericType(typeof(ISoftDelete<>))
                            && entry.Entity.TryGetPropertyValue("IsDeleted", out bool isDeleted)
                            && isDeleted
                        )
                        {
                            SetDeletionAuditProperties(entry);
                        }
                        break;
                    case EntityState.Deleted:
                        CancelDeletionForSoftDelete(entry);
                        SetDeletionAuditProperties(entry);
                        break;
                }
            }
        }

        protected virtual void SetCreationAuditProperties(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(ICreationAudited<>)))
            {
                if (entry.Entity.TryGetPropertyValue("CreatedAt", out DateTimeOffset? createdAt) && !createdAt.HasValue)
                {
                    entry.Entity.SetPropertyValue<DateTimeOffset?>("CreatedAt", DateTime.Now);
                }

                if (entry.Entity.TryGetPropertyValue("CreatedByUserId", out Guid? createdByUserId) && !createdByUserId.HasValue)
                {
                    entry.Entity.SetPropertyValue("CreatedByUserId", UserId);
                }
            }
        }

        protected virtual void SetModificationAuditProperties(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(IUpdateAudited<>)))
            {
                entry.Entity.SetPropertyValue<DateTimeOffset?>("UpdatedAt", DateTime.Now);
                entry.Entity.SetPropertyValue("UpdatedByUserId", UserId);
            }
        }

        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(ISoftDelete<>)))
            {
                entry.State = EntityState.Unchanged;
                entry.Entity.SetPropertyValue("IsDeleted", true);
            }
        }

        protected virtual void SetDeletionAuditProperties(EntityEntry entry)
        {
            if (entry.Entity.IsAssignableToGenericType(typeof(IDeleteAudited<>)))
            {
                if (entry.Entity.TryGetPropertyValue("DeletedAt", out DateTimeOffset? deletedAt) && !deletedAt.HasValue)
                {
                    entry.Entity.SetPropertyValue<DateTimeOffset?>("DeletedAt", DateTime.Now);
                }

                if (entry.Entity.TryGetPropertyValue("DeletedByUserId", out Guid? deletedByUserId) && !deletedByUserId.HasValue)
                {
                    entry.Entity.SetPropertyValue("DeletedByUserId", UserId);
                }
            }
        }
    }
}
