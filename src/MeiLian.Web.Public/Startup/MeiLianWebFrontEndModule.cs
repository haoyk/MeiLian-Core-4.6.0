using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MeiLian.Configuration;
using MeiLian.EntityFrameworkCore;

namespace MeiLian.Web.Public.Startup
{
    [DependsOn(
        typeof(MeiLianWebCoreModule)
    )]
    public class MeiLianWebFrontEndModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public MeiLianWebFrontEndModule(IHostingEnvironment env, MeiLianEntityFrameworkCoreModule MeiLianEntityFrameworkCoreModule)
        {
            _appConfiguration = env.GetAppConfiguration();
            MeiLianEntityFrameworkCoreModule.SkipDbSeed = true;
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = _appConfiguration["App:WebSiteRootAddress"] ?? "http://localhost:45776/";

            //Changed AntiForgery token/cookie names to not conflict to the main application while redirections.
            Configuration.Modules.AbpWebCommon().AntiForgery.TokenCookieName = "Public-XSRF-TOKEN";
            Configuration.Modules.AbpWebCommon().AntiForgery.TokenHeaderName = "Public-X-XSRF-TOKEN";

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.Navigation.Providers.Add<FrontEndNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MeiLianWebFrontEndModule).GetAssembly());
        }
    }
}
