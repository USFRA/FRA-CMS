using Microsoft.Owin;
using Owin;
using RadCms.Providers;
using System;

[assembly: OwinStartup(typeof(RadCms.Web.Startup))]

namespace RadCms.Web
{
    public partial class Startup
    {
        public void ConfigAuth(IAppBuilder app)
        {
        }

    }
}
