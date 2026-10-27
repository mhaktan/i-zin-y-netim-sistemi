using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace IzinYonetimSistemi
{
    [DependsOn(typeof(IzinYonetimSistemiCoreModule), typeof(AbpAutoMapperModule))]
    public class IzinYonetimSistemiApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                cfg.AddMaps(typeof(IzinYonetimSistemiApplicationModule).GetAssembly());
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IzinYonetimSistemiApplicationModule).GetAssembly());
        }
    }
}
