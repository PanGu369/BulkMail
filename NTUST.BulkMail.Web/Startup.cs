using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BulkMail.Startup))]
namespace BulkMail
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
