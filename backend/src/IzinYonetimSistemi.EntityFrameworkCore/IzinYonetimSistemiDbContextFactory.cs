using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IzinYonetimSistemi.EntityFrameworkCore
{
    public class IzinYonetimSistemiDbContextFactory : IDesignTimeDbContextFactory<IzinYonetimSistemiDbContext>
    {
        public IzinYonetimSistemiDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../IzinYonetimSistemi.Web.Host"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connStr = configuration.GetConnectionString("Default");
            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql(connStr);
            return new IzinYonetimSistemiDbContext(builder.Options);
        }
    }
}
