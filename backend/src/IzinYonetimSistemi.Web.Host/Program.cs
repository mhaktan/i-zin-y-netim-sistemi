using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IzinYonetimSistemi.EntityFrameworkCore;
using IzinYonetimSistemi.EntityFrameworkCore.Seed;
using IzinYonetimSistemi.Entities;

namespace IzinYonetimSistemi.Web.Host
{
    /// <summary>
    /// Background service that runs migration + seed once at startup
    /// without blocking the HTTP pipeline.
    /// </summary>
    public class MigrationHostedService : IHostedService
    {
        private readonly IConfiguration _config;

        public MigrationHostedService(IConfiguration config)
        {
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var connStr = _config.GetConnectionString("Default") ?? "";
            if (string.IsNullOrEmpty(connStr)) return;

            // Run in background to avoid blocking host startup (prevents EF tooling timeout)
            _ = Task.Run(async () =>
            {
                // Small delay to ensure host is fully started before DB operations
                await Task.Delay(1000, cancellationToken);
                try
                {
                    var optionsBuilder = new DbContextOptionsBuilder();
                    optionsBuilder.UseNpgsql(connStr);

                    using (var db = new IzinYonetimSistemiDbContext(optionsBuilder.Options))
                    {
                        // EnsureCreated bos veritabaninda en guncel semayi kurar ve true doner.
                        // Dolu veritabaninda hicbir sey yapmaz — o durumda bekleyen migration'lar
                        // SchemaMigrations.Apply icinde sirayla calistirilir.
                        var freshlyCreated = db.Database.EnsureCreated();
                        Console.WriteLine("[Migration] Migration tanimi yok — sema EnsureCreated ile yonetiliyor.");
                        Console.WriteLine("[Migration] Database is up to date.");

                        // Seed sample data — wrapped in its own try so a failure here doesn't block RBAC seed below.
                        try
                        {
                    if (!db.LeaveTypes.Any())
                    {
                        db.LeaveTypes.AddRange(
                    new LeaveType { Id = 1, Name = "İzin Türü 1", Description = "Yıllık plan kapsamında hazırlandı.", MaxDaysPerYear = 42, RequiresDocument = true },
                    new LeaveType { Id = 2, Name = "İzin Türü 2", Description = "Departman talebi doğrultusunda güncellendi.", MaxDaysPerYear = 17, RequiresDocument = false }
                        );
                    }
                    if (!db.LeaveRequests.Any())
                    {
                        db.LeaveRequests.AddRange(
                    new LeaveRequest { Id = 5, RequestNo = "KOD-001", StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(-3), TotalDays = 42, Reason = "Örnek 1", Status = (LeaveRequestStatus)0, RevisionNote = "Yıllık plan kapsamında hazırlandı.", EmployeeId = 3, LeaveTypeId = 1 },
                    new LeaveRequest { Id = 6, RequestNo = "KOD-002", StartDate = DateTime.Today.AddDays(-24), EndDate = DateTime.Today.AddDays(-24), TotalDays = 17, Reason = "Örnek 2", Status = (LeaveRequestStatus)4, RevisionNote = "Departman talebi doğrultusunda güncellendi.", EmployeeId = 4, LeaveTypeId = 2 }
                        );
                    }
                    if (!db.Employees.Any())
                    {
                        db.Employees.AddRange(
                    new Employee { Id = 3, FullName = "Ayşe Yılmaz", EmployeeCode = "KOD-001", Department = "Örnek 1", Title = "Pazarlama bütçesi 2026", HireDate = DateTime.Today.AddDays(-3), AnnualLeaveBalance = 42 },
                    new Employee { Id = 4, FullName = "Mehmet Demir", EmployeeCode = "KOD-002", Department = "Örnek 2", Title = "Yazılım lisans yenileme", HireDate = DateTime.Today.AddDays(-24), AnnualLeaveBalance = 17 }
                        );
                    }
                            db.SaveChanges();
                            Console.WriteLine("[Seed] Sample data created.");
                        }
                        catch (Exception sampleEx)
                        {
                            Console.WriteLine($"[Seed] Sample data skipped: {sampleEx.GetType().Name}: {sampleEx.Message}");
                            // Carry on — RBAC seed must still run so admin/123qwe is usable.
                        }
                        // Sync identity sequences to MAX(Id). Seeded rows carry explicit Ids which do NOT
                        // advance Postgres identity sequences → nextval collides with a seed row and the
                        // first few inserts fail with a duplicate-key 500. Runs every startup; idempotent.
                        try
                        {
                            foreach (var et in db.Model.GetEntityTypes())
                            {
                                var pk = et.FindPrimaryKey();
                                var table = et.GetTableName();
                                if (pk == null || pk.Properties.Count != 1 || table == null) continue;
                                var pkType = Nullable.GetUnderlyingType(pk.Properties[0].ClrType) ?? pk.Properties[0].ClrType;
                                if (pkType != typeof(int) && pkType != typeof(long)) continue;
                                var col = pk.Properties[0].GetColumnName();
                                try
                                {
                                    db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"" + table + "\"', '" + col + "'), (SELECT COALESCE(MAX(\"" + col + "\"), 0) FROM \"" + table + "\") + 1, false);");
                                }
                                catch (Exception seqTableEx)
                                {
                                    Console.WriteLine($"[Seed] Sequence sync skipped for {table}: {seqTableEx.Message}");
                                }
                            }
                            Console.WriteLine("[Seed] Identity sequences synced.");
                        }
                        catch (Exception seqEx)
                        {
                            Console.WriteLine($"[Seed] Sequence sync skipped: {seqEx.GetType().Name}: {seqEx.Message}");
                        }
                    }
                    // RBAC seed (Admin/User roles + permissions + admin user) runs through ABP DI
                    // so PermissionRegistry can be injected. SeedHelper is idempotent.
                    SeedHelper.SeedHostDb(Abp.Dependency.IocManager.Instance);
                    Console.WriteLine("[Seed] RBAC seed complete (Admin role + admin user).");
                }
                catch (Exception ex)
                {
                    // Full diagnostic — surface the real cause so silent seed failures are debuggable.
                    Console.WriteLine($"[Migration] FAILED: {ex.GetType().Name}: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"[Migration] InnerException: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
                    Console.WriteLine("[Migration] StackTrace:");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("[Migration] App continues without migration — admin user will not exist.");
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public class Program
    {
        // Runtime entry: WebHost is required because ABP Startup returns IServiceProvider.
        public static void Main(string[] args)
        {
            // Npgsql 7+ requires UTC DateTimes — enable legacy behavior for ABP compatibility
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .Run();
        }

        // Design-time entry for EF Core tools (dotnet ef migrations).
        // Without this, EF tools wait 5 minutes for IHost build (resolver default timeout)
        // and then SIGTERM any running dotnet process — killing live dev servers.
        // We expose a minimal IHost that EF tools resolve in milliseconds; the actual
        // DbContext is built by IDesignTimeDbContextFactory in the EntityFrameworkCore project.
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);
    }
}
