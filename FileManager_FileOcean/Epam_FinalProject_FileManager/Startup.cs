using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Epam_FinalProject_FileManager.Startup))]
namespace Epam_FinalProject_FileManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
