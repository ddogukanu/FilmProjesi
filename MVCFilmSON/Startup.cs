using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCFilmSON.Startup))]
namespace MVCFilmSON
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
