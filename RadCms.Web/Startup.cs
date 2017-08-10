using Microsoft.Owin;
using Owin;
using RadCms.Data;
using System.Data.Entity;

[assembly: OwinStartup(typeof(RadCms.Web.Startup))]
namespace RadCms.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            RegisterDependencies(app);
            ConfigAuth(app);
#if DEBUG
            Database.SetInitializer(new DatabaseInitializer());
#endif
        }
    }
}
