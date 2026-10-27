using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IzinYonetimSistemi.EntityFrameworkCore
{
    [DependsOn(typeof(IzinYonetimSistemiCoreModule), typeof(AbpEntityFrameworkCoreModule))]
    public class IzinYonetimSistemiEntityFrameworkCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpEfCore().AddDbContext<IzinYonetimSistemiDbContext>(options =>
            {
                // connStr ABP tarafından CoreModule.DefaultNameOrConnectionString'ten resolve edilir
                // (Railway'de ConnectionStrings__Default env var). Ekstra ConfigurationBuilder/paket
                // gerekmez. ExistingConnection = nested UnitOfWork bağlantı paylaşımı.
                if (options.ExistingConnection != null)
                {
                    options.DbContextOptions.UseNpgsql(options.ExistingConnection);
                }
                else
                {
                    options.DbContextOptions.UseNpgsql(options.ConnectionString);
                }
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IzinYonetimSistemiEntityFrameworkCoreModule).GetAssembly());
        }
    }
}
