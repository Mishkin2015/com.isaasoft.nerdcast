using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Isaasoft.Bugatti.Podcast.WebAPI.Startup))]
namespace Isaasoft.Bugatti.Podcast.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
