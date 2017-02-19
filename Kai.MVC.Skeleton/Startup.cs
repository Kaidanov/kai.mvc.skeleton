using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kai.MVC.Skeleton.Startup))]
namespace Kai.MVC.Skeleton
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
             ConfigureAuth(app);
        }
    }
}
