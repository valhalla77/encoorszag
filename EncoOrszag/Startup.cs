using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EncoOrszag.Startup))]
namespace EncoOrszag
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
