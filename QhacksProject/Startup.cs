using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QhacksProject.Startup))]
namespace QhacksProject
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
