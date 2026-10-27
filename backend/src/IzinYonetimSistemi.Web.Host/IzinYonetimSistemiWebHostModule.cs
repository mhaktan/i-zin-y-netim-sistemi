using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using IzinYonetimSistemi.EntityFrameworkCore;

namespace IzinYonetimSistemi.Web.Host
{
    [DependsOn(typeof(IzinYonetimSistemiApplicationModule), typeof(IzinYonetimSistemiEntityFrameworkCoreModule), typeof(AbpAspNetCoreModule))]
    public class IzinYonetimSistemiWebHostModule : AbpModule
    {
        public override void PreInitialize()
        {
            // Expose all AppServices as dynamic API controllers
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(IzinYonetimSistemiApplicationModule).GetAssembly(),
                    moduleName: "app",
                    useConventionalHttpVerbs: true
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IzinYonetimSistemiWebHostModule).GetAssembly());
        }
    }
}
