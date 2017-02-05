using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NeuralSite.Startup))]
namespace NeuralSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
