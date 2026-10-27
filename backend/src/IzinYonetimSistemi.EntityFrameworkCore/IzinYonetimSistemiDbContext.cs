using Microsoft.EntityFrameworkCore;
using Abp.EntityFrameworkCore;
using IzinYonetimSistemi.Entities;

namespace IzinYonetimSistemi.EntityFrameworkCore
{
    public class IzinYonetimSistemiDbContext : AbpDbContext
    {
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ApprovalRecord> ApprovalRecords { get; set; }
        public DbSet<StatusChangeLog> StatusChangeLogs { get; set; }


        public IzinYonetimSistemiDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kayit basina tek bekleyen onay: es zamanli iki gonderim iki "Pending" acip onayciya
            // bir adimi atlatabiliyordu. Filtreli tekil indeks MySQL'de yok — orada uygulama kontrolu yeterli.
            var provider = Database.ProviderName ?? "";
            var pendingFilter = provider.Contains("Npgsql") ? "\"Status\" = 'Pending'" : provider.Contains("SqlServer") ? "[Status] = 'Pending'" : null;
            if (pendingFilter != null)
                modelBuilder.Entity<ApprovalRecord>()
                    .HasIndex(a => new { a.EntityType, a.EntityId })
                    .IsUnique()
                    .HasFilter(pendingFilter);

            // Employee 1:N LeaveRequest
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.LeaveRequests)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // LeaveType 1:N LeaveRequest
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(x => x.LeaveType)
                .WithMany(x => x.LeaveRequests)
                .HasForeignKey(x => x.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            // RBAC: AppUser N:N AppRole via UserRole junction
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();

            // RolePermission: AppRole 1:N RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RolePermission>()
                .HasIndex(rp => new { rp.RoleId, rp.PermissionName })
                .IsUnique();

            // AppRole.Name unique
            modelBuilder.Entity<AppRole>()
                .HasIndex(r => r.Name)
                .IsUnique();

        }
    }
}
